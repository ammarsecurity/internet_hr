using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models
{
    public class ScheduleDelayPenalties
    {
        [Key]
        public Guid Id { get; set; }
        public int formtime { get; set; }
        public int totime { get; set; }
        public decimal PenaltiesPrice { get; set; }

        [DefaultValue("false")]
        public bool IsDelete { get; set; }

    }
}
