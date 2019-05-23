namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedPropertiesNameInItemsModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Items", "ItemPhoto", c => c.Binary());
            DropColumn("dbo.Items", "UserPhoto");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Items", "UserPhoto", c => c.Binary());
            DropColumn("dbo.Items", "ItemPhoto");
        }
    }
}
