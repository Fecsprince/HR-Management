using HRManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace HRManagement.Data.Supports
{
    public class FinalContextSeeder : DropCreateDatabaseIfModelChanges<DataContext>
    {

        protected override void Seed(DataContext context)
        {

            Employee employee = new Employee()
            {
                Id = "08996620-6e92-4e16-ac18-8e8668c195a9",
                Name = "Admin",
                DOB = DateTime.Now,
                DOE = DateTime.Now,
                CreatedAt = DateTime.Now,
                Email = "admin@gmail.com",
                Gender = "Male",
                User_ID = "18996620-6e92-4e16-ac18-8e8668c195a9",
                Desgination_ID = "1",
                JobUnit_Branch_ID = "1",
                BasicSalary = 75000,
                HousingAllowance = 10000M,
                TransportAllowance = 5000,
                UtilityAllowance = 10000,
                Pension = 8000,
                Tax = 3.5,
                GrossSalary = 108000,
                NetSalary = 105375
            };

            context.Employees.Add(employee);


            List<Designation> designations = new List<Designation>()
            { new Designation(){
                Id = "1",
                Name = "Admin",
                Description = "Admin department"
            },
            new Designation(){
                Id = "2",
                Name = "TEMPORARY",
                Description = "Temporary designation"
            },
            };

            context.Designations.AddRange(designations);

            List<Branch> branches = new List<Branch>()
            { new Branch(){
                Id = "1",
                Name = "HQR",
                Address = "Zuba, FCT, NG"
            },
            new Branch(){
                Id = "2",
                Name = "TEMPORARY",
                Address = "Zuba, FCT, NG"
            }
           };

            context.Branches.AddRange(branches);

            context.SaveChanges();
            base.Seed(context);
        }
    }
}
