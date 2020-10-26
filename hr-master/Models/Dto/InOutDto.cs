using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models.Dto
{
    public class InOutDto
    {
        public string  Empolyee_Name  { get; set; }

        public int InOut_status { get; set; }

        public DateTime  InOut_date  { get; set; }

        public string EmployeeInOut { get; set; }

        public int distance { get; set; }

        public string locationurl { get; set; }


        public string InOut_time { get; set; }


        public Guid Id { get; set; }


     
    }
}
