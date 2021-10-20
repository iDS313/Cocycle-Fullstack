namespace Cocycle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class area_state_changes : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Areas", "StateId");
            AddForeignKey("dbo.Areas", "StateId", "dbo.States", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Areas", "StateId", "dbo.States");
            DropIndex("dbo.Areas", new[] { "StateId" });
        }
    }
}
