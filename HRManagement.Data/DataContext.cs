using HRManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.Data
{
    public class DataContext : DbContext
    {
         
        public DataContext()
       :base("DefaultConnection") {

        }


        public DbSet<Designation> Designations { get; set; }
        public DbSet<Branch> Branches { get; set; } 
        public DbSet<FeedBack> FeedBacks { get; set; } 



    }
}
