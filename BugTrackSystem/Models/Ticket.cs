using System;
using System.Collections;
using System.Collections.Generic;

namespace BugTrackSystem.Models
{
    public class Ticket: SystemItem
    {
        public string AssignedUserId { get; set; }
        public ApplicationUser AssignedUser { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public int TicketStatusId { get; set; }
        public TicketStatus TicketStatus { get; set; }
        public int TicketTypeId { get; set; }
        public TicketType TicketType { get; set; }
        public int TicketPropertyId { get; set; }
        public TicketProperty TicketProperty { get; set; }
        public virtual ICollection<TicketAttachment> Attachments { get; set; }
        public virtual ICollection<TicketComment> Comments { get; set; }
        public virtual ICollection<TicketHistory> Histories { get; set; }
        public Ticket()
        {
            Attachments = new HashSet<TicketAttachment>();
            Comments = new HashSet<TicketComment>();
            Histories = new HashSet<TicketHistory>();
        }

    }
}