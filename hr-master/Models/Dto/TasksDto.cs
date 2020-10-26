using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models.Dto
{
    public class TasksDto
    {
        public Guid Id { get; set; }

        public string Task_Title { get; set; }

        public Guid Task_Price_rewards { get; set; }

        public decimal Task_Price { get; set; }

        public string Task_part { get; set; }

        public Guid part_Id { get; set; }

        public string Task_Note { get; set; }

        public string Task_closed_Note { get; set; }

        public string Task_Employee_WorkOn { get; set; }

        public string Task_Employee_Open { get; set; }

        public string Task_Employee_Close { get; set; }

        public DateTime Task_Date { get; set; }

        public DateTime Task_EndDate { get; set; }

        public DateTime Task_Open { get; set; }

        public DateTime Task_Done { get; set; }

        public string Tower_Name { get; set; }

        public Guid Tower_Id { get; set; }

        public Guid InternetUserId { get; set; }

        public int Task_Status { get; set; }

        public Guid Task_Price_id { get; set; }
    }
}
