namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedBothToFilesModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Files", "Sellers_SellersId", "dbo.Sellers");
            DropIndex("dbo.Files", new[] { "Sellers_SellersId" });
            RenameColumn(table: "dbo.Files", name: "Sellers_SellersId", newName: "SellersId");
            AlterColumn("dbo.Files", "SellersId", c => c.Int(nullable: false));
            CreateIndex("dbo.Files", "SellersId");
            AddForeignKey("dbo.Files", "SellersId", "dbo.Sellers", "SellersId", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Files", "SellersId", "dbo.Sellers");
            DropIndex("dbo.Files", new[] { "SellersId" });
            AlterColumn("dbo.Files", "SellersId", c => c.Int());
            RenameColumn(table: "dbo.Files", name: "SellersId", newName: "Sellers_SellersId");
            CreateIndex("dbo.Files", "Sellers_SellersId");
            AddForeignKey("dbo.Files", "Sellers_SellersId", "dbo.Sellers", "SellersId");
        }
    }
}
