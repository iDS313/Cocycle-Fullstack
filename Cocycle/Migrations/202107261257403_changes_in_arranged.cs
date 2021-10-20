namespace Cocycle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changes_in_arranged : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Arrangeds", "StartTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Arrangeds", "StartTime", c => c.DateTime());
        }
    }
}
