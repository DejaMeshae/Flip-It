namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedUm : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "ItemPhoto");
            DropColumn("dbo.Items", "ItemPhoto");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Items", "ItemPhoto", c => c.Binary());
            AddColumn("dbo.AspNetUsers", "ItemPhoto", c => c.Binary());
        }
    }
}
