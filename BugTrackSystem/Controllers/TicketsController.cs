using BugTrackSystem.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
namespace BugTrackSystem.Controllers
{
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "Submitter")]
        public ActionResult CreateTicketPage()
        {
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Title");
            ViewBag.TypeId = new SelectList(db.Types, "Id", "Name");
            ViewBag.PropertyId = new SelectList(db.Properties, "Id", "Name");
            return View();
        }

        [Authorize(Roles = "Submitter")]
        [HttpPost]
        public ActionResult CreateTicketPage(Ticket ticket, int ProjectId, int TypeId, int PropertyId)
        {
            if (ModelState.IsValid)
            {
                ticket.OwnerId = User.Identity.GetUserId();
                ticket.ProjectId = ProjectId;
                ticket.TicketTypeId = TypeId;
                ticket.TicketStatus = db.Statuses.First(s => s.Name == "Unassigned");
                ticket.TicketPropertyId = PropertyId;
                ticket.Created = DateTime.Now;
                TicketHelper.CreateTicket(db, ticket);
                ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Title");
                ViewBag.TypeId = new SelectList(db.Types, "Id", "Name");
                ViewBag.PropertyId = new SelectList(db.Properties, "Id", "Name");
                return RedirectToAction("Index", "Manage");
            }
            return View(ticket);
        }

        [Authorize]
        public ActionResult EditTicketDetail(int id)
        {
            ViewBag.TypeId = new SelectList(db.Types, "Id", "Name");
            ViewBag.PropertyId = new SelectList(db.Properties, "Id", "Name");
            var ticket = db.Tickets.Find(id);
            return View(ticket);
        }

        [Authorize]
        [HttpPost]
        public ActionResult EditTicketDetail(Ticket editedTicket, int TypeId, int PropertyId)
        {
            if (ModelState.IsValid)
            {
                editedTicket.TicketTypeId = TypeId;
                editedTicket.TicketPropertyId = PropertyId;
                TicketHelper.EditTicketDetail(db, editedTicket, User.Identity.GetUserId());
                return RedirectToAction("Detail", "Tickets", new { id = editedTicket.Id});
            }
            ViewBag.TypeId = new SelectList(db.Types, "Id", "Name");
            ViewBag.PropertyId = new SelectList(db.Properties, "Id", "Name");
            return View(editedTicket);
        }

        [Authorize(Roles = "Admin, Manager")]
        public ActionResult AssignUserToTicket(int id)
        {
            var ticket = db.Tickets.Find(id);

            ViewBag.SubmitterId = new SelectList(UserHelper.AllUsersInRole("Developer"), "Id", "Email");
            return View();
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpPost]
        public ActionResult AssignUserToTicket(int id, string SubmitterId)
        {
            if (ModelState.IsValid)
            {
                TicketHelper.AssignUserToTicket(db, id, SubmitterId);
                return RedirectToAction("Detail", new { id });
            }
            ViewBag.SubmitterId = new SelectList(UserHelper.AllUsersInRole("Developer"), "Id", "Email");
            return View();
        }

        [Authorize(Roles = "Admin, Manager")]
        public ActionResult RemoveUserFromTicket(int id)
        {
            TicketHelper.RemoveUserToTicket(db, id);
            return RedirectToAction("Detail", new { id });
        }

        [Authorize]
        public ActionResult AddCommentToTicket(int id)
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddCommentToTicket(int id, TicketComment comment)
        {
            if (ModelState.IsValid)
            {
                comment.TicketId = id;
                comment.Date = DateTime.Now;
                comment.UserId = User.Identity.GetUserId();
                TicketHelper.AddCommentToTicket(db, comment);
                return RedirectToAction("Detail", new { id });
            }
            return View(comment);
        }

        [Authorize]
        public ActionResult EditComment(int id)
        {
            var comment = db.Comments.Find(id);
            return View("AddCommentToTicket", comment);
        }

        [Authorize]
        [HttpPost]
        public ActionResult EditComment(int id, TicketComment comment)
        {
            if (ModelState.IsValid)
            {
                var oldC = db.Comments.Find(id);
                comment.TicketId = oldC.TicketId;
                comment.Date = DateTime.Now;
                comment.UserId = User.Identity.GetUserId();
                TicketHelper.DeleteCommentFromTcket(id);
                TicketHelper.AddCommentToTicket(db, comment);
                return RedirectToAction("Detail", new { id = comment.TicketId });
            }
            return View("AddCommentToTicket", comment);
        }

        [Authorize]
        public ActionResult AddAttchmentToTicket(int id)
        {
           
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddAttchmentToTicket(int id, TicketAttachment attchment)
        {
            if (ModelState.IsValid)
            {
                attchment.TicketId = id;
                attchment.Date = DateTime.Now;
                attchment.UserId = User.Identity.GetUserId();
                TicketHelper.AddAttchmentToTcket(db, attchment);
                return RedirectToAction("Detail", "Tickets", new { id });
            }
            return View(attchment);
        }

        [Authorize]
        public ActionResult EditAttchment(int id)
        {
            var attachment = db.Attachments.Find(id);
            return View("AddAttchmentToTicket", attachment);
        }

        [Authorize]
        [HttpPost]
        public ActionResult EditAttchment(int id, TicketAttachment attchment)
        {
            
            if (ModelState.IsValid)
            {
                var oldA = db.Attachments.Find(id);
                attchment.TicketId = oldA.TicketId;
                attchment.Date = DateTime.Now;
                attchment.UserId = User.Identity.GetUserId();
                TicketHelper.DeleteAttchmentFormTcket(id);
                TicketHelper.AddAttchmentToTcket(db, attchment);
                return RedirectToAction("Detail", "Tickets", new { id  = attchment.TicketId });
            }
            return View("AddAttchmentToTicket", attchment);
        }

        [Authorize]
        public ActionResult DeleteCommentFromTcket(int id)
        {
            var ticketId = db.Comments.Find(id).TicketId;
            TicketHelper.DeleteCommentFromTcket(id);
            return RedirectToAction("Detail", new { id = ticketId });
        }

        [Authorize]
        public ActionResult DeleteAttchmentFormTcket(int id)
        {
            var ticketId = db.Attachments.Find(id).TicketId;
            TicketHelper.DeleteAttchmentFormTcket(id);
            return RedirectToAction("Detail", new { id = ticketId });
        }

        [Authorize]
        public ActionResult GetAllTicketsForUser()
        {
            var tickties = TicketHelper.GetAllTicketsForUser(User.Identity.GetUserId());
            return View(tickties);
        }

        [Authorize]
        public ActionResult GetAllTicketsForProject(Project project)
        {
            var projects = TicketHelper.GetAllTicketsForProject(project.Id);
            return View(projects);
        }

        [Authorize]
        public ActionResult FilterTickets(TicketFilter filters)
        {
            var tickties = TicketHelper.FilterTickets(filters);
            return View(tickties);
        }

        [Authorize]
        public ActionResult Detail(int id)
        {
            Ticket ticket = db.Tickets.Include(t => t.Owner).Include(t => t.Attachments).Include(t => t.Comments).Include(t => t.TicketStatus).Include(t => t.AssignedUser).Include(t => t.TicketProperty).Include(t => t.TicketType).Include("Attachments.User").Include("Comments.User").First(t => t.Id == id);
            
            if (User.IsInRole("Admin"))
            {
                ViewBag.Identity = "Admin";
            }
            if (User.IsInRole("Manager"))
            {
                ViewBag.Identity = "Manager";
            }
            if (User.IsInRole("Developer"))
            {
                ViewBag.Identity = "Developer";
            }
            if (User.IsInRole("Submitter"))
            {
                ViewBag.Identity = "Submitter";
            }
            ViewBag.UserId = User.Identity.GetUserId();
            return View(ticket);
        }

        public ActionResult Search(string option, string search, int? page)
        {
            int pageSize = 3;
            int pageNumber = (page ?? 1);

            ViewBag.option = option;
            ViewBag.search = search;

            if (option == "Title")
            {
                var list = db.Tickets.Where(t => t.Title.Contains(search) || search == null).ToList();
                return View(list.ToPagedList(pageNumber, pageSize));
            }
            else if (option == "Description")
            {
               var list=db.Tickets.Where(t => t.Description.Contains(search) || search == null).ToList();
                return View(list.ToPagedList(pageNumber, pageSize));
            }
            else if (option == "Status")
            {
                var list = db.Tickets.Where(t => t.TicketStatus.Name.Contains(search) || search == null).ToList();
                return View(list.ToPagedList(pageNumber, pageSize));
            }
            else if (option == "Priority")
            {
                var list = db.Tickets.Where(t => t.TicketProperty.Name.Contains(search) || search == null).ToList();
                return View(list.ToPagedList(pageNumber, pageSize));
            }
            else if (option == "Type")
            {
                var list = db.Tickets.Where(t => t.TicketType.Name.Contains(search) || search == null).ToList();
                return View(list.ToPagedList(pageNumber, pageSize));
            }
            else if (option == "User")
            {
                var list = db.Tickets.Where(t => t.Owner.Email.Contains(search) || t.AssignedUser.Email.Contains(search) || search == null).ToList();
                return View(list.ToPagedList(pageNumber, pageSize));
            }
            else if (option == "Project")
            {
                var list = db.Tickets.Where(t => t.Project.Title.Contains(search) || t.Project.Description.Contains(search) || search == null).ToList();
                return View(list.ToPagedList(pageNumber, pageSize));
            }
            else 
            return View(new List<Ticket>().ToPagedList(pageNumber, pageSize));
        }
    }
}