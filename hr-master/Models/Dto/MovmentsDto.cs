using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models.Dto
{
    public class MovmentsDto
    {

        public Guid Id { get; set; }
        public string Movment_Employee { get; set; }


        public Guid ItemId { get; set; }

        public string Item_Name { get; set; }

        public DateTime Movement_date { get; set; }

        public int Movement_type { get; set; }

        public int Movement_Count { get; set; }

        public string Movement_Note { get; set; }

        public string Movement_Received { get; set; }
    }
}
