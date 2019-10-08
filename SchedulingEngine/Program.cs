using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Xml;
using Autofac;
using CS.Common.Utilities;
using Nomination.BusinessLayer.Interfaces;
using Nomination.BusinessLayer.Interfaces.Naesb;
using Nomination.BusinessLayer.Services;
using Nomination.BusinessLayer.Services.RequestForConfirmation;
using Nomination.BusinessLayer.Services.ConfirmationResponse.Event;
using Nomination.BusinessLayer.Services.ConfirmationResponse.Naesb;
using Nomination.BusinessLayer.Services.Naesb;
using Nomination.BusinessLayer.Services.QuickResponse.Naesb;
using Nomination.BusinessLayer.Services.RequestForConfirmation.Event;
using Nomination.BusinessLayer.Services.RequestForConfirmation.Naesb;
using Nomination.BusinessLayer.Services.Requestors;
using Nomination.BusinessLayer.Services.ScheduledQuantity.Event;
using Nomination.BusinessLayer.Services.ScheduledQuantity.Naesb;
using Nomination.Persistence.RequestForConfirmation;
using Nomination.Persistence.ConfirmationResponse;
using Nomination.Persistence.ScheduledQuantity;
using Nomination.Persistence.Naesb;
using Nomination.Domain.Incidents;
using Nomination.Domain.Naesb;
using Nomination.Domain.RequestForConfirmation;
using Nomination.Domain.ConfirmationResponse;
using Nomination.Domain.QuickResponse.Naesb;
using Nomination.Domain.RequestForConfirmation.Naesb;
using Nomination.Domain.ScheduledQuantities;
using HttpClientHandler = Nomination.BusinessLayer.Services.Requestors.HttpClientHandler;

namespace SchedulingEngine
{
    class Program
    {
        private static IContainer Container;
        private static ContainerBuilder ContainerBuilder()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.Register(context => ServiceSettings.Get(
                Properties.Settings.Default.ContentUserId,
                Properties.Settings.Default.Environment)
            );

            builder.RegisterType<NaesbRepository>().As<INaesbRepository>();

            builder.RegisterType<NaesbEventRepository>().As<INaesbEventRepository>();
            builder.RegisterType<NaesbEventGet>().As<INaesbEventGet>();
            builder.RegisterType<NaesbEventChange>().As<INaesbEventChange>();
            builder.RegisterType<NaesbEventUpdate>().As<INaesbEventUpdate>();

            builder.RegisterType<NaesbRequestForConfirmationCreate>().As<INaesbRequestForConfirmationCreate>();
            builder.RegisterType<NaesbRequestForConfirmationHeaderGet>().As<INaesbRequestForConfirmationHeaderGet>();
            builder.RegisterType<NaesbRequestForConfirmationGet>().As<INaesbRequestForConfirmationGet>();
            builder.RegisterType<RequestForConfirmationRepository>().As<IRequestForConfirmationRepository>();
            builder.RegisterType<RequestForConfirmationEventRepository>().As<IRequestForConfirmationEventRepository>();
            builder.RegisterType<RequestForConfirmationGet>().As<IRequestForConfirmationGet>();
            builder.RegisterType<RequestForConfirmationEventCreate>().As<IRequestForConfirmationEventCreate>();

            builder.RegisterType<NaesbEventProcessRespository>().As<INaesbEventProcessRepository>();
            builder.RegisterType<NaesbEventProcessCreate>().As<INaesbEventProcessCreate>();
            builder.RegisterType<NaesbEventProcessChange>().As<INaesbEventProcessChange>();
            builder.RegisterType<NaesbEventProcessUpdate>().As<INaesbEventProcessUpdate>();
            builder.RegisterType<NaesbEventProcessRfcCompletion>().As<INaesbEventProcessRfcCompletion>();
            builder.RegisterType<NaesbEventProcessGet>().As<INaesbEventProcessGet>();
            builder.RegisterType<NaesbEventProcessError>().As<INaesbEventProcessError>();

            builder.RegisterType<NaesbPipelineRepository>().As<INaesbPipelineRepository>();
            builder.RegisterType<NaesbPipelineGet>().As<INaesbPipelineGet>();
            builder.RegisterType<NaesbPipelineGetByPipeline>().As<INaesbPipelineGetByPipeline>();

