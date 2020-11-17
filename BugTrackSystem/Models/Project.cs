using System.Collections.Generic;

namespace BugTrackSystem.Models
{
    public class Project : SystemItem
    {
        public bool IsArchived { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }
         
        public Project()
        {
            Tickets = new HashSet<Ticket>();
            ApplicationUsers = new HashSet<ApplicationUser>();
        }
    }
}