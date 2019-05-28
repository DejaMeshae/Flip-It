namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FKToFilesAndItems : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Files", "SellersId", "dbo.Sellers");
            DropForeignKey("dbo.Files", "Items_ItemsId", "dbo.Items");
            DropIndex("dbo.Files", new[] { "SellersId" });
            DropIndex("dbo.Files", new[] { "Items_ItemsId" });
            RenameColumn(table: "dbo.Files", name: "Items_ItemsId", newName: "ItemsId");
            AlterColumn("dbo.Files", "ItemsId", c => c.Int(nullable: true));
            CreateIndex("dbo.Files", "ItemsId");
            AddForeignKey("dbo.Files", "ItemsId", "dbo.Items", "ItemsId", cascadeDelete: true);
            DropColumn("dbo.Files", "SellersId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Files", "SellersId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Files", "ItemsId", "dbo.Items");
            DropIndex("dbo.Files", new[] { "ItemsId" });
            AlterColumn("dbo.Files", "ItemsId", c => c.Int());
            RenameColumn(table: "dbo.Files", name: "ItemsId", newName: "Items_ItemsId");
            CreateIndex("dbo.Files", "Items_ItemsId");
            CreateIndex("dbo.Files", "SellersId");
            AddForeignKey("dbo.Files", "Items_ItemsId", "dbo.Items", "ItemsId");
            AddForeignKey("dbo.Files", "SellersId", "dbo.Sellers", "SellersId", cascadeDelete: true);
        }
    }
}
