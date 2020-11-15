using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;

namespace BugTrackSystem.Models
{
    public static class TicketHelper
    {
        public static void CreateTicket(ApplicationDbContext db, Ticket ticket)
        {
            db.Tickets.Add(ticket);
        }

        public static void EditTicketDetail(ApplicationDbContext db, Ticket EditedTicket, string userId)
        {
            var ticket = db.Tickets.Find(EditedTicket.Id);
            ticket.Updated = DateTime.Now;
            PropertyInfo[] fi = ticket.GetType().GetProperties();
            foreach(var field in fi)
            {
                if(field.GetValue(ticket).Equals(field.GetValue(EditedTicket)) &&
                    (field.Name == "Title" || field.Name == "Description" || field.Name == "TicketStatus" || field.Name == "TicketType" || field.Name == "TicketProperty"))
                {
                    db.Histories.Add(
                        new TicketHistory { 
                            TicketId = ticket.Id, 
                            UserId = userId, 
                            Date = DateTime.Now, 
                            Property = field.Name, 
                            OldValue = (string)field.GetValue(ticket, null),
                            NewValue = (string)field.GetValue(EditedTicket, null) });

                    field.SetValue(ticket, field.GetValue(EditedTicket));
                }
            }
            db.SaveChanges();
        }

        public static void AssignUserToTicket(ApplicationDbContext db, int ticketId, string userId)
        {
            db.Tickets.Find(ticketId).AssignedUserId = userId;
            db.SaveChanges();
        }

        public static void RemoveUserToTicket(ApplicationDbContext db, int ticketId)
        {
            db.Tickets.Find(ticketId).AssignedUserId = null;
            db.Tickets.Find(ticketId).AssignedUser = null;
            db.SaveChanges();
        }
        
        public static void AddCommentToTicket(ApplicationDbContext db, TicketComment comment)
        {
            db.Comments.Add(comment);
            db.SaveChanges();
        }
    }
}