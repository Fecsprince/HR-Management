namespace HRManagement.WEBUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HRDBEmployeeCRUD2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Employees", "JobUnit_Branch_ID", "dbo.Branches");
            DropForeignKey("dbo.Employees", "Desgination_ID", "dbo.Designations");
            DropForeignKey("dbo.AspNetUsers", "Employee_ID", "dbo.Employees");
            DropIndex("dbo.AspNetUsers", new[] { "Employee_ID" });
            DropIndex("dbo.Employees", new[] { "Desgination_ID" });
            DropIndex("dbo.Employees", new[] { "JobUnit_Branch_ID" });
            DropColumn("dbo.AspNetUsers", "Employee_ID");
            DropTable("dbo.Employees");
            DropTable("dbo.Branches");
            DropTable("dbo.Designations");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Designations",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Description = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Branches",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Address = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Gender = c.String(),
                        DOB = c.DateTime(nullable: false),
                        Desgination_ID = c.String(maxLength: 128),
                        JobUnit_Branch_ID = c.String(maxLength: 128),
                        DOE = c.DateTime(nullable: false),
                        BasicSalary = c.Decimal(nullable: false, precision: 18, scale: 2),
                        HousingAllowance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TransportAllowance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UtilityAllowance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Pension = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax = c.Double(nullable: false),
                        GrossSalary = c.Double(nullable: false),
                        NetSalary = c.Double(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AspNetUsers", "Employee_ID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Employees", "JobUnit_Branch_ID");
            CreateIndex("dbo.Employees", "Desgination_ID");
            CreateIndex("dbo.AspNetUsers", "Employee_ID");
            AddForeignKey("dbo.AspNetUsers", "Employee_ID", "dbo.Employees", "Id");
            AddForeignKey("dbo.Employees", "Desgination_ID", "dbo.Designations", "Id");
            AddForeignKey("dbo.Employees", "JobUnit_Branch_ID", "dbo.Branches", "Id");
        }
    }
}
