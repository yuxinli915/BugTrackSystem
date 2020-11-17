using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTrackSystem.Models
{
    public class TicketFilter
    {
        public string Status { get; set; }
        public string Priority { get; set; }
        public string Type { get; set; }
        public string Project { get; set; }
        public DateTime? ByCreatedDate { get; set; }
        public DateTime? ByUpdatedDate { get; set; }
        public string Title { get; set; }
        public string Submitter { get; set; }
        public string Developer { get; set; }
    }
}