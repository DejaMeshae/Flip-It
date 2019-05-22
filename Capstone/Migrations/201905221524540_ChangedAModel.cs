namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedAModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        ItemsId = c.Int(nullable: false, identity: true),
                        ItemName = c.String(),
                        Price = c.Int(nullable: false),
                        Category = c.String(),
                        UserPhoto = c.Binary(),
                        Condition = c.String(),
                        Summary = c.String(),
                    })
                .PrimaryKey(t => t.ItemsId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Items");
        }
    }
}
