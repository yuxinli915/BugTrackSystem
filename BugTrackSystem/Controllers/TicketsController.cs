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
            ViewBag.TicketStatusId = new SelectList(db.Statuses, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.Types, "Id", "Name");
            ViewBag.TicketPropertyId = new SelectList(db.Properties, "Id", "Name");

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
            ViewBag.TicketStatusId = new SelectList(db.Statuses, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.Types, "Id", "Name");
            ViewBag.TicketPropertyId = new SelectList(db.Properties, "Id", "Name");
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
                return RedirectToAction("Index", "Manage");
            }
            ViewBag.userId = new SelectList(db.Users.Where(u => u.Roles.Any(r => r.RoleId == "3")), "Id", "Name");
            return View();
        }

        [Authorize(Roles = "Admin, Manager")]
        public ActionResult RemoveUserFromTicket()
        {
            return View();
        }

        [Authorize(Roles = "Admin, Manager")]
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
                if (User.IsInRole("Admin"))
                {
                    TicketHelper.AddCommentToTicket(db, comment);
                }
                else if (User.IsInRole("Manager"))
                {
                    var projects = ProjectHelper.GetAllProjectsForUser(User.Identity.GetUserId());
                    var task = projects.Where(i => i.Tickets.Where(j => j.Id == comment.TicketId).Any()).FirstOrDefault();
                    if (task != null)
                    {
                        TicketHelper.AddCommentToTicket(db, comment);
                    }
                }
                else if (User.IsInRole("Developer"))
                {
                    var task = TicketHelper.GetAllTicketsForUser(User.Identity.GetUserId());
                    if (task != null)
                    {
                        var ticket = task.Where(i => i.Id == comment.TicketId).FirstOrDefault();
                        if (ticket != null)
                        {
                            TicketHelper.AddCommentToTicket(db, comment);
                        }

                    }
                }
                else if (User.IsInRole("Submitter"))
                {
                    var task = TicketHelper.GetAllTicketsForSumitter(User.Identity.GetUserId());
                    if (task != null)
                    {
                        var ticket = task.Where(i => i.Id == comment.TicketId).FirstOrDefault();
                        if (ticket != null)
                        {
                            TicketHelper.AddCommentToTicket(db, comment);
                        }

                    }
                }


                return RedirectToAction("Index", "Manage");
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
                if (User.IsInRole("Admin"))
                {
                    var ticket = db.Tickets.Where(i => i.Id == attchment.TicketId).FirstOrDefault();
                    TicketHelper.AddAttchmentToTcket(ticket.Id, attchment);
                }
                else if (User.IsInRole("Manager"))
                {
                    var projects = ProjectHelper.GetAllProjectsForUser(User.Identity.GetUserId());
                    var task = projects.Where(i => i.Tickets.Where(j => j.Id == attchment.TicketId).Any()).FirstOrDefault();
                    if (task != null)
                    {
                        var ticket = db.Tickets.Where(i => i.Id == attchment.TicketId).FirstOrDefault();
                        TicketHelper.AddAttchmentToTcket(ticket.Id, attchment);
                    }
                }
                else if (User.IsInRole("Developer"))
                {
                    var task = TicketHelper.GetAllTicketsForUser(User.Identity.GetUserId());
                    if (task != null)
                    {
                        var ticket = task.Where(i => i.Id == attchment.TicketId).FirstOrDefault();
                        if (ticket != null)
                        {
                            TicketHelper.AddAttchmentToTcket(ticket.Id, attchment);
                        }

                    }
                }
                else if (User.IsInRole("Submitter"))
                {
                    var task = TicketHelper.GetAllTicketsForSumitter(User.Identity.GetUserId());
                    if (task != null)
                    {
                        var ticket = task.Where(i => i.Id == attchment.TicketId).FirstOrDefault();
                        if (ticket != null)
                        {
                            TicketHelper.AddAttchmentToTcket(ticket.Id, attchment);
                        }

                    }
                }
                return RedirectToAction("Index", "Manage");
            }
            return View(attchment);
        }


        [Authorize]
        public ActionResult DeleteCommentFromTcket()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult DeleteCommentFromTcket(TicketComment comment)
        {
            if (ModelState.IsValid)
            {
                if (User.IsInRole("Admin"))
                {
                    TicketHelper.DeleteCommentFromTcket(comment.Id);
                }
                else if (User.IsInRole("Manager"))
                {
                    var projects = ProjectHelper.GetAllProjectsForUser(User.Identity.GetUserId());
                    var task = projects.Where(i => i.Tickets.Where(j => j.Id == comment.TicketId).Any()).FirstOrDefault();
                    if (task != null)
                    {
                        var ticket = db.Tickets.Where(i => i.Id == comment.TicketId).FirstOrDefault();
                        TicketHelper.DeleteCommentFromTcket(ticket.Id);
                    }
                }
                else if (User.IsInRole("Developer"))
                {
                    var task = db.Comments.Where(i => i.Id == comment.Id && i.UserId == User.Identity.GetUserId()).FirstOrDefault();
                    if (task != null)
                    {
                        TicketHelper.DeleteCommentFromTcket(task.Id);
                    }
                }
                else if (User.IsInRole("Submitter"))
                {
                    var task = TicketHelper.GetAllTicketsForSumitter(User.Identity.GetUserId());
                    if (task != null)
                    {
                        var ticket = task.Where(i => i.Id == comment.TicketId).FirstOrDefault();
                        if (ticket != null)
                        {
                            TicketHelper.DeleteCommentFromTcket(ticket.Id);
                        }

                    }
                }
                return RedirectToAction("Index", "Manage");
            }
            return View(comment);
        }
        [Authorize]
        public ActionResult DeleteAttchmentFormTcket()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult DeleteAttchmentFormTcket(TicketAttachment attchment)
        {
            if (ModelState.IsValid)
            {
                if (User.IsInRole("Admin"))
                {
                    TicketHelper.DeleteAttchmentFormTcket(attchment.Id, attchment.TicketId);
                }
                else if (User.IsInRole("Manager"))
                {
                    var projects = ProjectHelper.GetAllProjectsForUser(User.Identity.GetUserId());
                    var attch = projects.Where(i => i.Tickets.Where(j => j.Id == attchment.TicketId).Any()).FirstOrDefault();
                    if (attch != null)
                    {
                        TicketHelper.DeleteAttchmentFormTcket(attch.Id, attchment.TicketId);
                    }
                }
                else if (User.IsInRole("Developer"))
                {
                    var attch = db.Attachments.Where(i => i.Id == attchment.Id && i.UserId == User.Identity.GetUserId()).FirstOrDefault();
                    if (attch != null)
                    {
                        TicketHelper.DeleteAttchmentFormTcket(attch.Id, attchment.TicketId);
                    }
                }
                else if (User.IsInRole("Submitter"))
                {
                    var task = TicketHelper.GetAllTicketsForSumitter(User.Identity.GetUserId());
                    if (task != null)
                    {
                        var attch = task.Where(i => i.Id == attchment.TicketId && i.Attachments.Where(j => j.Id == attchment.Id).Any()).FirstOrDefault();
                        if (attch != null)
                        {
                            TicketHelper.DeleteAttchmentFormTcket(attch.Id, attchment.TicketId);
                        }
                    }
                }

                TicketHelper.DeleteAttchmentFormTcket(attchment.Id, attchment.TicketId);
                return RedirectToAction("Index", "Manage");
            }
            return View(attchment);
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
    }
}