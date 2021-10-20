namespace Cocycle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class routespostcode_relationship : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Routes", "PostCodeId", c => c.Int());
            AddColumn("dbo.Routes", "Description", c => c.String());
            CreateIndex("dbo.Routes", "PostCodeId");
            AddForeignKey("dbo.Routes", "PostCodeId", "dbo.PostCodes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Routes", "PostCodeId", "dbo.PostCodes");
            DropIndex("dbo.Routes", new[] { "PostCodeId" });
            DropColumn("dbo.Routes", "Description");
            DropColumn("dbo.Routes", "PostCodeId");
        }
    }
}
