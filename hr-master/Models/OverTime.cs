using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models
{
    public class OverTime
    {
        public Guid Id { get; set; }
        public int formtime { get; set; }
        public int totime { get; set; }
        public decimal OverTimePrice { get; set; }

        [DefaultValue("false")]
        public bool IsDelete { get; set; }
    }
}