            builder.RegisterType<NaesbUtilityRepository>().As<INaesbUtilityRepository>();
            builder.RegisterType<NaesbUtilityGet>().As<INaesbUtilityGet>();
            builder.RegisterType<NaesbUtilityGetByUtility>().As<INaesbUtilityGetByUtility>();
            
            builder.RegisterType<NaesbQuickResponseHeaderGet>().As<INaesbQuickResponseHeaderGet>();
            builder.RegisterType<NaesbQuickResponseGet>().As<INaesbQuickResponseGet>();
            builder.RegisterType<NaesbQuickResponseCreate>().As<INaesbQuickResponseCreate>();

            builder.RegisterType<NaesbConfirmationResponseCreate>().As<INaesbConfirmationResponseCreate>();
            builder.RegisterType<ConfirmationResponseEventRepository>().As<IConfirmationResponseEventRepository>();
            builder.RegisterType<ConfirmationResponseEventGet>().As<IConfirmationResponseEventGet>();
            builder.RegisterType<ConfirmationResponseEventCreate>().As<IConfirmationResponseEventCreate>();
            builder.RegisterType<ConfirmationResponseEventProcess>().As<IConfirmationResponseEventProcess>();

            builder.RegisterType<NaesbOperatorScheduledQuantitiesCreate>().As<INaesbOperatorScheduledQuantitiesCreate>();
            builder.RegisterType<ScheduledQuantityEventRepository>().As<IScheduledQuantityEventRepository>();
            builder.RegisterType<ScheduledQuantityEventGet>().As<IScheduledQuantityEventGet>();
            builder.RegisterType<ScheduledQuantityEventCreate>().As<IScheduledQuantityEventCreate>();
            builder.RegisterType<ScheduledQuantityEventProcess>().As<IScheduledQuantityEventProcess>();

            builder.RegisterType<NaesbEventGetRfcsToProcess>().As<INaesbEventGetRfcsToProcess>();

            builder.RegisterType<HttpClientHandler>().As<IHttpClient>();
            builder.RegisterType<IncidentRequestor>().As<IResourse<Incident>>();
            builder.Register(context => IncidentRequestorSetting.Get(
                Properties.Settings.Default.RemedyUriCredentials,
                Properties.Settings.Default.ContentApplicationName,
                Properties.Settings.Default.ContentUserId)
            );

            return builder;
        }

        static void Main(string[] args)
        {
            DateTime processStart = DateTime.Now;
            //processStart = new DateTime(2019, 07, 16, 11, 15, 00);

            var eventType = Properties.Settings.Default.EventType;
            //eventType = "request-for-confirmation";
            //eventType = "confirmation-response";
            //eventType = "operator-scheduled-quantities";

            Console.WriteLine("============================================================================");
            Console.WriteLine("==Process Start Date: " + processStart);
            Console.WriteLine("============================================================================");

            try
            {
                //build AutoFac container Dependency Injection
                Container = ContainerBuilder().Build();

                if (eventType == "request-for-confirmation")
                {
                    ProcessRequestForConfirmations(processStart);
                }
                if (eventType == "confirmation-response")
                {
                    ProcessConfirmationResponses(processStart);
                }
                if (eventType == "operator-scheduled-quantities")
                {
                    ProcessScheduledQuantities(processStart);
                }
            }
            catch (Exception ex)
            {
                CreateIncident(ex, null);  //write to Remedy
            }
            
            Console.WriteLine("==Process End Date: " + DateTime.Now);
            Console.WriteLine("============================================================================");

            Environment.ExitCode = 0;
        }

