using System.ComponentModel.DataAnnotations;
using System.Web;

namespace HRManagement.Data.Supports
{
    public class UploadEmployeePassportViewModel
    {
        public string PassportUrl { get; set; }
        [Required]
        [Display(Name = "Employee Passport")]
        public HttpPostedFileBase PassportUpload { get; set; }

    }
}
