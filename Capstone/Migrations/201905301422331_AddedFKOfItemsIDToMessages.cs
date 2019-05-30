namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFKOfItemsIDToMessages : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "ItemsId", c => c.Int(nullable: true));
            CreateIndex("dbo.Messages", "ItemsId");
            AddForeignKey("dbo.Messages", "ItemsId", "dbo.Items", "ItemsId", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "ItemsId", "dbo.Items");
            DropIndex("dbo.Messages", new[] { "ItemsId" });
            DropColumn("dbo.Messages", "ItemsId");
        }
    }
}
