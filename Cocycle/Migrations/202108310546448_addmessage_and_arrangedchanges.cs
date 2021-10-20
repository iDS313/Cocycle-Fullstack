namespace Cocycle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addmessage_and_arrangedchanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Arrangeds", "PostCodeId", c => c.Int());
            CreateIndex("dbo.Arrangeds", "PostCodeId");
            AddForeignKey("dbo.Arrangeds", "PostCodeId", "dbo.PostCodes", "Id");
            DropColumn("dbo.Arrangeds", "PostCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Arrangeds", "PostCode", c => c.String());
            DropForeignKey("dbo.Arrangeds", "PostCodeId", "dbo.PostCodes");
            DropIndex("dbo.Arrangeds", new[] { "PostCodeId" });
            DropColumn("dbo.Arrangeds", "PostCodeId");
        }
    }
}
