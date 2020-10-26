using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models.Form
{
    public class AddOverTime
    {
        public int formtime { get; set; }
        public int totime { get; set; }
        public decimal OverTimePrice { get; set; }
    }
}
