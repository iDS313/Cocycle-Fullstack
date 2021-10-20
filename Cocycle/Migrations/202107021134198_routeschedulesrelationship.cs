namespace Cocycle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class routeschedulesrelationship : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.RouteSchedules", "RouteId");
            AddForeignKey("dbo.RouteSchedules", "RouteId", "dbo.Routes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RouteSchedules", "RouteId", "dbo.Routes");
            DropIndex("dbo.RouteSchedules", new[] { "RouteId" });
        }
    }
}
