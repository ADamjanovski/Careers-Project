namespace PersonalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addednewattributetoJobAdvertisment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobAdvertisments", "SalaryFrom", c => c.Int(nullable: false));
            AddColumn("dbo.JobAdvertisments", "SalaryTo", c => c.Int(nullable: false));
            AddColumn("dbo.JobAdvertisments", "Type", c => c.Int(nullable: false));
            AddColumn("dbo.JobAdvertisments", "Condition", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobAdvertisments", "Condition");
            DropColumn("dbo.JobAdvertisments", "Type");
            DropColumn("dbo.JobAdvertisments", "SalaryTo");
            DropColumn("dbo.JobAdvertisments", "SalaryFrom");
        }
    }
}
