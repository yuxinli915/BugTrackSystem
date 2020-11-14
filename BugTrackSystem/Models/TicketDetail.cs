using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTrackSystem.Models
{
    public class TicketDetail
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime Date { get; set; }
    }

    public class TicketHistory : TicketDetail
    {
        public string Property { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }

    public class TicketComment : TicketDetail
    {
        public string Body { get; set; }
    }

    public class TicketAttachment : TicketDetail
    {
        public string FilePath { get; set; }
        public string Description { get; set; }
        public string FileUrl { get; set; }
    }

    public class TicketNotificaiton : TicketComment
    {

    }
}