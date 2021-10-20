namespace Cocycle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class postcode_state_city_relations : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.PostCodes", "AreaId");
            CreateIndex("dbo.PostCodes", "StateId");
            AddForeignKey("dbo.PostCodes", "AreaId", "dbo.Areas", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PostCodes", "StateId", "dbo.States", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PostCodes", "StateId", "dbo.States");
            DropForeignKey("dbo.PostCodes", "AreaId", "dbo.Areas");
            DropIndex("dbo.PostCodes", new[] { "StateId" });
            DropIndex("dbo.PostCodes", new[] { "AreaId" });
        }
    }
}
