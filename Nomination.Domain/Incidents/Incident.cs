using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nomination.Domain.Incidents
{
    public class Incident
    {
        public string Id { get; set; }
        public string AssignedSupportGroup { get; set; }
        public string AssignedSupportOrganization { get; set; }
        public string Assignee { get; set; }
        public string ClientService { get; set; }
        public string Company { get; set; }
        public string Department { get; set; }
        public string Impact { get; set; }
        public string Notes { get; set; }
        public string Priority { get; set; }
        public string ReportedSource { get; set; }
        public string Resolution { get; set; }
        public string ServiceType { get; set; }
        public string Status { get; set; }
        public string Summary { get; set; }
        public string Urgency { get; set; }
    }
}
