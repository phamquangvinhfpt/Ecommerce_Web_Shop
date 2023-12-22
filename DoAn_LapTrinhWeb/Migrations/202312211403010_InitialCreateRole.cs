namespace DoAn_LapTrinhWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreateRole : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        role_id = c.Int(nullable: false, identity: true),
                        role_name = c.String(),
                    })
                .PrimaryKey(t => t.role_id);

            //Set identity start value to 0
            Sql("DBCC CHECKIDENT ('Roles', RESEED, 0)");
            
            CreateTable(
                "dbo.Account_Role",
                c => new
                    {
                        account_id = c.Int(nullable: false),
                        role_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.account_id, t.role_id })
                .ForeignKey("dbo.Account", t => t.account_id, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.role_id, cascadeDelete: true)
                .Index(t => t.account_id)
                .Index(t => t.role_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Account_Role", "role_id", "dbo.Roles");
            DropForeignKey("dbo.Account_Role", "account_id", "dbo.Account");
            DropIndex("dbo.Account_Role", new[] { "role_id" });
            DropIndex("dbo.Account_Role", new[] { "account_id" });
            DropTable("dbo.Account_Role");
            DropTable("dbo.Roles");
        }
    }
}
