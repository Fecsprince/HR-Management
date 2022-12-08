namespace ERManagement.WEBUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class employeeTable2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "JobUnit_Branch_ID", "dbo.Branches");
            DropForeignKey("dbo.AspNetUsers", "Desgination_ID", "dbo.Designations");
            DropIndex("dbo.AspNetUsers", new[] { "Desgination_ID" });
            DropIndex("dbo.AspNetUsers", new[] { "JobUnit_Branch_ID" });
            AddColumn("dbo.Employees", "Gender", c => c.String());
            AddColumn("dbo.Employees", "Desgination_ID", c => c.String(maxLength: 128));
            AddColumn("dbo.Employees", "JobUnit_Branch_ID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Employees", "Desgination_ID");
            CreateIndex("dbo.Employees", "JobUnit_Branch_ID");
            AddForeignKey("dbo.Employees", "JobUnit_Branch_ID", "dbo.Branches", "Id");
            AddForeignKey("dbo.Employees", "Desgination_ID", "dbo.Designations", "Id");
            DropColumn("dbo.AspNetUsers", "Desgination_ID");
            DropColumn("dbo.AspNetUsers", "JobUnit_Branch_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "JobUnit_Branch_ID", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUsers", "Desgination_ID", c => c.String(maxLength: 128));
            DropForeignKey("dbo.Employees", "Desgination_ID", "dbo.Designations");
            DropForeignKey("dbo.Employees", "JobUnit_Branch_ID", "dbo.Branches");
            DropIndex("dbo.Employees", new[] { "JobUnit_Branch_ID" });
            DropIndex("dbo.Employees", new[] { "Desgination_ID" });
            DropColumn("dbo.Employees", "JobUnit_Branch_ID");
            DropColumn("dbo.Employees", "Desgination_ID");
            DropColumn("dbo.Employees", "Gender");
            CreateIndex("dbo.AspNetUsers", "JobUnit_Branch_ID");
            CreateIndex("dbo.AspNetUsers", "Desgination_ID");
            AddForeignKey("dbo.AspNetUsers", "Desgination_ID", "dbo.Designations", "Id");
            AddForeignKey("dbo.AspNetUsers", "JobUnit_Branch_ID", "dbo.Branches", "Id");
        }
    }
}
