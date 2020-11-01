using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models.Form
{
    public class AddTaskes
    {

        public string Task_Title { get; set; }

        public Guid Task_Price_rewards { get; set; }

        public Guid Task_part { get; set; }

        public Guid Task_Employee_WorkOn { get; set; }

        public string Task_Note { get; set; }

        public DateTime Task_Date { get; set; }

        public DateTime Task_EndDate { get; set; }

        public Guid Tower_Id { get; set; }
        public Guid InternetUserId { get; set; }




    }
}
