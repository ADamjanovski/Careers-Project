namespace PersonalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tryingtoremovecolumn : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.JobApplications", "FK_dbo.JobApplications_dbo.CustomUsers_Person_Id");
            DropIndex("dbo.JobApplications", new[] { "Person_Id1" });
            DropColumn("dbo.JobApplications", "Person_Id1");

        }

        public override void Down()
        {
        }
    }
}
