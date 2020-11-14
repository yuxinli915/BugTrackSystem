using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTrackSystem.Models
{
    public class TicketAttachment : TicketDetail
    {
        public string FilePath { get; set; }
        public string Description { get; set; }
        public string FileUrl { get; set; }
    }
}