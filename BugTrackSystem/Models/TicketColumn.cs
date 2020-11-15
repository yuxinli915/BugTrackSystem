using System.Collections.Generic;

namespace BugTrackSystem.Models
{
    public class TicketColumn
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
        public TicketColumn()
        {
            Tickets = new HashSet<Ticket>();
        }

        public class TicketStatus : TicketColumn
        {
        }
        public class TicketType : TicketColumn
        {
        }
        public class TicketProperty : TicketColumn
        {
        }
    }
}