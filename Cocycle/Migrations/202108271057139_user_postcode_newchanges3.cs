namespace Cocycle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class user_postcode_newchanges3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "PostCodeId", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "PostCodeId");
            AddForeignKey("dbo.AspNetUsers", "PostCodeId", "dbo.PostCodes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "PostCodeId", "dbo.PostCodes");
            DropIndex("dbo.AspNetUsers", new[] { "PostCodeId" });
            DropColumn("dbo.AspNetUsers", "PostCodeId");
        }
    }
}
