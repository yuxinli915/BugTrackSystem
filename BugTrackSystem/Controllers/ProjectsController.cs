using BugTrackSystem.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BugTrackSystem.Controllers
{
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Projects
        /*public ActionResult Index()
        {
            return View();
        }*/

        // GET: Projects/Details/5
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }           
            var project = ProjectHelper.GetProjectDetailByProjectId(id).ToList();
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult Create()
        {
            ViewBag.UserId = User.Identity.GetUserId();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult Create([Bind(Include = "Id,Title,Description,IsArchived")] Project project)
        {
            if (ModelState.IsValid)
            {
                ProjectHelper.CreateProject(project);
                db.SaveChanges();
                return Redirect("~/Manage/index");
            }
            ViewBag.UserId = User.Identity.GetUserId();

            return View(project);
        }

        // GET: Projects/Edit/5
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin, ProjectManager")]
        [HttpPost]
        public ActionResult Edit(int? id, string name)
        {
            ProjectHelper.EditProject(id, name);
            return RedirectToAction("Index", "Manage");
        }

        // GET: Projects/Delete/5
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [Authorize(Roles = "Admin, ProjectManager")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ProjectHelper.DeleteProject(id);
            return RedirectToAction("Index", "Manage");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //POST: AddUserToProject
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult AddUserToProject(string userId, int projId)
        {
            
            var user = db.Users.Find(userId);
            ProjectHelper.AssignUserToProject(user.Id, projId);

            return RedirectToAction("Index", "Manage" ,new { id = projId });
        }

        //POST: RemoveUserFromProject
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult RemoveUserFromProject(string userId, int projId)
        {
            var user = db.Users.Find(userId);
            ProjectHelper.RemoveUserFromProject(user.Id, projId);

            return RedirectToAction("Index", "Manage", new { id = projId });
        }
    }
}