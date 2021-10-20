namespace Cocycle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class arrange_changes : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Arrangeds", "OfferingDates", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Arrangeds", "OfferingDates", c => c.String());
        }
    }
}
