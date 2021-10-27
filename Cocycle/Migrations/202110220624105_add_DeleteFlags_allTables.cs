namespace Cocycle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_DeleteFlags_allTables : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Areas", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Arrangeds", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.PostCodes", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.States", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.FeedBacks", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.RouteGroups", "IsActive", c => c.Boolean(nullable: false));
            AlterColumn("dbo.FeedBacks", "description", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.FeedBacks", "description", c => c.String());
            DropColumn("dbo.RouteGroups", "IsActive");
            DropColumn("dbo.FeedBacks", "IsActive");
            DropColumn("dbo.States", "IsActive");
            DropColumn("dbo.PostCodes", "IsActive");
            DropColumn("dbo.Arrangeds", "IsActive");
            DropColumn("dbo.Areas", "IsActive");
        }
    }
}
