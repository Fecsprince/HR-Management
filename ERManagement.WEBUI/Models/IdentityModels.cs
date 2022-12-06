using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using HRManagement.Data.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ERManagement.WEBUI.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        //  - USER EXTRA PROPERTIES

        public string Name { get; set; }
        public DateTime DOB { get; set; }

        [ForeignKey("Designation")]
        public string Desgination_ID { get; set; }

        [ForeignKey("Branch")]
        public string JobUnit_Branch_ID { get; set; }

        public DateTime DOE { get; set; }
        public int MyProperty { get; set; }

        public decimal BasicSalary { get; set; }

        public decimal HousingAllowance { get; set; }
        public decimal TransportAllowance { get; set; }
        public decimal UtilityAllowance { get; set; }
        public decimal Pension { get; set; }

        public double Tax { get; set; }

        public double GrossSalary { get; set; }

        public double NetSalary { get; set; } 

        //REFERENCES
        public virtual Designation Designation { get; set; } 
        public virtual Branch Branch { get; set; }


    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }



        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}