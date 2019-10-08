using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nomination.BusinessLayer.Interfaces.Naesb;
using Nomination.Domain.Naesb;
using Nomination.Persistence.Common;
using Nomination.Persistence.Shared;

namespace Nomination.Persistence.Naesb
{
    public class NaesbEventNotificationRepository : Repository<tb_naesb_event_notifications>, INaesbEventNotificationRepository
    {
        public NaesbEventNotification Get(int id)
        {
            throw new NotImplementedException();
        }
        public List<NaesbEventNotification> GetEventNotificationsNotDelivered()
        {
            var objs = base.GetAll()
                .Where(p =>
                    p.ProcessedTime == null
                )
                .Select(new ModelFactory().Map)
                .ToList();

            return objs;
        }
        public int Create(NaesbEventNotification obj)
        {
            //map domain to enitity framework
            tb_naesb_event_notifications entity = new ModelFactory().Map(obj);

            //insert to database
            base.Add(entity);

            return Convert.ToInt32(entity.Id);
        }
        public void Update(int id, params object[] list)
        {
            var entity = base.Get(id);

            foreach (object kvp in list)
            {
                if (kvp is KeyValuePair<string, string> && ((KeyValuePair<string, string>)kvp).Key == "Source")
                {
                    entity.Source = ((KeyValuePair<string, string>)kvp).Value;
                }
                if (kvp is KeyValuePair<string, string> && ((KeyValuePair<string, string>)kvp).Key == "To")
                {
                    entity.To = ((KeyValuePair<string, string>)kvp).Value;
                }
                if (kvp is KeyValuePair<string, string> && ((KeyValuePair<string, string>)kvp).Key == "From")
                {
                    entity.From = ((KeyValuePair<string, string>)kvp).Value;
                }
                if (kvp is KeyValuePair<string, string> && ((KeyValuePair<string, string>)kvp).Key == "Cc")
                {
                    entity.CC = ((KeyValuePair<string, string>)kvp).Value;
                }
                if (kvp is KeyValuePair<string, string> && ((KeyValuePair<string, string>)kvp).Key == "Priority")
                {
                    entity.Priority = ((KeyValuePair<string, string>)kvp).Value;
                }
                if (kvp is KeyValuePair<string, string> && ((KeyValuePair<string, string>)kvp).Key == "Subject")
                {
                    entity.Subject = ((KeyValuePair<string, string>)kvp).Value;
                }
                if (kvp is KeyValuePair<string, string> && ((KeyValuePair<string, string>)kvp).Key == "Body")
                {
                    entity.Body = ((KeyValuePair<string, string>)kvp).Value;
                }
                if (kvp is KeyValuePair<string, bool> && ((KeyValuePair<string, bool>)kvp).Key == "Html")
                {
                    entity.Html = ((KeyValuePair<string, bool>)kvp).Value;
                }
                if (kvp is KeyValuePair<string, DateTime?> && ((KeyValuePair<string, DateTime?>)kvp).Key == "ProcessedTime")
                {
                    entity.ProcessedTime = ((KeyValuePair<string, DateTime?>)kvp).Value;
                }
                if (kvp is KeyValuePair<string, DateTime?> && ((KeyValuePair<string, DateTime?>)kvp).Key == "GasDay")
                {
                    entity.GasDay = ((KeyValuePair<string, DateTime?>)kvp).Value;
                }
                if (kvp is KeyValuePair<string, string> && ((KeyValuePair<string, string>)kvp).Key == "Pipeline")
                {
                    entity.PipelineCd = ((KeyValuePair<string, string>)kvp).Value;
                }
                if (kvp is KeyValuePair<string, string> && ((KeyValuePair<string, string>)kvp).Key == "Utility")
                {
                    entity.CompanyCd = ((KeyValuePair<string, string>)kvp).Value;
                }
                if (kvp is KeyValuePair<string, string> && ((KeyValuePair<string, string>)kvp).Key == "Cycle")
                {
                    entity.CycleCd = ((KeyValuePair<string, string>)kvp).Value;
                }
                if (kvp is KeyValuePair<string, string> && ((KeyValuePair<string, string>)kvp).Key == "Ref1")
                {
                    entity.Ref1 = ((KeyValuePair<string, string>)kvp).Value;
                }
                if (kvp is KeyValuePair<string, string> && ((KeyValuePair<string, string>)kvp).Key == "Ref2")
                {
                    entity.Ref2 = ((KeyValuePair<string, string>)kvp).Value;
                }
                if (kvp is KeyValuePair<string, string> && ((KeyValuePair<string, string>)kvp).Key == "Ref3")
                {
                    entity.Ref3 = ((KeyValuePair<string, string>)kvp).Value;
                }
            }

            base.Change(entity);
        }
    }
}
