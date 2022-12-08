﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.Data.Models
{
    public class Branch : BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }

    }
}
