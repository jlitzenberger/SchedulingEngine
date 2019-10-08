using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Autofac;
using Newtonsoft.Json;
using CS.Common.Utilities;
using CS.Common.Utilities.Email.Client;
using CS.Common.Utilities.Email.Models;
using CS.Common.Utilities.Email.Services;
using CS.Common.Utilities.Email.Settings;
using Nomination.BusinessLayer.Interfaces;
using Nomination.BusinessLayer.Interfaces.Naesb;
using Nomination.BusinessLayer.Services.Naesb;
using Nomination.BusinessLayer.Services.Requestors;
using Nomination.Domain.Incidents;
using Nomination.Domain.Naesb;
using Nomination.Persistence.Naesb;
using HttpClientHandler = Nomination.BusinessLayer.Services.Requestors.HttpClientHandler;

namespace SchedulingMonitor
{
    public class NaesbEventEmail : NaesbEventMonitor
    {
        public short? OffSet { get; set; }
        public DateTime? Processed { get; set; }
    }

    public class EmailSettings : IEmailSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }

        public EmailSettings()
        {
            Host = Properties.Settings.Default.SmtpHost;
            Port = Convert.ToInt32(Properties.Settings.Default.SmtpPort);
        }
    }

    class Program
    {
        public static IContainer Container;

        private static ContainerBuilder ContainerBuilder()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<HttpClientHandler>().As<IHttpClient>();

            builder.RegisterType<IncidentRequestor>().As<IResourse<Incident>>();
            builder.Register(context => IncidentRequestorSetting.Get(
                Properties.Settings.Default.RemedyUriCredentials,
                Properties.Settings.Default.ContentApplicationName,
                Properties.Settings.Default.ContentUserId)
            );

            builder.RegisterType<EmailSettings>().As<IEmailSettings>();
            //builder.Register(context => SetEmailSetting.Get(
            //    Properties.Settings.Default.SmtpHost,
            //    Convert.ToInt32(Properties.Settings.Default.SmtpPort))
            //);
            builder.RegisterType<WecEmailClient>().As<IEmailClient>();
            builder.RegisterType<EmailService>().As<IEmailService>();

            builder.RegisterType<NaesbEventMonitorRepository>().As<INaesbEventMonitorRepository>();
            builder.RegisterType<NaesbEventMonitorGet>().As<INaesbEventMonitorGet>();
            builder.RegisterType<NaesbEventMonitorGetEventMonitors>().As<INaesbEventMonitorGetEventMonitors>();
            builder.RegisterType<NaesbEventMonitorUpdate>().As<INaesbEventMonitorUpdate>();

            builder.RegisterType<NaesbEventRepository>().As<INaesbEventRepository>();

            builder.RegisterType<NaesbEventNotificationRepository>().As<INaesbEventNotificationRepository>();
            builder.RegisterType<NaesbEventNotificationGetNotDelivered>().As<INaesbEventNotificationGetNotDelivered>();
            builder.RegisterType<NaesbEventNotificationCreate>().As<INaesbEventNotificationCreate>();
            builder.RegisterType<NaesbEventNotificationUpdate>().As<INaesbEventNotificationUpdate>();

            return builder;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("============================================================================");
            Console.WriteLine("==Process Start Date: " + DateTime.Now);
            Console.WriteLine("============================================================================");

            try
            {
                //build AutoFac container Dependency Injection
                Container = ContainerBuilder().Build();

                var runTime = DateTime.Now;
                //runTime = new DateTime(2019, 06, 17, 12, 11, 00);

                ProcessNaesbEventMonitor(runTime);        // calculate past due naesb notifications and add past due events to naesb event notification
                ProcessNaesbEventNotifications(runTime);  // process the naesb event notifications
            }
            catch (Exception ex)
            {
                CreateIncident(ex);  //write to Remedy
            }
            
            Console.WriteLine("============================================================================");
            Console.WriteLine("==Process End Date: " + DateTime.Now);
            Console.WriteLine("============================================================================");

            Environment.ExitCode = 0;
        }

        private static void ProcessNaesbEventMonitor(DateTime runTime)
        {
            //get a list of all the past due events
            List<NaesbEventEmail> objs = RetrieveNaesbEventMonitors(runTime);

            if (objs != null && objs.Count > 0)
            {
                //create the naesb event notification to be delivered
                CreateNaesbEventNotification(objs);
            }
            else
            {
                Console.WriteLine("==Processing:[Naesb Event Monitor] No past due Naesb event(s)");
            }
        }
        private static void ProcessNaesbEventNotifications(DateTime runTime)
        {
            //get the naesbEventNotifications to be processed
            List<NaesbEventNotification> objs = Container.Resolve<INaesbEventNotificationGetNotDelivered>().Invoke();

            if (objs != null && objs.Count > 0)
            {
                //Console.WriteLine("==Processing: Event Notifications -> DELIVERING...");
                Console.WriteLine("==Processing:[Event Notification(s)] [ Count: " + objs.Count + "] " + "to Send");

                int i = 1;
                //process all of the event notifications
                foreach (var item in objs)
                {
                    try
                    {
                        ProcessEmail(item);
                        Console.WriteLine("==Processing:[Event Notification(s)] [ " + i + " " + "of" + " " + objs.Count + " ] Subject: " + item.Subject + " -> SENT");

                        //update process time
                        Container.Resolve<INaesbEventNotificationUpdate>().Invoke(item.Id, runTime);

                        i++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("==Processing Error:");
                        Console.WriteLine(ex);

                    }
                }
            }
            else
            {
                Console.WriteLine("==Processing:[Event Notification(s)] No Event Notification(s)");
            }
        }
        private static void CreateNaesbEventNotification(List<NaesbEventEmail> objs)
        {
            //create the email body
            string emailBody = CreateEmailBody(objs);

            //create the event notification
            var eventNotification = new NaesbEventNotification
            {
                Source = "Scheduling Monitor",
                To = Properties.Settings.Default.EmailTo,
                From = Properties.Settings.Default.EmailFrom,
                Priority = MailPriority.Normal.ToString().ToUpper(),
                Subject = "Naesb Past Due Events",
                Body = emailBody,
                IsHtml = true,
                LastUpdateUserId = Properties.Settings.Default.ContentUserId
            };

            //add missed naesb event to naesb event notification
            Container.Resolve<INaesbEventNotificationCreate>().Invoke(eventNotification);
        }
        private static List<NaesbEventEmail> RetrieveNaesbEventMonitors(DateTime runTime)
        {
            int i = 0;

            //get list of possible past due naesb event monitors
            //where -->  active
            //           AND
            //          (runtime.TimeOfDay > than naesb_event_monitor.EventMonitorTime && naesb_event_monitor.LastCheckedTime = NULL
            //           OR
            //           runtime.TimeOfDay > than naesb_event_monitor.EventMonitorTime && naesb_event_monitor.LastCheckedTime.Date and runTime.Date difference is more than 1 day ... at least next day)
            List<NaesbEventMonitor> list = Container.Resolve<INaesbEventMonitorGetEventMonitors>().Invoke(runTime);

            if (list != null && list.Count > 0)
            {
                Console.WriteLine("==Processing:[Naesb Event Monitor] Event Notification(s) -> DETERMINING... ");

                //List<NaesbEventMonitor> notifications = new List<NaesbEventMonitor>();
                List<NaesbEventEmail> notifications = new List<NaesbEventEmail>();

                //loop through each naesb event monitor to determine possible past due naesb event
                foreach (var item in list)
                {
                    DateTime naesbEventMonitorCurrentDateTime = new DateTime(runTime.Year, runTime.Month, runTime.Day, item.EventMonitorTime.Hours, item.EventMonitorTime.Minutes, item.EventMonitorTime.Seconds);
                    if (runTime > naesbEventMonitorCurrentDateTime && (item.LastCheckedTime == null || runTime > item.LastCheckedTime))
                    {
                        //get current naesb event
                        NaesbEvent naesbEvent = Container.Resolve<INaesbEventGet>().Invoke(item.EventType, item.Pipeline, item.Utility, item.Cycle);
                        
                        if (naesbEvent.FileType == "RFC")
                        {
                            //if true the naesb event is past due
                            if ((naesbEvent.ProcessedTime == null || ((runTime.Date - naesbEvent.ProcessedTime.Value.Date).TotalDays > 0 && runTime.TimeOfDay > naesbEvent.CycleEnd)) && naesbEvent.On == true)
                            {
                                Console.WriteLine("==Processing: Event Notification -> PAST DUE MONITOR EVENT");
                                Console.WriteLine("==Processing: Event Notification -> [EventTypeDesc: " + item.EventTypeDescription + "] [EventType: " + item.EventType + "] [Cycle: " + item.Cycle + "] [Utility: " + item.Utility + "] [Pipeline: " + item.Pipeline + "]");

                                notifications.Add(new NaesbEventEmail
                                {
                                    Id = item.Id,
                                    Cycle = item.Cycle,
                                    CycleDescription = item.CycleDescription,
                                    SortSeq = item.SortSeq,
                                    Pipeline = item.Pipeline,
                                    Utility = item.Utility,
                                    EventType = item.EventType,
                                    EventTypeDescription = item.EventTypeDescription,
                                    Processed = naesbEvent.ProcessedTime,
                                    EventMonitorTime = item.EventMonitorTime,
                                    LastCheckedTime = item.LastCheckedTime,
                                    ActiveStart = item.ActiveStart,
                                    ActiveEnd = item.ActiveEnd,
                                    LastUpdateTime = item.LastUpdateTime,
                                    LastUpdateUserId = item.LastUpdateUserId,
                                    OffSet = naesbEvent.OffSet
                                });
                                Console.WriteLine("==Processing: Event Notification -> ADDED TO NOTIFICATIONS");

                                i++;
                            }
                        }
                        if (naesbEvent.FileType == "CR" || naesbEvent.FileType == "OSQ")
                        {
                            //if true the naesb event is past due
                            if ((naesbEvent.ProcessedTime == null || (runTime.Date - naesbEvent.ProcessedTime.Value.Date).TotalDays > 0) && naesbEvent.On == true)
                            {
                                Console.WriteLine("==Processing: Event Notification -> PAST DUE MONITOR EVENT");
                                Console.WriteLine("==Processing: Event Notification -> [EventTypeDesc: " + item.EventTypeDescription + "] [EventType: " + item.EventType + "] [Cycle: " + item.Cycle + "] [Utility: " + item.Utility + "] [Pipeline: " + item.Pipeline + "]");
 
                                notifications.Add(new NaesbEventEmail
                                {
                                    Id = item.Id,
                                    Cycle = item.Cycle,
                                    CycleDescription = item.CycleDescription,
                                    SortSeq = item.SortSeq,
                                    Pipeline = item.Pipeline,
                                    Utility = item.Utility,
                                    EventType = item.EventType,
                                    EventTypeDescription = item.EventTypeDescription,
                                    Processed = naesbEvent.ProcessedTime,
                                    EventMonitorTime = item.EventMonitorTime,
                                    LastCheckedTime = item.LastCheckedTime,
                                    ActiveStart = item.ActiveStart,
                                    ActiveEnd = item.ActiveEnd,
                                    LastUpdateTime = item.LastUpdateTime,
                                    LastUpdateUserId = item.LastUpdateUserId,
                                    OffSet = naesbEvent.OffSet

                                });
                                Console.WriteLine("==Processing: Event Notification -> ADDED TO NOTIFICATIONS");

                                i++;
                            }
                        }
                    }

                    //update NaesbEventMonitors LastCheckedTime
                    Container.Resolve<INaesbEventMonitorUpdate>().Invoke(item.Id, runTime);
                }

                if (i > 0)
                {
                    Console.WriteLine("==Processing:[Naesb Event Monitor] " + i + " Event Notification(s) -> CREATED");
                }

                return notifications;
            }

            return null;
        }
        
        private static void ProcessEmail(NaesbEventNotification obj)
        {
            try
            {
                var priority = new MailPriority();
                switch (obj.Priority.ToUpper())
                {
                    case "HIGH":
                        priority = MailPriority.High;
                        break;
                    case "LOW":
                        priority = MailPriority.Low;
                        break;
                    default:
                        priority = MailPriority.Normal;
                        break;
                }

                var email = new CS.Common.Utilities.Email.Models.Email
                {
                    Body = obj.Body,
                    CC = obj.Cc,
                    From = obj.From,
                    To = obj.To,
                    IsBodyHtml = obj.IsHtml,
                    Priority = priority,
                    Subject = obj.Subject
                };

                ProcessEmail(email);
            }
            catch (Exception ex)
            {
                CreateIncident(ex);
            }
        }
        private static void ProcessEmail(Email email)
        {
            Container.Resolve<IEmailService>().Send(email);
        }
        private static string CreateEmailBody(List<NaesbEventEmail> objs)
        {
            string messageBody = "<font>The following Scheduled Naesb Events did not occur: </font><br><br>";

            string htmlTableStart = "<table style=\"border-collapse:collapse; text-align:center;\" >";
            string htmlTableEnd = "</table>";
            string htmlHeaderRowStart = "<tr style =\"background-color:#6FA1D2; color:#ffffff;\">";
            string htmlHeaderRowEnd = "</tr>";
            string htmlTrStart = "<tr style =\"color:#555555;\">";
            string htmlTrEnd = "</tr>";
            string htmlTdStart = "<td style=\" border-color:#5c87b2; border-style:solid; border-width:thin; padding: 5px;\">";
            string htmlTdEnd = "</td>";

            messageBody += htmlTableStart;
            messageBody += htmlHeaderRowStart;
            messageBody += htmlTdStart + "GasDay " + htmlTdEnd;
            messageBody += htmlTdStart + "Cycle " + htmlTdEnd;
            messageBody += htmlTdStart + "Pipeline " + htmlTdEnd;
            messageBody += htmlTdStart + "Utility " + htmlTdEnd;
            messageBody += htmlTdStart + "Event Type " + htmlTdEnd;
            messageBody += htmlTdStart + "Event Type Description " + htmlTdEnd;
            messageBody += htmlTdStart + "Last Processed " + htmlTdEnd;
            messageBody += htmlHeaderRowEnd;

            foreach (var item in objs)
            {
                messageBody = messageBody + htmlTrStart;
                messageBody = messageBody + htmlTdStart + DateTime.Now.AddDays(double.Parse(item.OffSet.ToString())).ToShortDateString() + htmlTdEnd;
                messageBody = messageBody + htmlTdStart + item.Cycle + htmlTdEnd;
                messageBody = messageBody + htmlTdStart + item.Pipeline + htmlTdEnd;
                messageBody = messageBody + htmlTdStart + item.Utility + htmlTdEnd;
                messageBody = messageBody + htmlTdStart + item.EventType + htmlTdEnd;
                messageBody = messageBody + htmlTdStart + item.EventTypeDescription + htmlTdEnd;
                messageBody = messageBody + htmlTdStart + item.Processed + htmlTdEnd;
                messageBody = messageBody + htmlTrEnd;
            }

            messageBody = messageBody + htmlTableEnd;

            return messageBody;
        }

        private static void CreateIncident(Exception originalException)
        {
            try
            {
                Console.WriteLine("==Processing Error: [Sending invalid data to Remedy]");

                string stacktrace = Stacktrace(originalException);

                HttpResponseMessage hrm = Container.Resolve<IResourse<Incident>>()
                    .Post(
                        new Uri(Properties.Settings.Default.RemedyUriHost + Properties.Settings.Default.RemedyUri),
                        new Incident
                        {
                            AssignedSupportGroup = Properties.Settings.Default.RemedyAssignedSupportGroup,
                            ClientService = Properties.Settings.Default.RemedyClientService,
                            Company = Properties.Settings.Default.RemedyCompany,
                            Impact = Properties.Settings.Default.RemedyImpact,
                            Urgency = Properties.Settings.Default.RemedyUrgency,
                            Notes = stacktrace,
                            Summary = "Scheduling Monitor Failed."
                        }
                    );

                var incident = hrm.Headers.Single(m => m.Key == "X-Id");
                string incidentNumber = incident.Value.ToList()[0];

                Console.WriteLine("==Processing Error: [SUCCESS]-" + incidentNumber + "" + " created in Remedy");
            }
            catch (HttpResponseException exception)
            {
                Console.WriteLine("==Processing Error: [FAILED-]-failed to send invalid data to Remedy");
                Console.WriteLine("*************Http Response Error***************");
                Console.WriteLine("Http Status Code:" + exception.Response.StatusCode);

                foreach (var header in exception.Response.Headers)
                {
                    Console.WriteLine(header.Key + ", " + header.Value.ToList()[0]);
                }

                Console.WriteLine("*************Http Response Error***************");
            }
            catch (Exception exception)
            {
                Console.WriteLine("==Processing Error: [FAILED-]-failed to send invalid data to Remedy");
                Console.WriteLine(exception);

                throw originalException;
            }
        }
        private static string Stacktrace(Exception ex)
        {
            string stacktrace = string.Empty;
            if (ex?.InnerException == null)
            {
                stacktrace = ex.Message;
            }
            else
            {
                stacktrace = ex?.InnerException?.StackTrace;
            }

            return stacktrace;
        }
    }
}
