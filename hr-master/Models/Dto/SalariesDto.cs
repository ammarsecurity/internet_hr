using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models.Dto
{
    public class SalariesDto
    {
        public string Employee_Name { get; set; }

        public decimal Salary_Name { get; set; }

        public DateTime Salary_Date { get; set; }

        public decimal Totel_Penalties { get; set; }

        public decimal Totel_Rewareds { get; set; }

        public decimal Totel_Salary { get; set; }
    }
}
