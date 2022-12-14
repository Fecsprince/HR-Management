using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.Data.Models
{
    public class Employee : BaseEntity
    {
        [Required(ErrorMessage = "Employee is not on the database!")]
        public string User_ID { get; set; }
        [Required(ErrorMessage = "Employee email address is not on the database!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Email is missing")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Gender is missing")]
        public string Gender { get; set; }
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }

        public byte[] Passport { get; set; }


        [Required(ErrorMessage = "Designation is missing")]
        [ForeignKey("Designation")]
        public string Desgination_ID { get; set; }

        [Required(ErrorMessage = "Branch is missing")]
        [ForeignKey("Branch")]
        public string JobUnit_Branch_ID { get; set; }
        [DataType(DataType.Date)]
        public DateTime DOE { get; set; }

        [Required(ErrorMessage = "Basic Salary is missing")]
        [DataType(DataType.Currency)]
        public decimal BasicSalary { get; set; }

        [Required(ErrorMessage = "Housing Allowance is missing")]
        [DataType(DataType.Currency)]
        public decimal HousingAllowance { get; set; }

        [Required(ErrorMessage = "Transportation Allowance is missing")]
        [DataType(DataType.Currency)]
        public decimal TransportAllowance { get; set; }

        [Required(ErrorMessage = "Utility Allowance is missing")]
        [DataType(DataType.Currency)]
        public decimal UtilityAllowance
        {
            get; set;
        }
        [Required(ErrorMessage = "Pension is missing")]
        [DataType(DataType.Currency)]
        public decimal Pension
        {
            get; set;
        }

        [Required(ErrorMessage = "Tax is missing")]
        public double Tax
        {
            get; set;
        }

        [Required(ErrorMessage = "Gross Salary is missing")]
        [DataType(DataType.Currency)]
        public decimal GrossSalary
        {
            get; set;
        }

        [Required(ErrorMessage = "Net Salary is missing")]
        [DataType(DataType.Currency)]
        public decimal NetSalary
        {
            get; set;
        }

        public virtual Designation Designation { get; set; }
        public virtual Branch Branch { get; set; }

    }
}
