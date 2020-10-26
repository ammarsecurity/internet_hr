using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models.Dto
{
    public class EmployeeVacationsDto
    {
        public Guid Id { get; set; }
        public string employee_FullName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string VacationsNote { get; set; }
    }
}
