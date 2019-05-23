namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedMorePropertiesToItemsModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Items", "Lat", c => c.String());
            AddColumn("dbo.Items", "Lng", c => c.String());
            AddColumn("dbo.Items", "SellersId", c => c.Int(nullable: false));
            AlterColumn("dbo.Items", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            CreateIndex("dbo.Items", "SellersId");
            AddForeignKey("dbo.Items", "SellersId", "dbo.Sellers", "SellersId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Items", "SellersId", "dbo.Sellers");
            DropIndex("dbo.Items", new[] { "SellersId" });
            AlterColumn("dbo.Items", "Price", c => c.Int(nullable: false));
            DropColumn("dbo.Items", "SellersId");
            DropColumn("dbo.Items", "Lng");
            DropColumn("dbo.Items", "Lat");
        }
    }
}
