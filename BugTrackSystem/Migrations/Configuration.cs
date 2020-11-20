namespace BugTrackSystem.Migrations
{
    using BugTrackSystem.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using static BugTrackSystem.Models.TicketColumn;

    internal sealed class Configuration : DbMigrationsConfiguration<BugTrackSystem.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            context.Database.ExecuteSqlCommand("sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'");
            context.Database.ExecuteSqlCommand("sp_MSForEachTable 'IF OBJECT_ID(''?'') NOT IN (ISNULL(OBJECT_ID(''[dbo].[__MigrationHistory]''),0)) DELETE FROM ?'");
            context.Database.ExecuteSqlCommand("EXEC sp_MSForEachTable 'ALTER TABLE ? CHECK CONSTRAINT ALL'");
            /*Roles*/
            string[] roles = { "Admin", "Manager", "Developer", "Submitter" };
            foreach (var RoleName in roles)
            {
                if (!context.Roles.Any(r => r.Name == RoleName))/*Developer Role*/
                {
                    var store = new RoleStore<IdentityRole>(context);
                    var manager = new RoleManager<IdentityRole>(store);
                    var role = new IdentityRole { Name = RoleName };
                    manager.Create(role);
                }
            }

            /*Users*/
            var appUsers = new List<ApplicationUser>();
            string[] Users = { "admin@mysite.com",
                "manager1@mysite.com",  "manager2@mysite.com",
                "developer1@mysite.com",   "developer2@mysite.com",
                "submitter1@mysite.com",  "submitter2@mysite.com" };
            foreach (var usr in Users)
            {
                var PasswordHash = new PasswordHasher();
                ApplicationUser user;
                if (usr.StartsWith("sub"))
                {
                    user = new Submitter
                    {
                        UserName = usr,
                        Email = usr,
                        PasswordHash = PasswordHash.HashPassword("123456"),
                        Tickets = new HashSet<Ticket>()
                    };
                }else if(usr.StartsWith("dev"))
                {
                    user = new Developer
                    {
                        UserName = usr,
                        Email = usr,
                        PasswordHash = PasswordHash.HashPassword("123456")
                    };
                }
                else
                {
                    user = new ApplicationUser
                    {
                        UserName = usr,
                        Email = usr,
                        PasswordHash = PasswordHash.HashPassword("123456")
                    };
                }

                if (!context.Users.Any(u => u.UserName == usr))
                {
                    var store = new UserStore<ApplicationUser>(context);
                    var manager = new UserManager<ApplicationUser>(store);
                    manager.Create(user);
                    if (usr.StartsWith("admin"))
                    {
                        manager.AddToRole(user.Id, "Admin");
                    }
                    else if (usr.StartsWith("manager"))
                    {
                        manager.AddToRole(user.Id, "Manager");
                    }
                    else if (usr.StartsWith("developer"))
                    {
                        manager.AddToRole(user.Id, "Developer");
                    }
                    else if (usr.StartsWith("submitter"))
                    {
                        manager.AddToRole(user.Id, "Submitter");
                    }
                    appUsers.Add(user);
                }
            }
            if (appUsers.Count == 0)
            {
                appUsers = context.Users.ToList();
            }

            TicketProperty[] ticketProperties =
            {
                new TicketProperty{ Id=1,Name="Normal"},
                new TicketProperty{ Id=2,Name="Property 2"},
                new TicketProperty{ Id=3,Name="Urgent"},
            };
            TicketType[] ticketTypes =
            {
                new TicketType{ Id=1,Name="Request"},
                new TicketType{ Id=2,Name="Incident"},
                new TicketType{ Id=3,Name="Question"},
            };
            TicketStatus[] ticketStatuses =
            {
                new TicketStatus{ Id=1,Name="Assigned"},
                new TicketStatus{ Id=2,Name="Unassigned"},
                new TicketStatus{ Id=3,Name="Completed"},
            };
            context.Properties.AddOrUpdate(t => t.Name, ticketProperties);
            context.Types.AddOrUpdate(t => t.Name, ticketTypes);
            context.Statuses.AddOrUpdate(t => t.Name, ticketStatuses);

            Ticket[] tickets =
            {
                new Ticket(){
                    Id=1,
                    Title="ticket 1",
                    OwnerId=appUsers.Find(i=>i.UserName=="submitter1@mysite.com").Id,
                    ProjectId=1,
                    AssignedUserId=appUsers.Find(i=>i.UserName=="developer1@mysite.com").Id,
                    Created=DateTime.Now,
                    Updated=DateTime.Now,
                    TicketPropertyId=1,
                    TicketTypeId=1,
                    TicketStatusId=1,
                },
               new Ticket(){
                    Id=2,
                    Title="ticket 2",
                    OwnerId=appUsers.Find(i=>i.UserName=="submitter1@mysite.com").Id,
                    ProjectId=1,
                    AssignedUserId=appUsers.Find(i=>i.UserName=="developer2@mysite.com").Id,
                    Created=DateTime.Now,
                    Updated=DateTime.Now,
                     TicketPropertyId=1,
                    TicketTypeId=1,
                    TicketStatusId=1,
                },
                new Ticket(){
                     Id=3,
                    Title="ticket 3",
                    OwnerId=appUsers.Find(i=>i.UserName=="submitter1@mysite.com").Id,
                    ProjectId=1,
                    AssignedUserId=appUsers.Find(i=>i.UserName=="developer2@mysite.com").Id,
                    Created=DateTime.Now,
                    Updated=DateTime.Now,
                     TicketPropertyId=1,
                    TicketTypeId=2,
                    TicketStatusId=1,
                },
                 new Ticket(){
                      Id=4,
                    Title="ticket 4",
                    OwnerId=appUsers.Find(i=>i.UserName=="submitter1@mysite.com").Id,
                    ProjectId=1,
                    AssignedUserId=appUsers.Find(i=>i.UserName=="developer2@mysite.com").Id,
                    Created=DateTime.Now,
                    Updated=DateTime.Now,
                     TicketPropertyId=2,
                    TicketTypeId=1,
                    TicketStatusId=3,
                },
                 new Ticket(){
                      Id=5,
                    Title="ticket 5",
                    OwnerId=appUsers.Find(i=>i.UserName=="submitter1@mysite.com").Id,
                    ProjectId=1,
                    AssignedUserId=appUsers.Find(i=>i.UserName=="developer1@mysite.com").Id,
                    Created=DateTime.Now,
                    Updated=DateTime.Now,
                     TicketPropertyId=2,
                    TicketTypeId=2,
                    TicketStatusId=2,
                },
                      new Ticket(){
                           Id=6,
                    Title="ticket 6",
                    OwnerId=appUsers.Find(i=>i.UserName=="submitter2@mysite.com").Id,

                    ProjectId=2,
                    AssignedUserId=appUsers.Find(i=>i.UserName=="developer1@mysite.com").Id,
                    Created=DateTime.Now,
                    Updated=DateTime.Now,
                     TicketPropertyId=1,
                    TicketTypeId=1,
                    TicketStatusId=1,
                },
               new Ticket(){
                    Id=7,
                    Title="ticket 7",
                    OwnerId=appUsers.Find(i=>i.UserName=="submitter2@mysite.com").Id,

                    ProjectId=2,
                    AssignedUserId=appUsers.Find(i=>i.UserName=="developer2@mysite.com").Id,
                    Created=DateTime.Now,
                    Updated=DateTime.Now,
                     TicketPropertyId=1,
                    TicketTypeId=1,
                    TicketStatusId=1,
                },
                new Ticket(){
                     Id=8,
                    Title="ticket 8",
                    OwnerId=appUsers.Find(i=>i.UserName=="submitter2@mysite.com").Id,

                    ProjectId=2,
                    AssignedUserId=appUsers.Find(i=>i.UserName=="developer2@mysite.com").Id,
                    Created=DateTime.Now,
                    Updated=DateTime.Now,
                     TicketPropertyId=1,
                    TicketTypeId=1,
                    TicketStatusId=1,
                },
                 new Ticket(){
                      Id=9,
                    Title="ticket 9",
                    OwnerId=appUsers.Find(i=>i.UserName=="submitter2@mysite.com").Id,

                    ProjectId=2,
                    AssignedUserId=appUsers.Find(i=>i.UserName=="developer2@mysite.com").Id,
                    Created=DateTime.Now,
                    Updated=DateTime.Now,
                     TicketPropertyId=1,
                    TicketTypeId=1,
                    TicketStatusId=1,
                },
                 new Ticket(){
                      Id=10,
                    Title="ticket 10",
                    OwnerId=appUsers.Find(i=>i.UserName=="submitter2@mysite.com").Id,

                    ProjectId=2,
                    AssignedUserId=appUsers.Find(i=>i.UserName=="developer1@mysite.com").Id,
                    Created=DateTime.Now,
                    Updated=DateTime.Now,
                     TicketPropertyId=1,
                    TicketTypeId=1,
                    TicketStatusId=1,
                },
                new Ticket(){
                     Id=11,
                    Title="ticket 11",
                    OwnerId=appUsers.Find(i=>i.UserName=="submitter2@mysite.com").Id,

                    ProjectId=2,
                    AssignedUserId=appUsers.Find(i=>i.UserName=="developer1@mysite.com").Id,
                    Created=DateTime.Now,
                    Updated=DateTime.Now,
                     TicketPropertyId=1,
                    TicketTypeId=1,
                    TicketStatusId=1,
                },
               new Ticket(){
                    Id=12,
                    Title="ticket 12",
                    OwnerId=appUsers.Find(i=>i.UserName=="submitter2@mysite.com").Id,

                    ProjectId=2,
                    AssignedUserId=appUsers.Find(i=>i.UserName=="developer2@mysite.com").Id,
                    Created=DateTime.Now,
                    Updated=DateTime.Now,
                     TicketPropertyId=1,
                    TicketTypeId=1,
                    TicketStatusId=1,
                },
                new Ticket(){
                     Id=13,
                    Title="ticket 13",
                    OwnerId=appUsers.Find(i=>i.UserName=="submitter2@mysite.com").Id,

                    ProjectId=2,
                    AssignedUserId=appUsers.Find(i=>i.UserName=="developer2@mysite.com").Id,
                    Created=DateTime.Now,
                    Updated=DateTime.Now,
                     TicketPropertyId=1,
                    TicketTypeId=1,
                    TicketStatusId=1,
                },
                 new Ticket(){
                      Id=14,
                    Title="ticket 14",
                    OwnerId=appUsers.Find(i=>i.UserName=="submitter2@mysite.com").Id,

                    ProjectId=2,
                    AssignedUserId=appUsers.Find(i=>i.UserName=="developer2@mysite.com").Id,
                    Created=DateTime.Now,
                    Updated=DateTime.Now,
                     TicketPropertyId=3,
                    TicketTypeId=1,
                    TicketStatusId=2,
                },
                 new Ticket(){
                      Id=15,
                    Title="ticket 15",
                    OwnerId=appUsers.Find(i=>i.UserName=="submitter2@mysite.com").Id,

                    ProjectId=2,
                    AssignedUserId=appUsers.Find(i=>i.UserName=="developer1@mysite.com").Id,
                    Created=DateTime.Now,
                    Updated=DateTime.Now,
                     TicketPropertyId=2,
                    TicketTypeId=2,
                    TicketStatusId=3,
                }
            };

            Project[] projects = {
                new Project
                {
                    Id = 1,
                    Title="Bug Track Project 1",
                    IsArchived=false,
              //      Tickets= tickets.Where(i=>i.ProjectId==1).ToList(),
                    ApplicationUsers=appUsers.Where(i=>i.UserName.StartsWith("admin")
                    || i.UserName.StartsWith("manager1")).ToList()

                },
                new Project
                {
                    Id = 2,
                    Title = "Bug Track Project 2",
                    IsArchived=false,
                  //  Tickets= tickets.Where(i=>i.ProjectId==2).ToList(),
                    ApplicationUsers=appUsers.Where(i=>i.UserName.StartsWith("admin")
                    || i.UserName.StartsWith("manager2")).ToList()
                }
            };
            context.Projects.AddOrUpdate(t => t.Title, projects);

            context.Tickets.AddOrUpdate(t => t.Title, tickets);

        }
    }
}
