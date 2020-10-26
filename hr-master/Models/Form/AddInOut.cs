using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models.Form
{
    public class AddInOut
    {
        public int In_Out { get; set; }

        public string In_Out_Time { get; set; }

        public int In_Out_Status { get; set; }

        public int distance { get; set; }

        public string Employee_Latitude { get; set; }

        public string Employee_Longitude { get; set; }

        public DateTime In_Out_Date { get; set; }

    }
}
