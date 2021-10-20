namespace Cocycle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmailAccount_Added : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmailAccounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                        Username = c.String(),
                        Host = c.String(),
                        Port = c.String(),
                        EnableSSL = c.Boolean(nullable: false),
                        UseDefaultCredentials = c.Boolean(nullable: false),
                        Modified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EmailAccounts");
        }
    }
}
