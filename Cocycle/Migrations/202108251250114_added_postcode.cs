namespace Cocycle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added_postcode : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PostCodes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AreaId = c.Int(nullable: false),
                        StateId = c.Int(nullable: false),
                        PostCodeName = c.String(),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
        }
        public override void Down()
        {
            DropTable("dbo.PostCodes");
        }
    }
}
