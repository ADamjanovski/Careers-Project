namespace PersonalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changes_to_CustomUser : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.CustomUsers", "Address_Street1");
            DropColumn("dbo.CustomUsers", "Address_City1");
            DropColumn("dbo.CustomUsers", "Address_PostalCode1");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CustomUsers", "Address_PostalCode1", c => c.String());
            AddColumn("dbo.CustomUsers", "Address_City1", c => c.String());
            AddColumn("dbo.CustomUsers", "Address_Street1", c => c.String());
        }
    }
}
