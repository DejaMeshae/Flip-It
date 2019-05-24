namespace Capstone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedItemPhotoToIdentyModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ItemPhoto", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "ItemPhoto");
        }
    }
}
