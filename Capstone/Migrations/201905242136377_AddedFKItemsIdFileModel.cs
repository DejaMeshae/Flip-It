namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFKItemsIdFileModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Files", "ItemsId", c => c.Int(nullable: false));
            CreateIndex("dbo.Files", "ItemsId");
            AddForeignKey("dbo.Files", "ItemsId", "dbo.Items", "ItemsId", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Files", "ItemsId", "dbo.Items");
            DropIndex("dbo.Files", new[] { "ItemsId" });
            DropColumn("dbo.Files", "ItemsId");
        }
    }
}
