using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HRManagement.Data.Supports
{
    public class FeedBackVM
    {
        public string Subject { get; set; }
        [Display(Name ="Ticket No")]
        public string TicketNo { get; set; }

        public bool Status { get; set; }

        public string Email { get; set; }
        [Display(Name ="Created")]
        public string CreatedAt { get; set; }
        public string Id { get; set; }

        public string Message { get; set; }
    }
}