namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedBuyersModelToMessageModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Buyers", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Buyers", new[] { "ApplicationUserId" });
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        MessageId = c.Int(nullable: false, identity: true),
                        MessageBox = c.String(),
                        SellersId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MessageId)
                .ForeignKey("dbo.Sellers", t => t.SellersId, cascadeDelete: true)
                .Index(t => t.SellersId);
            
            DropTable("dbo.Buyers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Buyers",
                c => new
                    {
                        BuyersId = c.Int(nullable: false, identity: true),
                        Firstname = c.String(),
                        Lastname = c.String(),
                        Address = c.String(),
                        City = c.String(),
                        State = c.String(),
                        ZipCode = c.String(),
                        UserPhoto = c.Binary(),
                        Lat = c.String(),
                        Lng = c.String(),
                        ApplicationUserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.BuyersId);
            
            DropForeignKey("dbo.Messages", "SellersId", "dbo.Sellers");
            DropIndex("dbo.Messages", new[] { "SellersId" });
            DropTable("dbo.Messages");
            CreateIndex("dbo.Buyers", "ApplicationUserId");
            AddForeignKey("dbo.Buyers", "ApplicationUserId", "dbo.AspNetUsers", "Id");
        }
    }
}
