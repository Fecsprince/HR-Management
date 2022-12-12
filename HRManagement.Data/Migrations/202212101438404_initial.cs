namespace HRManagement.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
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
                        User_ID = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        Gender = c.String(nullable: false),
                        DOB = c.DateTime(nullable: false),
                        Desgination_ID = c.String(nullable: false, maxLength: 128),
                        JobUnit_Branch_ID = c.String(nullable: false, maxLength: 128),
                        DOE = c.DateTime(nullable: false),
                        BasicSalary = c.Decimal(nullable: false, precision: 18, scale: 2),
                        HousingAllowance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TransportAllowance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UtilityAllowance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Pension = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GrossSalary = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NetSalary = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Branches", t => t.JobUnit_Branch_ID, cascadeDelete: true)
                .ForeignKey("dbo.Designations", t => t.Desgination_ID, cascadeDelete: true)
                .Index(t => t.Desgination_ID)
                .Index(t => t.JobUnit_Branch_ID);
            
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
                "dbo.FeedBacks",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        TicketNo = c.String(),
                        Subject = c.String(nullable: false),
                        Status = c.Boolean(nullable: false),
                        Email = c.String(nullable: false),
                        Message = c.String(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employees", "Desgination_ID", "dbo.Designations");
            DropForeignKey("dbo.Employees", "JobUnit_Branch_ID", "dbo.Branches");
            DropIndex("dbo.Employees", new[] { "JobUnit_Branch_ID" });
            DropIndex("dbo.Employees", new[] { "Desgination_ID" });
            DropTable("dbo.FeedBacks");
            DropTable("dbo.Designations");
            DropTable("dbo.Employees");
            DropTable("dbo.Branches");
        }
    }
}
