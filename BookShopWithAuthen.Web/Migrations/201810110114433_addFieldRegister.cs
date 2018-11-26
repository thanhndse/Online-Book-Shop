namespace BookShopWithAuthen.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addFieldRegister : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "DayOfBirth", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "DayOfBirth");
        }
    }
}
