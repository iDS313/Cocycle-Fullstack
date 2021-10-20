namespace Cocycle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class aspUsersChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Created", c => c.DateTime(nullable: false));
            CreateIndex("dbo.AspNetUsers", "AreaId");
            CreateIndex("dbo.AspNetUsers", "StateId");
            AddForeignKey("dbo.AspNetUsers", "AreaId", "dbo.Areas", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUsers", "StateId", "dbo.States", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "StateId", "dbo.States");
            DropForeignKey("dbo.AspNetUsers", "AreaId", "dbo.Areas");
            DropIndex("dbo.AspNetUsers", new[] { "StateId" });
            DropIndex("dbo.AspNetUsers", new[] { "AreaId" });
            DropColumn("dbo.AspNetUsers", "Created");
        }
    }
}
