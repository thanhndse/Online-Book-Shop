namespace BookShopWithAuthen.Migrations
{
    using BookShopWithAuthen.Model.Models;
    using System.Data.Entity.Migrations;

    public partial class changeStatus : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "Status", c => c.Int(nullable: false, defaultValue: (int)StatusUser.Active));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "Status", c => c.Boolean(nullable: false));
        }
    }
}
