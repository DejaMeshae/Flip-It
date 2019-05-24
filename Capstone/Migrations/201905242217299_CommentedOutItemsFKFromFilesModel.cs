namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommentedOutItemsFKFromFilesModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Files", "ItemsId", "dbo.Items");
            DropIndex("dbo.Files", new[] { "ItemsId" });
            RenameColumn(table: "dbo.Files", name: "ItemsId", newName: "Items_ItemsId");
            AlterColumn("dbo.Files", "Items_ItemsId", c => c.Int());
            CreateIndex("dbo.Files", "Items_ItemsId");
            AddForeignKey("dbo.Files", "Items_ItemsId", "dbo.Items", "ItemsId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Files", "Items_ItemsId", "dbo.Items");
            DropIndex("dbo.Files", new[] { "Items_ItemsId" });
            AlterColumn("dbo.Files", "Items_ItemsId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Files", name: "Items_ItemsId", newName: "ItemsId");
            CreateIndex("dbo.Files", "ItemsId");
            AddForeignKey("dbo.Files", "ItemsId", "dbo.Items", "ItemsId", cascadeDelete: true);
        }
    }
}
