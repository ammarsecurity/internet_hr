﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models
{
    public class Tasks
    {
        [Key]
        public Guid Id { get; set; }

        public string Task_Title { get; set; }

        public Guid Task_Price_rewards { get; set; }

        public Guid Task_part { get; set; }
        public Guid Task_Open_Part { get; set; }

        public string Task_Note { get; set; }

        public string Task_closed_Note { get; set; }

        public Guid Task_Employee_WorkOn { get; set; }

        public Guid Task_Employee_Open { get; set; }

        public Guid Task_Employee_Close { get; set; }

        public DateTime Task_Date { get; set; }

        public DateTime Task_EndDate { get; set; }

        public DateTime Task_Open { get; set; }

        public DateTime Task_Done { get; set; }

        public Guid Tower_Id { get; set; }
        public Guid InternetUserId { get; set; }

        public int Task_Status { get; set; }


        [DefaultValue("false")]
        public bool IsDelete { get; set; }


    }
}
