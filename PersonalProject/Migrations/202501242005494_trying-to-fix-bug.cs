namespace PersonalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tryingtofixbug : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.JobApplications", "jobAdvertisment_Id", "dbo.JobAdvertisments");
            DropForeignKey("dbo.JobAdvertisments", "Company_Id", "dbo.CustomUsers");
            DropIndex("dbo.JobAdvertisments", new[] { "Company_Id" });
            DropIndex("dbo.JobApplications", new[] { "jobAdvertisment_Id" });
            DropIndex("dbo.JobApplications", new[] { "Person_Id1" });

           // RenameColumn(table: "dbo.JobApplications", name: "Person_Id1", newName: "ForeignKeyPerson");
            RenameColumn(table: "dbo.JobApplications", name: "Person_Id", newName: "ForeignKeyPerson");
            RenameIndex(table: "dbo.JobApplications", name: "IX_Person_Id", newName: "IX_ForeignKeyPerson");
            AlterColumn("dbo.JobAdvertisments", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.JobAdvertisments", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.JobAdvertisments", "City", c => c.String(nullable: false));
            AlterColumn("dbo.JobAdvertisments", "Company_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.JobApplications", "CV", c => c.String(nullable: false));
            AlterColumn("dbo.JobApplications", "jobAdvertisment_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.JobAdvertisments", "Company_Id");
            CreateIndex("dbo.JobApplications", "jobAdvertisment_Id");
            AddForeignKey("dbo.JobApplications", "jobAdvertisment_Id", "dbo.JobAdvertisments", "Id", cascadeDelete: true);
            AddForeignKey("dbo.JobAdvertisments", "Company_Id", "dbo.CustomUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobAdvertisments", "Company_Id", "dbo.CustomUsers");
            DropForeignKey("dbo.JobApplications", "jobAdvertisment_Id", "dbo.JobAdvertisments");
            DropIndex("dbo.JobApplications", new[] { "jobAdvertisment_Id" });
            DropIndex("dbo.JobAdvertisments", new[] { "Company_Id" });
            AlterColumn("dbo.JobApplications", "jobAdvertisment_Id", c => c.Int());
            AlterColumn("dbo.JobApplications", "CV", c => c.String());
            AlterColumn("dbo.JobAdvertisments", "Company_Id", c => c.Int());
            AlterColumn("dbo.JobAdvertisments", "City", c => c.String());
            AlterColumn("dbo.JobAdvertisments", "Description", c => c.String());
            AlterColumn("dbo.JobAdvertisments", "Title", c => c.String());
            RenameIndex(table: "dbo.JobApplications", name: "IX_ForeignKeyPerson", newName: "IX_Person_Id");
            RenameColumn(table: "dbo.JobApplications", name: "ForeignKeyPerson", newName: "Person_Id");
            //RenameColumn(table: "dbo.JobApplications", name: "ForeignKeyPerson", newName: "Person_Id1");
          //  CreateIndex("dbo.JobApplications", "Person_Id1");
            CreateIndex("dbo.JobApplications", "jobAdvertisment_Id");
            CreateIndex("dbo.JobAdvertisments", "Company_Id");
            AddForeignKey("dbo.JobAdvertisments", "Company_Id", "dbo.CustomUsers", "Id");
            AddForeignKey("dbo.JobApplications", "jobAdvertisment_Id", "dbo.JobAdvertisments", "Id");
        }
    }
}
