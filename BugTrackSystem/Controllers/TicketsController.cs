﻿using BugTrackSystem.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Net;
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
                return RedirectToAction("Index", "Manage");
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
            Ticket ticket = db.Tickets.Include(t => t.Owner).Include(t => t.Attachments).Include(t => t.Comments).Include(t => t.TicketStatus).Include(t => t.AssignedUser).Include(t => t.TicketProperty).Include(t => t.TicketType).First(t => t.Id == id);
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
    }
}