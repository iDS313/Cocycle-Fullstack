namespace Cocycle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Routes", "StartTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Routes", "EndTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Routes", "EndTime", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Routes", "StartTime", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
    }
}
