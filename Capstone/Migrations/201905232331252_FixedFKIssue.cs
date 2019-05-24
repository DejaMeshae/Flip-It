namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixedFKIssue : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Sellers", new[] { "ApplicationUserId" });
            CreateIndex("dbo.Sellers", "ApplicationUserID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Sellers", new[] { "ApplicationUserID" });
            CreateIndex("dbo.Sellers", "ApplicationUserId");
        }
    }
}
