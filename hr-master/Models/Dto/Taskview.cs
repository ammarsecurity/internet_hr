using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models.Dto
{
    public class Taskview
    {
       
        public List<TasksDto> AllTask { get; set; }

        public int totalRecords { get; set; }
    }
}
