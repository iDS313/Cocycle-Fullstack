namespace Cocycle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Feedback_Changed1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FeedBacks", "RideId", "dbo.Arrangeds");
            DropIndex("dbo.FeedBacks", new[] { "RideId" });
            AddColumn("dbo.FeedBacks", "RouteId", c => c.Int());
            AlterColumn("dbo.FeedBacks", "RideId", c => c.Int());
            CreateIndex("dbo.FeedBacks", "RideId");
            CreateIndex("dbo.FeedBacks", "RouteId");
            AddForeignKey("dbo.FeedBacks", "RouteId", "dbo.Routes", "Id");
            AddForeignKey("dbo.FeedBacks", "RideId", "dbo.Arrangeds", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FeedBacks", "RideId", "dbo.Arrangeds");
            DropForeignKey("dbo.FeedBacks", "RouteId", "dbo.Routes");
            DropIndex("dbo.FeedBacks", new[] { "RouteId" });
            DropIndex("dbo.FeedBacks", new[] { "RideId" });
            AlterColumn("dbo.FeedBacks", "RideId", c => c.Int(nullable: false));
            DropColumn("dbo.FeedBacks", "RouteId");
            CreateIndex("dbo.FeedBacks", "RideId");
            AddForeignKey("dbo.FeedBacks", "RideId", "dbo.Arrangeds", "Id", cascadeDelete: true);
        }
    }
}
