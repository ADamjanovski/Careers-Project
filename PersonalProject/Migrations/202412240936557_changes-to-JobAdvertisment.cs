namespace PersonalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changestoJobAdvertisment : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.JobAdvertisments", "Company_Id", "dbo.CustomUsers");
            DropIndex("dbo.JobAdvertisments", new[] { "Company_Id" });
            RenameColumn(table: "dbo.JobAdvertisments", name: "Company_Id", newName: "companyId");
            RenameColumn(table: "dbo.JobApplications", name: "Person_Id1", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.JobApplications", name: "Person_Id", newName: "Person_Id1");
            RenameColumn(table: "dbo.JobApplications", name: "__mig_tmp__0", newName: "Person_Id");
            RenameIndex(table: "dbo.JobApplications", name: "IX_Person_Id1", newName: "__mig_tmp__0");
            RenameIndex(table: "dbo.JobApplications", name: "IX_Person_Id", newName: "IX_Person_Id1");
            RenameIndex(table: "dbo.JobApplications", name: "__mig_tmp__0", newName: "IX_Person_Id");
            AddColumn("dbo.JobAdvertisments", "Title", c => c.String());
            AddColumn("dbo.JobAdvertisments", "Description", c => c.String());
            AddColumn("dbo.JobAdvertisments", "Created", c => c.DateTime(nullable: false));
            AddColumn("dbo.JobAdvertisments", "ActiveTill", c => c.DateTime(nullable: false));
            AlterColumn("dbo.JobAdvertisments", "companyId", c => c.Int(nullable: false));
            CreateIndex("dbo.JobAdvertisments", "companyId");
            AddForeignKey("dbo.JobAdvertisments", "companyId", "dbo.CustomUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobAdvertisments", "companyId", "dbo.CustomUsers");
            DropIndex("dbo.JobAdvertisments", new[] { "companyId" });
            AlterColumn("dbo.JobAdvertisments", "companyId", c => c.Int());
            DropColumn("dbo.JobAdvertisments", "ActiveTill");
            DropColumn("dbo.JobAdvertisments", "Created");
            DropColumn("dbo.JobAdvertisments", "Description");
            DropColumn("dbo.JobAdvertisments", "Title");
            RenameIndex(table: "dbo.JobApplications", name: "IX_Person_Id", newName: "__mig_tmp__0");
            RenameIndex(table: "dbo.JobApplications", name: "IX_Person_Id1", newName: "IX_Person_Id");
            RenameIndex(table: "dbo.JobApplications", name: "__mig_tmp__0", newName: "IX_Person_Id1");
            RenameColumn(table: "dbo.JobApplications", name: "Person_Id", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.JobApplications", name: "Person_Id1", newName: "Person_Id");
            RenameColumn(table: "dbo.JobApplications", name: "__mig_tmp__0", newName: "Person_Id1");
            RenameColumn(table: "dbo.JobAdvertisments", name: "companyId", newName: "Company_Id");
            CreateIndex("dbo.JobAdvertisments", "Company_Id");
            AddForeignKey("dbo.JobAdvertisments", "Company_Id", "dbo.CustomUsers", "Id");
        }
    }
}
