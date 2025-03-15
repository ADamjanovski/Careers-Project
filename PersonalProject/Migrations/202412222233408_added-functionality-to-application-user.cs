namespace PersonalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedfunctionalitytoapplicationuser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "CustomUser_Id", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "CustomUser_Id");
            AddForeignKey("dbo.AspNetUsers", "CustomUser_Id", "dbo.CustomUsers", "Id");
            DropColumn("dbo.CustomUsers", "Address_Country");
            DropColumn("dbo.CustomUsers", "Address_Country1");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CustomUsers", "Address_Country1", c => c.String());
            AddColumn("dbo.CustomUsers", "Address_Country", c => c.String());
            DropForeignKey("dbo.AspNetUsers", "CustomUser_Id", "dbo.CustomUsers");
            DropIndex("dbo.AspNetUsers", new[] { "CustomUser_Id" });
            DropColumn("dbo.AspNetUsers", "CustomUser_Id");
        }
    }
}
