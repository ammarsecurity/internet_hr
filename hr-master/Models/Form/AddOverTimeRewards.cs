﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models.Form
{
    public class AddOverTimeRewards
    {
        public Guid Employees_Id { get; set; }

        public decimal OverTimeRewards_Price { get; set; }

        public DateTime OverTimeRewards_Date { get; set; }

        public string OverTimeRewards_Note { get; set; }


        public Guid OverTimeRewards_Enterid { get; set; }

    }
}
