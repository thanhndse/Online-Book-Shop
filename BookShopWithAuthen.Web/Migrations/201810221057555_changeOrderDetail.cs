namespace BookShopWithAuthen.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeOrderDetail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderDetails", "BookName", c => c.String());
            AddColumn("dbo.OrderDetails", "Image", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderDetails", "Image");
            DropColumn("dbo.OrderDetails", "BookName");
        }
    }
}
