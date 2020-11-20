namespace BugTrackSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createdseedmethod : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "IsArchived", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Tickets", "Title", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Projects", "Title", c => c.String(nullable: false, maxLength: 30));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Projects", "Title", c => c.String());
            AlterColumn("dbo.Tickets", "Title", c => c.String());
            DropColumn("dbo.Projects", "IsArchived");
        }
    }
}
