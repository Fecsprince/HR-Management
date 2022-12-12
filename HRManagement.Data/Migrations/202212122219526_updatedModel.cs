namespace HRManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Branches", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Branches", "Address", c => c.String(nullable: false));
            AlterColumn("dbo.Designations", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Designations", "Name", c => c.String());
            AlterColumn("dbo.Branches", "Address", c => c.String());
            AlterColumn("dbo.Branches", "Name", c => c.String());
        }
    }
}
