using BugTrackSystem.Models;
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
    }
}