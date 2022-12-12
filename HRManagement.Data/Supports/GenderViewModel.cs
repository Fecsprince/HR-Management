using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.Data.Supports
{
    public class GenderViewModel
    {
        public string Gender { get; set; }

    }
    public class GenderTools
    {
        public List<GenderViewModel> GenderList()
        {
            return new List<GenderViewModel>()
        {
            new GenderViewModel(){Gender = "Female"},
            new GenderViewModel(){Gender = "Male"}

        };
        }
    }
}
