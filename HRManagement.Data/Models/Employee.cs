using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.Data.Models
{
   public class Employee : BaseEntity
    {
        public string Name { get; set; }
        public DateTime DOB { get; set; }      

        public DateTime DOE { get; set; }

        public decimal BasicSalary { get; set; }

        public decimal HousingAllowance { get; set; }
        public decimal TransportAllowance { get; set; }
        public decimal UtilityAllowance { get; set; }
        public decimal Pension { get; set; }

        public double Tax { get; set; }

        public double GrossSalary { get; set; }

        public double NetSalary { get; set; }

    }
}
