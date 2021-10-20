namespace Cocycle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Arranged_addridecompleted : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Arrangeds", "RideCompleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Arrangeds", "RideCompleted");
        }
    }
}
