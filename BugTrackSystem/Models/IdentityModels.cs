﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using static BugTrackSystem.Models.TicketColumn;

namespace BugTrackSystem.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public  string GetRoleName(string userId)
        {
            return UserHelper.GetRoleForUser(userId);
        }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public virtual ICollection<Project> Projects { get; set; }

        public virtual ICollection<TicketAttachment> Attachments { get; set; }
        public virtual ICollection<TicketComment> Comments { get; set; }
        public virtual ICollection<TicketHistory> Histories { get; set; }

        public ApplicationUser()
        {
            Attachments = new HashSet<TicketAttachment>();
            Comments = new HashSet<TicketComment>();
            Histories = new HashSet<TicketHistory>();
            Projects = new HashSet<Project>();
        }
    }

    public class Submitter : ApplicationUser
    {
        public virtual ICollection<Ticket> Tickets { get; set; }
        public Submitter()
        {
            Tickets = new HashSet<Ticket>();
        }
    }

    public class Developer : ApplicationUser
    {
        public virtual ICollection<TicketNotificaiton> Notificaitons { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
        public Developer()
        {
            Notificaitons = new HashSet<TicketNotificaiton>();
            Tickets = new HashSet<Ticket>();
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TicketAttachment> Attachments { get; set; }
        public DbSet<TicketComment> Comments { get; set; }
        public DbSet<TicketHistory> Histories { get; set; }
        public DbSet<TicketType> Types { get; set; }
        public DbSet<TicketProperty> Properties { get; set; }
        public DbSet<TicketStatus> Statuses { get; set; }
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

    }
}