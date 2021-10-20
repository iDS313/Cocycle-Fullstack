namespace Cocycle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class route_changesnew : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Routes", "PostCode");
            DropColumn("dbo.Routes", "RouteVideo");
            DropColumn("dbo.RouteSchedules", "StartTime");
            DropColumn("dbo.RouteSchedules", "EndTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RouteSchedules", "EndTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.RouteSchedules", "StartTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Routes", "RouteVideo", c => c.String());
            AddColumn("dbo.Routes", "PostCode", c => c.String());
        }
    }
}
