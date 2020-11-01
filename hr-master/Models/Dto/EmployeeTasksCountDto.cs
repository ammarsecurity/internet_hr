using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models.Dto
{
    public class EmployeeTasksCountDto
    {
        public string Employee_FullName { get; set; }

        public int AddTaskMonth { get; set; }
        public int AddTaskToday { get; set; }

        public int FollowerTaskMonth { get; set; }
        public int DoneTaskMonth { get; set; }
        public int FollowerTaskToday{ get; set; }
        public int DoneTaskToday { get; set; }
    }
}
