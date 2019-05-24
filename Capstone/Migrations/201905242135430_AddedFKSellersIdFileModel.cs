namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFKSellersIdFileModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Files", "SellersId", c => c.Int(nullable: false));
            CreateIndex("dbo.Files", "SellersId");
            AddForeignKey("dbo.Files", "SellersId", "dbo.Sellers", "SellersId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Files", "SellersId", "dbo.Sellers");
            DropIndex("dbo.Files", new[] { "SellersId" });
            DropColumn("dbo.Files", "SellersId");
        }
    }
}
