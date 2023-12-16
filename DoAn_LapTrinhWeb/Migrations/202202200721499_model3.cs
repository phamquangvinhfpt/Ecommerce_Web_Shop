namespace DoAn_LapTrinhWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class model3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Feedback_Image", new[] { "feedback_id", "account_id" }, "dbo.Feedback");
            DropForeignKey("dbo.ReplyFeedback", new[] { "Feedback_feedback_id", "Feedback_account_id" }, "dbo.Feedback");
            DropIndex("dbo.Feedback_Image", new[] { "feedback_id", "account_id" });
            DropIndex("dbo.ReplyFeedback", new[] { "Feedback_feedback_id", "Feedback_account_id" });
            DropColumn("dbo.ReplyFeedback", "feedback_id");
            RenameColumn(table: "dbo.ReplyFeedback", name: "Feedback_feedback_id", newName: "feedback_id");
            DropPrimaryKey("dbo.Feedback");
            AddColumn("dbo.ReplyFeedback", "account_id", c => c.Int(nullable: false));
            AlterColumn("dbo.ReplyFeedback", "feedback_id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Feedback", "feedback_id");
            CreateIndex("dbo.ReplyFeedback", "feedback_id");
            CreateIndex("dbo.ReplyFeedback", "account_id");
            AddForeignKey("dbo.ReplyFeedback", "account_id", "dbo.Account", "account_id", cascadeDelete: true);
            AddForeignKey("dbo.ReplyFeedback", "feedback_id", "dbo.Feedback", "feedback_id", cascadeDelete: true);
            DropColumn("dbo.ReplyFeedback", "account_name");
            DropColumn("dbo.ReplyFeedback", "Feedback_account_id");
            DropTable("dbo.Feedback_Image");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Feedback_Image",
                c => new
                    {
                        image_id = c.Int(nullable: false, identity: true),
                        feedback_id = c.Int(nullable: false),
                        account_id = c.Int(nullable: false),
                        image = c.String(unicode: false, storeType: "text"),
                        create_at = c.DateTime(nullable: false),
                        create_by = c.String(nullable: false, maxLength: 20, unicode: false),
                        status = c.String(maxLength: 1, fixedLength: true, unicode: false),
                        update_by = c.String(nullable: false, maxLength: 20),
                        update_at = c.DateTime(),
                    })
                .PrimaryKey(t => t.image_id);
            
            AddColumn("dbo.ReplyFeedback", "Feedback_account_id", c => c.Int());
            AddColumn("dbo.ReplyFeedback", "account_name", c => c.String(maxLength: 20));
            DropForeignKey("dbo.ReplyFeedback", "feedback_id", "dbo.Feedback");
            DropForeignKey("dbo.ReplyFeedback", "account_id", "dbo.Account");
            DropIndex("dbo.ReplyFeedback", new[] { "account_id" });
            DropIndex("dbo.ReplyFeedback", new[] { "feedback_id" });
            DropPrimaryKey("dbo.Feedback");
            AlterColumn("dbo.ReplyFeedback", "feedback_id", c => c.Int());
            DropColumn("dbo.ReplyFeedback", "account_id");
            AddPrimaryKey("dbo.Feedback", new[] { "feedback_id", "account_id" });
            RenameColumn(table: "dbo.ReplyFeedback", name: "feedback_id", newName: "Feedback_feedback_id");
            AddColumn("dbo.ReplyFeedback", "feedback_id", c => c.Int(nullable: false));
            CreateIndex("dbo.ReplyFeedback", new[] { "Feedback_feedback_id", "Feedback_account_id" });
            CreateIndex("dbo.Feedback_Image", new[] { "feedback_id", "account_id" });
            AddForeignKey("dbo.ReplyFeedback", new[] { "Feedback_feedback_id", "Feedback_account_id" }, "dbo.Feedback", new[] { "feedback_id", "account_id" });
            AddForeignKey("dbo.Feedback_Image", new[] { "feedback_id", "account_id" }, "dbo.Feedback", new[] { "feedback_id", "account_id" });
        }
    }
}