        private static void ProcessRequestForConfirmations(DateTime processStart)
        {
            //return all valid rfcs to be processed
            var rfcs = Container.Resolve<INaesbEventGetRfcsToProcess>().Invoke(processStart);

            if (rfcs != null && rfcs.Count > 0)
            {
                Console.WriteLine("==Processing:[" + Properties.Settings.Default.EventType + "'s] [File Count: " + rfcs.Count + "]");

                int i = 1;
                foreach (var rfc in rfcs)
                {
                    try
                    {
                        NaesbEvent naesbEvent = Container.Resolve<INaesbEventGet>().Invoke("RFC", rfc.PartyIndentificaton.PipelineEntity, rfc.PartyIndentificaton.UtilityEntity, rfc.Cycle);

                        //this will always be current because if an rfc for a cycle doesn't go out...then it never does for that cycle.
                        rfc.GasDay = processStart.AddDays(naesbEvent.OffSet != null ? Convert.ToDouble(naesbEvent.OffSet) : 0);

                        Console.WriteLine("==Processing: [" + Properties.Settings.Default.EventType + "] [" + i + " of " + rfcs.Count + "] [Cycle: " + rfc.Cycle + "] [Utility: " + rfc.PartyIndentificaton.UtilityEntity + "] [Pipeline: " + rfc.PartyIndentificaton.PipelineEntity + "]");

                        ProcessRequestForConfirmation(rfc, processStart);

                        //update naesb event process
                        DateTime now = DateTime.Now;
                        Container.Resolve<INaesbEventUpdate>().Invoke(naesbEvent.Id, now);
                        Console.WriteLine("==Processing:  4. Updated the naesb event process Processed Time: " + now);

                        i++;

                        Console.WriteLine("==Processing: [" + Properties.Settings.Default.EventType + "] <<SUCCESSFUL>>");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("==Processing Error:");
                        Console.WriteLine(ex);
                        i++;

                        CreateIncident(ex, null);

                        Console.WriteLine("==Processing: [" + Properties.Settings.Default.EventType + "] <<UNSUCCESSFUL>>");
                    }
                }
            }
            else
            {
                Console.WriteLine("==Processing:[" + Properties.Settings.Default.EventType + "'s] None to Process");
            }

            Console.WriteLine("============================================================================");
        }
        private static void ProcessConfirmationResponses(DateTime processStart)
        {
            //get all of the ConfirmationResponse files from BizConnect
            var files = GetFiles("ConfirmationResponse");

            if (files != null && files.Count > 0)
            {
                Console.WriteLine("==Processing:[" + Properties.Settings.Default.EventType + "'s] [File Count: " + files.Count + "]");

                int i = 1;
                foreach (var file in files)
                {
                    NaesbEventProcess nep = null;

                    try
                    {
                        Console.WriteLine("==Processing: [" + Properties.Settings.Default.EventType + "] [" + i + " of " + files.Count + "] [File: " + file.DirectoryName + "\\" + file.Name + "]");
                        
                        //check if the naesb event flag is on for this event/pipeline/utility/cycle
                        var naesbEvent = GetCrNaesbEvent(file);
                        if (naesbEvent != null && naesbEvent.On == true)
                        {
                            nep = ProcessConfirmationResponse(file, processStart);

                            if (nep != null)
                            {
                                //update naesb event
                                DateTime now = DateTime.Now;
                                Container.Resolve<INaesbEventUpdate>().Invoke(naesbEvent.Id, now);
                                Console.WriteLine("==Processing:  2. Updated the naesb event process 'ProcessedTime': [" + now + "]");

                                //send quick response
                                SendQuickResponse(file);

                                //delete the file
                                DeleteNaesbFile(file, i, files.Count);

                                Console.WriteLine("==Processing: [" + Properties.Settings.Default.EventType + "] <<SUCCESSFUL>>");
                            }
                            else
                            {
                                //archive the file
                                ArchiveNaesbFile(file, i, files.Count);

                                Console.WriteLine("==Processing: [" + Properties.Settings.Default.EventType + "] <<UNSUCCESSFUL>>");
                            }
                        }
                        else
                        {
                            Console.WriteLine("==Processing: [" + Properties.Settings.Default.EventType + "]  **Naesb event: [Cycle: " + naesbEvent?.Cycle + "] [Utility: " + naesbEvent?.Utility + "] [Pipeline: " + naesbEvent?.Pipeline + "] is not active or does not exist.)");

                            //archive the file
                            ArchiveNaesbFile(file, i, files.Count);

                            Console.WriteLine("==Processing: [" + Properties.Settings.Default.EventType + "] <<SUCCESSFUL with WARNING>>");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("==Processing Error:");
                        Console.WriteLine(ex);

                        if (nep?.Id > 0) //if the id == 0 it failed before the naesbEventProcess row was created
                        {
                            string stacktrace = Stacktrace(ex);
                            Container.Resolve<INaesbEventProcessError>().Invoke(nep.Id, "CR", stacktrace);
                        }

                        CreateIncident(ex, file);

                        //archive the file
                        ArchiveNaesbFile(file, i, files.Count);

                        Console.WriteLine("==Processing: [" + Properties.Settings.Default.EventType + "] <<UNSUCCESSFUL>>");
                    }

                    i++;
                }
            }
            else
            {
                Console.WriteLine("==Processing:[" + Properties.Settings.Default.EventType + "'s] None to Process");
            }

            Console.WriteLine("============================================================================");
        }
        private static void ProcessScheduledQuantities(DateTime processStart)
        {
            //get all of the OperatorScheduledQuantity files from BizConnect
            var files = GetFiles("OperatorScheduledQuantity");

            if (files != null && files.Count > 0)
            {
                Console.WriteLine("==Processing:[" + Properties.Settings.Default.EventType + "'s] [File Count: " + files.Count + "]");

                int i = 1;
                foreach (var file in files)
                {
                    NaesbEventProcess nep = null;

                    try
                    {
                        Console.WriteLine("==Processing: [" + Properties.Settings.Default.EventType + "] [" + i + " of " + files.Count + "] [File: " + file.DirectoryName + "\\" + file.Name + "]");

                        //check if the naesb event flag is on for this event/pipeline/utility/cycle
                        var naesbEvent = GetOsqNaesbEvent(file);
                        if (naesbEvent != null && naesbEvent.On == true)
                        {
                            nep = ProcessScheduledQuantity(file, processStart);

                            if (nep != null)
                            {
                                //update naesb event
                                DateTime now = DateTime.Now;
                                Container.Resolve<INaesbEventUpdate>().Invoke(naesbEvent.Id, now);
                                Console.WriteLine("==Processing:  2. Updated the naesb event process 'ProcessedTime': [" + now + "]");

                                //delete the file
                                DeleteNaesbFile(file, i, files.Count);

                                Console.WriteLine("==Processing: [" + Properties.Settings.Default.EventType + "] <<SUCCESSFUL>>");
                            }
                            else
                            {
                                //archive the file
                                ArchiveNaesbFile(file, i, files.Count);

                                Console.WriteLine("==Processing: [" + Properties.Settings.Default.EventType + "] <<UNSUCCESSFUL>>");
                            }
                        }
                        else
                        {
                            Console.WriteLine("==Processing: [" + Properties.Settings.Default.EventType + "]  **Naesb event: [Cycle: " + naesbEvent?.Cycle + "] [Utility: " + naesbEvent?.Utility + "] [Pipeline: " + naesbEvent?.Pipeline + "] is not active or does not exist.)");

                            //archive the file
                            ArchiveNaesbFile(file, i, files.Count);

                            Console.WriteLine("==Processing: [" + Properties.Settings.Default.EventType + "] <<SUCCESSFUL with WARNING>>");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("==Processing Error:");
                        Console.WriteLine(ex);

                        if (nep?.Id > 0) //if the id == 0 it failed before the naesbEventProcess row was created
                        {
                            string stacktrace = Stacktrace(ex);
                            Container.Resolve<INaesbEventProcessError>().Invoke(nep.Id, "OSQ", stacktrace);
                        }

                        CreateIncident(ex, file);

                        //archive the file
                        ArchiveNaesbFile(file, i, files.Count);

                        Console.WriteLine("==Processing: [" + Properties.Settings.Default.EventType + "] <<UNSUCCESSFUL>>");
                    }

                    i++;
                }
            }
            else
            {
                Console.WriteLine("==Processing:[" + Properties.Settings.Default.EventType + "'s] None to Process");
            }

            Console.WriteLine("============================================================================");
        }

        private static void ProcessRequestForConfirmation(RequestForConfirmation rfc, DateTime processStart)
        {
            string naesbFileName = string.Empty;
            int id = 0;

            try
            {
                if (rfc != null)
                {
                    Console.WriteLine("==Processing:  1. Retrieved rfc from the repository for Gas Day: " + rfc.GasDay.ToShortDateString());

                    //create an Naesb file name
                    naesbFileName = FormatNaesbRequestForConfirmationFileName(rfc);

                    //create request for confirmation
                    id = Container.Resolve<INaesbRequestForConfirmationCreate>().Invoke(processStart, naesbFileName, rfc);
                    Console.WriteLine("==Processing:  2. Created request for confirmation naesb event process: [Id: " + id + "] [Cycle: " + rfc.Cycle + "] [Utility: " + rfc.PartyIndentificaton.UtilityEntity + "] [Pipeline: " + rfc.PartyIndentificaton.PipelineEntity + "]");

                    //map domain rfc model to the Naesb rfc model.
                    NaesbRequestForConfirmation nrfc = Container.Resolve<INaesbRequestForConfirmationGet>().Invoke(rfc);
                    
                    //get the newly created naesb event process that was processed
                    NaesbEventProcess obj = Container.Resolve<INaesbEventProcessGet>().Invoke(id);
                    if (obj != null)
                    {
                        //save rfc mxl file to BizConnect
                        SaveNaesbRequestForConfirmationFile(nrfc, naesbFileName);
                        Console.WriteLine("==Processing:  3. Created rfc naesb file: [" + Properties.Settings.Default.NaesbOutboundUnc + naesbFileName + "]");
                    }
                    else
                    {
                        Console.WriteLine("==Processing:  The request for confirmation was NOT created");
                    }
                }
                else
                {
                    Console.WriteLine("==Processing: No rfc to process");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("==Processing Error:");
                Console.WriteLine(ex);

                if (id > 0) //if the id == 0 it failed before the naesbEventProcess row was created
                {
                    string stacktrace = Stacktrace(ex);

                    Container.Resolve<INaesbEventProcessError>().Invoke(id, "RFC", stacktrace);
                }
                
                CreateIncident(ex, null);
            }
        }
        private static NaesbEventProcess ProcessConfirmationResponse(FileInfo file, DateTime processStart)
        {
            int id = 0;

            try
            {
                //create confirmation response naesb event process
                id = Container.Resolve<INaesbConfirmationResponseCreate>().Invoke(processStart, file);

                //get the newly created naesb event process that was processed
                NaesbEventProcess obj = Container.Resolve<INaesbEventProcessGet>().Invoke(id);
                if (obj != null)
                {
                    Console.WriteLine("==Processing:  1. Created confirmation response naesb event process: [Id: " + id + "] [Cycle: " + obj.Cycle + "] [Utility: " + obj.Utility + "] [Pipeline: " + obj.Pipeline + "]");
                }
                else
                {
                    Console.WriteLine("==Processing:  The confirmation response was not created");
                }

                return obj;
            }
            catch (NaesbError ex)
            {
                if (ex.ReasonCode == "101")
                {
                    Console.WriteLine("==Processing: **Pipeline/Utility/Cycle naesb event not found.**");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("==Processing Error:");
                Console.WriteLine(ex);

                if (id > 0) //if the id == 0 it failed before the naesbEventProcess row was created
                {
                    string stacktrace = Stacktrace(ex);
                    Container.Resolve<INaesbEventProcessError>().Invoke(id, "CR", stacktrace);
                }

                CreateIncident(ex, file);
            }

            return null;
        }
        private static NaesbEventProcess ProcessScheduledQuantity(FileInfo file, DateTime processStart)
        {
            int id = 0;

            try
            {
                //create operator scheduled quantities naesb event process
                id = Container.Resolve<INaesbOperatorScheduledQuantitiesCreate>().Invoke(processStart, file);

                //get the newly created naesb event process that was processed
                NaesbEventProcess obj = Container.Resolve<INaesbEventProcessGet>().Invoke(id);
                if (obj != null)
                {
                    Console.WriteLine("==Processing:  1. Created operator scheduled quantities naesb event process: [Id: " + id + "] [Cycle: " + obj.Cycle + "] [Utility: " + obj.Utility + "] [Pipeline: " + obj.Pipeline + "]");
                }
                else
                {
                    Console.WriteLine("==Processing:  The operator scheduled quantities was NOT created");
                }

                return obj;
            }
            catch (NaesbError ex)
            {
                if (ex.ReasonCode == "101")
                {
                    Console.WriteLine("==Processing: **Pipeline/Utility/Cycle naesb event not found.**");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("==Processing Error:");
                Console.WriteLine(ex);

                if (id > 0) //if the id == 0 it failed before the naesbEventProcess row was created
                {
                    string stacktrace = Stacktrace(ex);
                    Container.Resolve<INaesbEventProcessError>().Invoke(id, "OSQ", stacktrace);
                }

                CreateIncident(ex, file);
            }

            return null;
        }

        private static void SendQuickResponse(FileInfo file)
        {
            // check if the naesb event flag is on for this event/pipeline/utility/cycle
            var naesbEvent = GetQrNaesbEvent(file);
            if (naesbEvent != null && naesbEvent.On == true)
            {
                //transform file to ConfirmationResponse
                var cr = Container.Resolve<IConfirmationResponseEventGet>().Invoke(file);

                //format naseb file name
                string fileName = FormatNaesbQuickResponseFileName(cr);

                //transform ConfirmationResponse to NaesbQuickResponse
                NaesbQuickResponse naesbQuickResponse = Container.Resolve<INaesbQuickResponseGet>().Invoke(cr);

                //create naesb quick response naesb event process
                int naesbEventProcessId = Container.Resolve<INaesbQuickResponseCreate>().Invoke(DateTime.Now, fileName, cr);
                Console.WriteLine("==Processing:  3. Created quick response naesb event process: [Id: " + naesbEventProcessId + "] [Cycle: " + naesbEvent.Cycle + "] [Utility: " + naesbEvent.Utility + "] [Pipeline: " + naesbEvent.Pipeline + "]");

                //save quick response to disk
                SaveNaesbQuickResponseFile(naesbQuickResponse, fileName);
                Console.WriteLine("==Processing:  4. Created naesb quick response: [File: " + Properties.Settings.Default.NaesbOutboundUnc + fileName + "]");

                //update naesb event
                DateTime now = DateTime.Now;
                Container.Resolve<INaesbEventUpdate>().Invoke(naesbEvent.Id, now);
                Console.WriteLine("==Processing:  5. Updated the quick response naesb event process 'ProcessedTime': [" + now + "]");
            }
        }
        private static void SaveNaesbRequestForConfirmationFile(NaesbRequestForConfirmation nrfc, string ediFileName)
        {
            //serialize with out the namespace because GENTRAN can't handle it
            XmlDocument xmlFile = XmlTransformer.XmlSerialize(nrfc, true);
            xmlFile.Save(Properties.Settings.Default.NaesbOutboundUnc + ediFileName);
        }
        private static void SaveNaesbQuickResponseFile(NaesbQuickResponse qr, string fileName)
        {
            //serialize with out the namespace because GENTRAN can't handle it
            XmlDocument xmlFile = XmlTransformer.XmlSerialize(qr, true);
            xmlFile.Save(Properties.Settings.Default.NaesbOutboundUnc + fileName);
        }
        private static string FormatNaesbRequestForConfirmationFileName(RequestForConfirmation rfc)
        {
            string fileName = rfc.GasDay.ToString("yyyyMMdd") + "_" +
                                 rfc.Cycle + "_" +
                                 "RFC_" +
                                 rfc.PartyIndentificaton.PipelineEntity + "_" +
                                 rfc.PartyIndentificaton.UtilityEntity + ".xml";

            return fileName;
        }
        private static string FormatNaesbQuickResponseFileName(ConfirmationResponse cr)
        {
            string fileName = cr.GasDay.ToString("yyyyMMdd") + "_" +
                              cr.Cycle + "_" +
                              "QR_" +
                              cr.PartyIndentificaton.PipelineEntity + "_" +
                              cr.PartyIndentificaton.UtilityEntity + ".crqr";

            return fileName;
        }
        private static List<FileInfo> GetFiles(string documentElementName)
        {
            var dir = new DirectoryInfo(Properties.Settings.Default.NaesbInboundUnc);
            List<FileInfo> files = 
                dir.GetFiles()
                .Where(x => x.Extension == ".xml")
                .OrderBy(x => x.CreationTime)
                .ToList();

            if (files.Count > 0)
            {
                List<FileInfo> fis = new List<FileInfo>();
                foreach (var file in files)
                {
                    try
                    {
                        XmlDocument xml = XmlTransformer.ConvertToXmlDocument(file);
                        if (xml.DocumentElement != null && xml.DocumentElement.Name == documentElementName)
                        {
                            fis.Add(file);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("==Processing Error:");
                        Console.WriteLine(ex);

                        CreateIncident(ex, file);

                        File.Copy(file.FullName, Properties.Settings.Default.NaesbInboundUnc + "Archive\\" + file.Name, true);
                        Console.WriteLine("==Processing: [" + Properties.Settings.Default.EventType + "] [File: " + file.FullName + "] has been moved to " + Properties.Settings.Default.NaesbInboundUnc + "Archive\\" + file.Name);
                        Console.WriteLine("==Processing: [" + Properties.Settings.Default.EventType + "] <<UNSUCCESSFUL>>");
                        File.Delete(file.FullName);
                    }
                }

                return fis;
            }

            return null;
        }
        private static NaesbEvent GetOsqNaesbEvent(FileInfo file)
        {
            //get scheduled quantities from naesb file
            ScheduledQuantities osq = Container.Resolve<IScheduledQuantityEventGet>().Invoke(file);

            //get the naesb event
            var naesbEvent = Container.Resolve<INaesbEventGet>().Invoke("OSQ", osq.PartyIndentificaton.PipelineEntity, osq.PartyIndentificaton.UtilityEntity, osq.Cycle);

            return naesbEvent;
        }
        private static NaesbEvent GetCrNaesbEvent(FileInfo file)
        {
            //get confirmation response from naesb file
            ConfirmationResponse cr = Container.Resolve<IConfirmationResponseEventGet>().Invoke(file);

            //get the naesb event
            var naesbEvent = Container.Resolve<INaesbEventGet>().Invoke("CR", cr.PartyIndentificaton.PipelineEntity, cr.PartyIndentificaton.UtilityEntity, cr.Cycle);

            return naesbEvent;
        }
        private static NaesbEvent GetQrNaesbEvent(FileInfo file)
        {
            //get confirmation response from naesb file
            ConfirmationResponse cr = Container.Resolve<IConfirmationResponseEventGet>().Invoke(file);

            //get the naesb event
            var naesbEvent = Container.Resolve<INaesbEventGet>().Invoke("QR", cr.PartyIndentificaton.PipelineEntity, cr.PartyIndentificaton.UtilityEntity, cr.Cycle);

            return naesbEvent;
        }
        private static void DeleteNaesbFile(FileInfo file, int current, int total)
        {
            File.Delete(file.FullName);
            Console.WriteLine("==Processing: [" + Properties.Settings.Default.EventType + "] [" + current + " of " + total + "] [File: " + file.DirectoryName + "\\" + file.Name + "] has been removed");
        }
        private static void ArchiveNaesbFile(FileInfo file, int current, int total)
        {
            File.Copy(file.FullName, Properties.Settings.Default.NaesbInboundUnc + "Archive\\" + file.Name, true);
            Console.WriteLine("==Processing: [" + Properties.Settings.Default.EventType + "] [" + current + " of " + total + "] [File: " + file.FullName + "] has been moved to " + Properties.Settings.Default.NaesbInboundUnc + "Archive\\" + file.Name);
            //delete the file
            DeleteNaesbFile(file, current, total);
        }
        private static void CreateIncident(Exception originalException, FileInfo file)
        {
            try
            {
                Console.WriteLine("==Processing Error: [FAILURE]-[Sending invalid data to Remedy]");

                string stacktrace = Stacktrace(originalException);

                string notes = string.Empty;
                if (file != null)
                {
                    notes = "Gentran file: " + file + "\r\n\r\n" + stacktrace;
                }
                else
                {
                    notes = stacktrace;
                }

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
                            Notes = notes,
                            Summary = "Gentran/Pegasys EDI interface failed."
                        }
                    );

                var incident = hrm.Headers.Single(m => m.Key == "X-Id");
                string incidentNumber = incident.Value.ToList()[0];

                Console.WriteLine("==Processing Error: [SUCCESS]-[" + incidentNumber + "" + " created in Remedy]");
            }
            catch (HttpResponseException exception)
            {
                Console.WriteLine("==Processing Error: [FAILURE]-[Failed to send invalid data to Remedy]");
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
                Console.WriteLine("==Processing Error: [FAILURE]-[Failed to send invalid data to Remedy]");
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