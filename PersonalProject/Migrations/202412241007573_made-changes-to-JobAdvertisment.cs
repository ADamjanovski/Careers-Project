namespace PersonalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class madechangestoJobAdvertisment : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.JobAdvertisments", "companyId", "dbo.CustomUsers");
            DropIndex("dbo.JobAdvertisments", new[] { "companyId" });
            RenameColumn(table: "dbo.JobAdvertisments", name: "companyId", newName: "Company_Id");
            AlterColumn("dbo.JobAdvertisments", "Company_Id", c => c.Int());
            CreateIndex("dbo.JobAdvertisments", "Company_Id");
            AddForeignKey("dbo.JobAdvertisments", "Company_Id", "dbo.CustomUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobAdvertisments", "Company_Id", "dbo.CustomUsers");
            DropIndex("dbo.JobAdvertisments", new[] { "Company_Id" });
            AlterColumn("dbo.JobAdvertisments", "Company_Id", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.JobAdvertisments", name: "Company_Id", newName: "companyId");
            CreateIndex("dbo.JobAdvertisments", "companyId");
            AddForeignKey("dbo.JobAdvertisments", "companyId", "dbo.CustomUsers", "Id", cascadeDelete: true);
        }
    }
}
