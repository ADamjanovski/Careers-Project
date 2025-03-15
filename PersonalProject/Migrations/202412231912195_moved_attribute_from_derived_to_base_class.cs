namespace PersonalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class moved_attribute_from_derived_to_base_class : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CustomUsers", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.CustomUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.CustomUsers", new[] { "User_Id" });
            DropIndex("dbo.CustomUsers", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.CustomUsers", "User_Id");
            DropColumn("dbo.CustomUsers", "ApplicationUser_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CustomUsers", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.CustomUsers", "User_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.CustomUsers", "ApplicationUser_Id");
            CreateIndex("dbo.CustomUsers", "User_Id");
            AddForeignKey("dbo.CustomUsers", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.CustomUsers", "User_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
