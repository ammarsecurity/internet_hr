using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models
{
    public class RewardsTable
    {
        [Key]
        public Guid Id { get; set; }

        public string RewardsInfo { get; set; }

        public decimal RewardsPrice { get; set; }

        [DefaultValue("false")]
        public bool IsDelete { get; set; }
    }
}
