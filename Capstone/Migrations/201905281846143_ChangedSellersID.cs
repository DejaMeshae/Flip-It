namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedSellersID : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Items", name: "Id", newName: "SellersId");
            RenameIndex(table: "dbo.Items", name: "IX_Id", newName: "IX_SellersId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Items", name: "IX_SellersId", newName: "IX_Id");
            RenameColumn(table: "dbo.Items", name: "SellersId", newName: "Id");
        }
    }
}
