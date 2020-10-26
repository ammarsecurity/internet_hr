using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models.Dto
{
    public class InOutMap
    {
        public Guid Id { get; set; }

        public string Employee_Latitude { get; set; }

        public string Employee_Longitude { get; set; }

        public string Employee_Name { get; set; }

        public string InOutColor { get; set; }

        public string InOut_Status { get; set; }
    }
}
