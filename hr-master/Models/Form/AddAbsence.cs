using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models.Form
{
    public class AddAbsence
    {
        public Guid EmployeeId { get; set; }

        public DateTime AbsenceDate { get; set; }
    }
}
