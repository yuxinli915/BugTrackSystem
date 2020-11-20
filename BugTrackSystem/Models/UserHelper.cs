using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTrackSystem.Models
{
    public class UserHelper
    {
        static ApplicationDbContext db = new ApplicationDbContext();
        static UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>
            (new UserStore<ApplicationUser>(db));
        static RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>
            (new RoleStore<IdentityRole>(db));

        public static bool CreateUser(string Email)
        {
            if (db.Users.Any(u => u.Email == Email))
                return false;
            else
            {
                var user = new ApplicationUser() { UserName = Email, Email = Email };
                db.Users.Add(user);
                db.SaveChanges();

                return true;
            }
        }
        public static void CreateRole(string roleName)
        {
            if (!roleManager.RoleExists(roleName))
                roleManager.Create(new IdentityRole { Name = roleName });
        }

        public static void DeleteRole(string roleName)
        {
            if (roleManager.RoleExists(roleName))
            {
                var role = roleManager.FindByName(roleName);
                roleManager.Delete(role);
            }
        }

        public static void RemoveUserFromRole(string userId, string roleName)
        {
            var user = userManager.FindById(userId);
            var currentUserRole = user.Roles.ToString();

            if (!roleManager.RoleExists(roleName))
                roleManager.Create(new IdentityRole { Name = roleName });

            userManager.RemoveFromRole(userId, currentUserRole);
        }

        public static void AddUserToRole(string userId, string roleName)
        {
            if (!userManager.IsInRole(userId, roleName))
                userManager.AddToRole(userId, roleName);
        }

        public static void CreateAssignedNotification(string userId)
        {
            Developer user = (Developer)userManager.FindById(userId);
            var assignedNotification = new TicketNotificaiton();
            assignedNotification.User = user;
            assignedNotification.UserId = userId;
            assignedNotification.Body = $"Created: {DateTime.Now.ToString()} - {user.UserName} has been assigned a new ticket.";
        }

        public static void CreateTicketChangeNotification(string userId, int ticketId)
        {
            Developer user = (Developer)userManager.FindById(userId);
            var ticketChangeNotification = new TicketNotificaiton();
            ticketChangeNotification.User = user;
            ticketChangeNotification.UserId = userId;
            ticketChangeNotification.Body = $"Created: {DateTime.Now.ToString()} - there was a change to ticket #{ticketId}.";
        }

        public static List<ApplicationUser> AllUsersInRole(string roleName)
        {
            var role = roleManager.Roles.FirstOrDefault(i => i.Name == roleName);

            return userManager.Users.Where(i => i.Roles.Any(j => j.RoleId == role.Id)).ToList();
        }
    }
}