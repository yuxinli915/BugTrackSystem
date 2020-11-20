using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTrackSystem.Models
{
    public static class ProjectHelper
    {
        static ApplicationDbContext db = new ApplicationDbContext();

        public static bool CreateProject(Project project)
        {
            if(db.Projects.Any(p => p.Title == project.Title))
            {
                return false;
            }
            db.Projects.Add(project);
            db.SaveChanges();
            return true;
        }

        public static bool DeleteProject(int id)
        {
            if (db.Projects.Any(p => p.Id == id))
            {
                var proj = db.Projects.Find(id);                             
                db.Projects.Remove(proj);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public static bool EditProject(Project project)
        {
            try
            {
                db.Entry(project).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
            
        }

        public static List<Project> GetAllProjectsForUser(string id)
        {
            return db.Users.Find(id).Projects.ToList();
        }

        public static List<Project> GetProjectDetailByProjectId(int? PId)
        {
            return db.Projects.Where(p => p.Id == PId).ToList();           
        }

        

        public static bool AssignUserToProject(string UserId, int ProjId)
        {
            var prj = db.Projects.Find(ProjId);
            var user = db.Users.Find(UserId);
            if (!prj.ApplicationUsers.Contains(user))
            {
                prj.ApplicationUsers.Add(user);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public static bool RemoveUserFromProject(string UserId, int ProjId)
        {
            var prj = db.Projects.Find(ProjId);
            var user = db.Users.Find(UserId);
            if (prj.ApplicationUsers.Contains(user))
            {
                prj.ApplicationUsers.Remove(user);
                db.SaveChanges();
                return true;
            }
            return false;
        }
    }
}