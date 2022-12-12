namespace HRManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modelx1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Employees", "Tax", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Employees", "Tax", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
