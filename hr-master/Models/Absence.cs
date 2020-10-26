using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models
{
    public class Absence
    {
        [Key]
        public Guid Id { get; set; }

        public Guid EmployeeId { get; set; }

        public DateTime AbsenceDate { get; set; }

        [DefaultValue("false")]
        public bool IsDelete { get; set; }


    }
}
