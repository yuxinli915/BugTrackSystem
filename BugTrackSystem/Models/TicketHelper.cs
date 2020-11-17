using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace BugTrackSystem.Models
{
    public  static class TicketHelper
    {
       static  ApplicationDbContext db = new ApplicationDbContext();

        public static bool DeleteCommentFromTcket(int id)
        {
            var comment=db.Comments.Where(i => i.Id == id).FirstOrDefault();
            if (comment!=null)
            {
                db.Comments.Remove(comment);
                db.SaveChanges();
                return true;
            }
            return false;
        }
        public static bool AddAttchmentToTcket(int id,TicketAttachment attachment)
        {       
            if (attachment != null)
            {
                attachment.TicketId = id;
                db.Attachments.Add(attachment);
                db.SaveChanges();
                return true;
            }
            return false;
        }
        public static bool DeleteAttchmentFormTcket(int id, int ticketId)
        {
            var attchment = db.Attachments.Where(j => j.Id == id && j.TicketId == ticketId).FirstOrDefault();
            if (attchment != null)
            {
                db.Attachments.Remove(attchment);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public static IEnumerable<Ticket> GetAllTicketsForUser()
        {
            return db.Tickets.ToList();
        }
        public static IEnumerable<Ticket> GetAllTicketsForProject(int projectId)
        {
            return db.Tickets.Where(i=>i.ProjectId==projectId).ToList();
        }

        public static IEnumerable<Ticket> FilterTickets(TicketFilter filters)
        {
            var tickts = db.Tickets.AsQueryable();
            if (!string.IsNullOrEmpty(filters.Status))
            {
                tickts = tickts.Where(i => i.TicketStatus.Name == filters.Status).AsQueryable();
            }
            if (!string.IsNullOrEmpty(filters.Priority))
            {
                tickts = tickts.Where(i => i.TicketProperty.Name == filters.Priority).AsQueryable();
            }
            if (!string.IsNullOrEmpty(filters.Project))
            {
                tickts = tickts.Where(i => i.Project.Title == filters.Project).AsQueryable();
            }
            if (!string.IsNullOrEmpty(filters.Developer))
            {
                tickts = tickts.Where(i => i.AssignedUser.UserName == filters.Developer).AsQueryable();
            }
            if (!string.IsNullOrEmpty(filters.Title))
            {
                tickts = tickts.Where(i => i.Title.Contains(filters.Title)).AsQueryable();
            }
            if (!string.IsNullOrEmpty(filters.Submitter))
            {
                tickts = tickts.Where(i => i.AssignedUser.UserName == filters.Submitter).AsQueryable();
            }
            if (filters.ByCreatedDate!=null)
            {
                tickts = tickts.Where(i => i.Created == filters.ByCreatedDate).AsQueryable();
            }
            if (filters.ByUpdatedDate != null)
            {
                tickts = tickts.Where(i => i.Updated == filters.ByUpdatedDate).AsQueryable();
            }
            return  tickts.ToList();
        }

        public static IEnumerable<Ticket> SearchTicketsByKeyword(TicketFilter filters)
        {
            var tickts = db.Tickets.AsQueryable();
            if (!string.IsNullOrEmpty(filters.Status))
            {
                tickts = tickts.Where(i => i.TicketStatus.Name == filters.Status).AsQueryable();
            }
            if (!string.IsNullOrEmpty(filters.Priority))
            {
                tickts = tickts.Where(i => i.TicketProperty.Name == filters.Priority).AsQueryable();
            }
            if (!string.IsNullOrEmpty(filters.Project))
            {
                tickts = tickts.Where(i => i.Project.Title == filters.Project).AsQueryable();
            }
            if (!string.IsNullOrEmpty(filters.Developer))
            {
                tickts = tickts.Where(i => i.AssignedUser.UserName == filters.Developer).AsQueryable();
            }
            if (!string.IsNullOrEmpty(filters.Title))
            {
                tickts = tickts.Where(i => i.Title.Contains(filters.Title)).AsQueryable();
            }
            if (!string.IsNullOrEmpty(filters.Submitter))
            {
                tickts = tickts.Where(i => i.AssignedUser.UserName == filters.Submitter).AsQueryable();
            }
            if (filters.ByCreatedDate != null)
            {
                tickts = tickts.Where(i => i.Created == filters.ByCreatedDate).AsQueryable();
            }
            if (filters.ByUpdatedDate != null)
            {
                tickts = tickts.Where(i => i.Updated == filters.ByUpdatedDate).AsQueryable();
            }
            return tickts.ToList();
        }

        public static void CreateTicket(ApplicationDbContext database, Ticket ticket)
        {
            database.Tickets.Add(ticket);
            database.SaveChanges();
        }

        public static void EditTicketDetail(ApplicationDbContext database, Ticket EditedTicket, string userId)
        {
            var ticket = database.Tickets.Find(EditedTicket.Id);
            ticket.Updated = DateTime.Now;
            PropertyInfo[] fi = ticket.GetType().GetProperties();
            foreach (var field in fi)
            {
                if (field.GetValue(ticket).Equals(field.GetValue(EditedTicket)) &&
                    (field.Name == "Title" || field.Name == "Description" || field.Name == "TicketStatus" || field.Name == "TicketType" || field.Name == "TicketProperty"))
                {
                    database.Histories.Add(
                        new TicketHistory
                        {
                            TicketId = ticket.Id,
                            UserId = userId,
                            Date = DateTime.Now,
                            Property = field.Name,
                            OldValue = (string)field.GetValue(ticket, null),
                            NewValue = (string)field.GetValue(EditedTicket, null)
                        });

                    field.SetValue(ticket, field.GetValue(EditedTicket));
                }
            }
            database.SaveChanges();
        }

        public static void AssignUserToTicket(ApplicationDbContext database, int ticketId, string userId)
        {
            database.Tickets.Find(ticketId).AssignedUserId = userId;
            database.SaveChanges();
        }

        public static void RemoveUserToTicket(ApplicationDbContext database, int ticketId)
        {
            database.Tickets.Find(ticketId).AssignedUserId = null;
            database.Tickets.Find(ticketId).AssignedUser = null;
            database.SaveChanges();
        }

        public static void AddCommentToTicket(ApplicationDbContext database, TicketComment comment)
        {
            database.Comments.Add(comment);
            database.SaveChanges();
        }
    }
}