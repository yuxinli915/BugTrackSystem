using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTrackSystem.Models
{
    public class TicketComment : TicketDetail
    {
        public string Body { get; set; }
    }
}