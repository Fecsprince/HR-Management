using HRManagement.WEBUI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity;

namespace HRManagement.WEBUI
{
    public class InitialContextSeeder : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            //CREATE ROLES

            List<IdentityRole> roles = new List<IdentityRole>()
            {
                new IdentityRole{ Id = "1", Name = "SuperAdmin"},
                new IdentityRole{ Id = "2", Name = "Employee"}
            };

            foreach (var role in roles)
            {
                context.Roles.Add(role);
            }
            context.SaveChanges();

            List<ApplicationUser> users = new List<ApplicationUser>() {
                new ApplicationUser
            {
                Id = "18996620-6e92-4e16-ac18-8e8668c195a9",
                AccessFailedCount = 0,
                Email = "admin@gmail.com",
                EmailConfirmed = false,
                LockoutEnabled = true,
                PasswordHash = "AFWOI7Q4oAcr5U+EHcUTlNV8vZJU/bw4fDW6yvZztoiPfSSt/1V9HF48mtnSiQYNEg==",
                SecurityStamp = "2973d2be-edad-43f2-9377-40e1c52d4460",
                UserName = "admin@gmail.com",
                PhoneNumber = "08063526371",
                TwoFactorEnabled = false
            },
            };

            foreach (var user in users)
            {
                context.Users.Add(user);
                context.SaveChanges();

                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                if (user.Email == "admin@gmail.com")
                {
                    UserManager.AddToRole(user.Id, "SuperAdmin");
                }
            }
            base.Seed(context);
        }
    }
}