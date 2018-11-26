namespace BookShopWithAuthen.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcart : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CartDetails",
                c => new
                    {
                        UserID = c.String(nullable: false, maxLength: 128),
                        BookID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserID, t.BookID })
                .ForeignKey("dbo.Books", t => t.BookID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.BookID);
            
            AddColumn("dbo.Orders", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CartDetails", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.CartDetails", "BookID", "dbo.Books");
            DropIndex("dbo.CartDetails", new[] { "BookID" });
            DropIndex("dbo.CartDetails", new[] { "UserID" });
            DropColumn("dbo.Orders", "Status");
            DropTable("dbo.CartDetails");
        }
    }
}
