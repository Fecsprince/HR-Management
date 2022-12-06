using System;
using System.ComponentModel.DataAnnotations;

namespace HRManagement.Data.Models 
{
   public abstract class BaseEntity
    {
        [Key]
        public string Id { get; set; }
        [Display(Name = "Creaeted Date")]
        public DateTimeOffset CreatedAt { get; set; }

        public BaseEntity()
        {
            this.Id = Guid.NewGuid().ToString();
            this.CreatedAt = DateTime.Now;
        }
    }
}
