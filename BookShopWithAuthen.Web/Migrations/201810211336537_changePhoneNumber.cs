namespace BookShopWithAuthen.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changePhoneNumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "PhoneNumber", c => c.String());
            DropColumn("dbo.Orders", "Phone");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "Phone", c => c.String());
            DropColumn("dbo.Orders", "PhoneNumber");
        }
    }
}
