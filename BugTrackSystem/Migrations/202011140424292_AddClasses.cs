namespace BugTrackSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddClasses : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TicketDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TicketId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        Date = c.DateTime(nullable: false),
                        FilePath = c.String(),
                        Description = c.String(),
                        FileUrl = c.String(),
                        Body = c.String(),
                        Property = c.String(),
                        OldValue = c.String(),
                        NewValue = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        ApplicationUser_Id1 = c.String(maxLength: 128),
                        ApplicationUser_Id2 = c.String(maxLength: 128),
                        Ticket_Id = c.Int(),
                        Ticket_Id1 = c.Int(),
                        Ticket_Id2 = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id1)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id2)
                .ForeignKey("dbo.Tickets", t => t.Ticket_Id)
                .ForeignKey("dbo.Tickets", t => t.Ticket_Id1)
                .ForeignKey("dbo.Tickets", t => t.Ticket_Id2)
                .ForeignKey("dbo.Tickets", t => t.TicketId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.TicketId)
                .Index(t => t.UserId)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id1)
                .Index(t => t.ApplicationUser_Id2)
                .Index(t => t.Ticket_Id)
                .Index(t => t.Ticket_Id1)
                .Index(t => t.Ticket_Id2);
            
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AssignedUserId = c.String(maxLength: 128),
                        ProjectId = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                        Updated = c.DateTime(),
                        TicketStatusId = c.Int(nullable: false),
                        TicketTypeId = c.Int(nullable: false),
                        TicketPropertyId = c.Int(nullable: false),
                        Title = c.String(),
                        Description = c.String(),
                        OwnerId = c.String(maxLength: 128),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AssignedUserId)
                .ForeignKey("dbo.AspNetUsers", t => t.OwnerId)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .ForeignKey("dbo.TicketProperties", t => t.TicketPropertyId, cascadeDelete: true)
                .ForeignKey("dbo.TicketStatus", t => t.TicketStatusId, cascadeDelete: true)
                .ForeignKey("dbo.TicketTypes", t => t.TicketTypeId, cascadeDelete: true)
                .Index(t => t.AssignedUserId)
                .Index(t => t.ProjectId)
                .Index(t => t.TicketStatusId)
                .Index(t => t.TicketTypeId)
                .Index(t => t.TicketPropertyId)
                .Index(t => t.OwnerId)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        OwnerId = c.String(maxLength: 128),
                        Admin_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.OwnerId)
                .ForeignKey("dbo.AspNetUsers", t => t.Admin_Id)
                .Index(t => t.OwnerId)
                .Index(t => t.Admin_Id);
            
            CreateTable(
                "dbo.TicketProperties",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TicketStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TicketTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AspNetUsers", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.AspNetUsers", "Project_Id", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "Project_Id");
            AddForeignKey("dbo.AspNetUsers", "Project_Id", "dbo.Projects", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Projects", "Admin_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.TicketDetails", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.TicketDetails", "TicketId", "dbo.Tickets");
            DropForeignKey("dbo.Tickets", "TicketTypeId", "dbo.TicketTypes");
            DropForeignKey("dbo.Tickets", "TicketStatusId", "dbo.TicketStatus");
            DropForeignKey("dbo.Tickets", "TicketPropertyId", "dbo.TicketProperties");
            DropForeignKey("dbo.Tickets", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Projects", "OwnerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.Tickets", "OwnerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.TicketDetails", "Ticket_Id2", "dbo.Tickets");
            DropForeignKey("dbo.TicketDetails", "Ticket_Id1", "dbo.Tickets");
            DropForeignKey("dbo.TicketDetails", "Ticket_Id", "dbo.Tickets");
            DropForeignKey("dbo.Tickets", "AssignedUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Tickets", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.TicketDetails", "ApplicationUser_Id2", "dbo.AspNetUsers");
            DropForeignKey("dbo.TicketDetails", "ApplicationUser_Id1", "dbo.AspNetUsers");
            DropForeignKey("dbo.TicketDetails", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Projects", new[] { "Admin_Id" });
            DropIndex("dbo.Projects", new[] { "OwnerId" });
            DropIndex("dbo.Tickets", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Tickets", new[] { "OwnerId" });
            DropIndex("dbo.Tickets", new[] { "TicketPropertyId" });
            DropIndex("dbo.Tickets", new[] { "TicketTypeId" });
            DropIndex("dbo.Tickets", new[] { "TicketStatusId" });
            DropIndex("dbo.Tickets", new[] { "ProjectId" });
            DropIndex("dbo.Tickets", new[] { "AssignedUserId" });
            DropIndex("dbo.TicketDetails", new[] { "Ticket_Id2" });
            DropIndex("dbo.TicketDetails", new[] { "Ticket_Id1" });
            DropIndex("dbo.TicketDetails", new[] { "Ticket_Id" });
            DropIndex("dbo.TicketDetails", new[] { "ApplicationUser_Id2" });
            DropIndex("dbo.TicketDetails", new[] { "ApplicationUser_Id1" });
            DropIndex("dbo.TicketDetails", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.TicketDetails", new[] { "UserId" });
            DropIndex("dbo.TicketDetails", new[] { "TicketId" });
            DropIndex("dbo.AspNetUsers", new[] { "Project_Id" });
            DropColumn("dbo.AspNetUsers", "Project_Id");
            DropColumn("dbo.AspNetUsers", "Discriminator");
            DropTable("dbo.TicketTypes");
            DropTable("dbo.TicketStatus");
            DropTable("dbo.TicketProperties");
            DropTable("dbo.Projects");
            DropTable("dbo.Tickets");
            DropTable("dbo.TicketDetails");
        }
    }
}
