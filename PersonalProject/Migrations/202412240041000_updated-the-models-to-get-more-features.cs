namespace PersonalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedthemodelstogetmorefeatures : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobAdvertisments", "Category_Name", c => c.String(maxLength: 128));
            AddColumn("dbo.JobAdvertisments", "Company_Id", c => c.Int());
            CreateIndex("dbo.JobAdvertisments", "Category_Name");
            CreateIndex("dbo.JobAdvertisments", "Company_Id");
            AddForeignKey("dbo.JobAdvertisments", "Category_Name", "dbo.Categories", "Name");
            AddForeignKey("dbo.JobAdvertisments", "Company_Id", "dbo.CustomUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobAdvertisments", "Company_Id", "dbo.CustomUsers");
            DropForeignKey("dbo.JobAdvertisments", "Category_Name", "dbo.Categories");
            DropIndex("dbo.JobAdvertisments", new[] { "Company_Id" });
            DropIndex("dbo.JobAdvertisments", new[] { "Category_Name" });
            DropColumn("dbo.JobAdvertisments", "Company_Id");
            DropColumn("dbo.JobAdvertisments", "Category_Name");
        }
    }
}
