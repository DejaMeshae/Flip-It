namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedStatusToItemsModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Items", "Status", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Items", "Status");
        }
    }
}
