using System;
using System.ComponentModel.DataAnnotations;

namespace HRManagement.Data.Models
{
    public class FeedBack : BaseEntity
    {
        [Key]
        public string TicketNo { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public bool Status { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }

        public FeedBack()
        {            
            this.TicketNo = RandomDigits(6);
        }


        public string RandomDigits(int length)
        {
            var random = new Random();
            string s = string.Empty;
            for (int i = 0; i < length; i++)
                s = String.Concat(s, random.Next(10).ToString());
            return s;
        }
    }
}