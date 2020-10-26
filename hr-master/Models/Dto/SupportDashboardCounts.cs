using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models.Dto
{
    public class SupportDashboardCounts
    {

        public int Alltask { get; set; }
        public int OpenTask { get; set; }

        public int Waittask { get; set; }

        public int DoneTask { get; set; }

        public decimal MySallery { get; set; }

    }
}
