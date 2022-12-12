using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.Data.Supports
{
    public class EmployeeViewModel 
    {
        [Required(ErrorMessage = "*")]
        public string Id  { get; set; }
        public string User_ID { get; set; }
        [Required(ErrorMessage = "Employee email address is not on the database!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "*")]
        public string Name { get; set; }
        [Required(ErrorMessage = "*")]
        public string Gender { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DOB { get; set; }

        [Required(ErrorMessage = "*")]
        [ForeignKey("Designation")]
        public string Desgination_ID { get; set; }

        [Required(ErrorMessage = "*")]
        [ForeignKey("Branch")]
        public string JobUnit_Branch_ID { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{MM/dd/yyyy}", ApplyFormatInEditMode = true)]
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
        public double Tax
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

    }
}
