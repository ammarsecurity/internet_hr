using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models.Dto
{
    public class RewardsDto
    {
        public Guid Id { get; set; }

        public Guid Employees_Id { get; set; }

        public string Employees_Name { get; set; }
        public decimal Rewards_Price { get; set; }

        public DateTime Rewards_Date { get; set; }

        public string Rewards_Note { get; set; }


        public string Rewards_Enterid { get; set; }
    }
}
