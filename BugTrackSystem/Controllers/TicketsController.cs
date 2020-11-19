using BugTrackSystem.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTrackSystem.Controllers
{
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "Submitter")]
        public ActionResult CreateTicketPage()
        {
            return View();
        }

        [Authorize(Roles = "Submitter")]
        [HttpPost]
        public ActionResult CreateTicketPage(Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                TicketHelper.CreateTicket(db, ticket);
                return RedirectToAction("Index", "Manage");
            }
            return View(ticket);
        }

        [Authorize]
        public ActionResult EditTicketDetail(int id)
        {
            var ticket = db.Tickets.Find(id);
            return View(ticket);
        }

        [Authorize]
        [HttpPost]
        public ActionResult EditTicketDetail(Ticket editedTicket)
        {
            if (ModelState.IsValid)
            {
                TicketHelper.EditTicketDetail(db, editedTicket, User.Identity.GetUserId());
                return RedirectToAction("Index", "Manage");
            }
            return View(editedTicket);
        }

        [Authorize(Roles = "Admin, Manager")]
        public ActionResult AssignUserToTicket(int id)
        {
            var ticket = db.Tickets.Find(id);
            ViewBag.SubmitterId = new SelectList(db.Users.Where(u => u.Roles.Any(r => r.RoleId == "3")), "Id", "Name"); // Need to get developer role id.
            return View();
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpPost]
        public ActionResult AssignUserToTicket(int id, string userId)
        {
            if (ModelState.IsValid)
            {
                TicketHelper.AssignUserToTicket(db, id, userId);
                return RedirectToAction("Detail", new { id });
            }
            ViewBag.userId = new SelectList(db.Users.Where(u => u.Roles.Any(r => r.RoleId == "3")), "Id", "Name");
            return View();
        }

        [Authorize(Roles = "Admin, Manager")]
        public ActionResult RemoveUserFromTicket(int id)
        {
            TicketHelper.RemoveUserToTicket(db, id);
            return RedirectToAction("Detail", new { id });
        }

        [Authorize]
        public ActionResult AddCommentToTicket()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddCommentToTicket(TicketComment comment)
        {
            if (ModelState.IsValid)
            {
                comment.UserId = User.Identity.GetUserId();
                TicketHelper.AddCommentToTicket(db, comment);
                return RedirectToAction("Detail", new { id = comment.TicketId});
            }
            return View(comment);
        }

        [Authorize]
        public ActionResult AddAttchmentToTicket()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddAttchmentToTicket(TicketAttachment attchment)
        {
            if (ModelState.IsValid)
            {
                attchment.UserId = User.Identity.GetUserId();
                TicketHelper.AddAttchmentToTcket(db, attchment);
                return RedirectToAction("Detail", new { id = attchment.TicketId});
            }
            return View(attchment);
        }

        [Authorize]
        public ActionResult DeleteCommentFromTcket(int id)
        {
            var ticketId = db.Comments.Find(id).TicketId;
            TicketHelper.DeleteCommentFromTcket(id);
            return RedirectToAction("Detail", new { id = ticketId});
        }


        [Authorize]
        public ActionResult DeleteAttchmentFormTcket(int id)
        {
            var ticketId = db.Attachments.Find(id).TicketId;
            TicketHelper.DeleteAttchmentFormTcket(id);
            return RedirectToAction("Detail", new { id = ticketId});
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
            var ticket = db.Tickets.Find(id);
            ViewBag.RoleId = db.Users.Find(User.Identity.GetUserId()).Roles.First().RoleId;
            ViewBag.UserId = User.Identity.GetUserId();
            return View(ticket);
        }
    }
}