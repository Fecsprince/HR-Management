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
        [Required(ErrorMessage = "*")]
        public string Name { get; set; }
        [Required(ErrorMessage = "*")]
        public string Gender { get; set; }
        public DateTime DOB { get; set; }

        [Required(ErrorMessage = "*")]
        [ForeignKey("Designation")]
        public string Desgination_ID { get; set; }

        [Required(ErrorMessage = "*")]
        [ForeignKey("Branch")]
        public string JobUnit_Branch_ID { get; set; }
        public DateTime DOE { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Currency)]
        public decimal BasicSalary { get; set; }
       
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Currency)]
        public decimal HousingAllowance { get; set; }
       
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Currency)]
        public decimal TransportAllowance { get; set; }
        
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Currency)]
        public decimal UtilityAllowance
        {
            get; set;
        }
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Currency)]
        public decimal Pension
        {
            get; set;
        }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Currency)]
        public decimal Tax
        {
            get; set;
        }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Currency)]
        public decimal GrossSalary
        {
            get; set;
        }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Currency)]
        public decimal NetSalary
        {
            get; set;
        }

        public virtual Designation Designation { get; set; }
        public virtual Branch Branch { get; set; }

    }
}
