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
            ViewBag.SubmitterId = new SelectList(db.Users.Where(u => u.Roles.Any(r => r.RoleId == "1")), "Id", "Name"); // Need to get developer role id.
            return View();
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpPost]
        public ActionResult AssignUserToTicket(int id, string userId)
        {
            if (ModelState.IsValid)
            {
                TicketHelper.AssignUserToTicket(db, id, userId);
                return RedirectToAction("Index", "Manage");
            }
            ViewBag.userId = new SelectList(db.Users.Where(u => u.Roles.Any(r => r.RoleId == "1")), "Id", "Name");
            return View();
        }

        [Authorize(Roles = "Admin, Manager")]
        public ActionResult RemoveUserFromTicket()
        {
            return View();
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpPost]
        public ActionResult RemoveUserFromTicket(int id)
        {
            if (ModelState.IsValid)
            {
                TicketHelper.RemoveUserToTicket(db, id);
                return RedirectToAction("Index", "Manage");
            }
            return View();
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
                TicketHelper.AddCommentToTicket(db, comment);
                return RedirectToAction("Index", "Manage");
            }
            return View(comment);
        }
    }
}