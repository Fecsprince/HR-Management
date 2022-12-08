namespace HRManagement.WEBUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class employeeTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        DOB = c.DateTime(nullable: false),
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
            CreateIndex("dbo.AspNetUsers", "Employee_ID");
            AddForeignKey("dbo.AspNetUsers", "Employee_ID", "dbo.Employees", "Id");
            DropColumn("dbo.AspNetUsers", "Name");
            DropColumn("dbo.AspNetUsers", "DOB");
            DropColumn("dbo.AspNetUsers", "DOE");
            DropColumn("dbo.AspNetUsers", "MyProperty");
            DropColumn("dbo.AspNetUsers", "BasicSalary");
            DropColumn("dbo.AspNetUsers", "HousingAllowance");
            DropColumn("dbo.AspNetUsers", "TransportAllowance");
            DropColumn("dbo.AspNetUsers", "UtilityAllowance");
            DropColumn("dbo.AspNetUsers", "Pension");
            DropColumn("dbo.AspNetUsers", "Tax");
            DropColumn("dbo.AspNetUsers", "GrossSalary");
            DropColumn("dbo.AspNetUsers", "NetSalary");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "NetSalary", c => c.Double(nullable: false));
            AddColumn("dbo.AspNetUsers", "GrossSalary", c => c.Double(nullable: false));
            AddColumn("dbo.AspNetUsers", "Tax", c => c.Double(nullable: false));
            AddColumn("dbo.AspNetUsers", "Pension", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.AspNetUsers", "UtilityAllowance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.AspNetUsers", "TransportAllowance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.AspNetUsers", "HousingAllowance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.AspNetUsers", "BasicSalary", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.AspNetUsers", "MyProperty", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "DOE", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "DOB", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "Name", c => c.String());
            DropForeignKey("dbo.AspNetUsers", "Employee_ID", "dbo.Employees");
            DropIndex("dbo.AspNetUsers", new[] { "Employee_ID" });
            DropColumn("dbo.AspNetUsers", "Employee_ID");
            DropTable("dbo.Employees");
        }
    }
}
