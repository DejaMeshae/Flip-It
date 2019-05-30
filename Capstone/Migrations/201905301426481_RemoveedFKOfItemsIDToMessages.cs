namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveedFKOfItemsIDToMessages : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Messages", "ItemsId", "dbo.Items");
            DropIndex("dbo.Messages", new[] { "ItemsId" });
            DropColumn("dbo.Messages", "ItemsId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Messages", "ItemsId", c => c.Int(nullable: false));
            CreateIndex("dbo.Messages", "ItemsId");
            AddForeignKey("dbo.Messages", "ItemsId", "dbo.Items", "ItemsId", cascadeDelete: true);
        }
    }
}
