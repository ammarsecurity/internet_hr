using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models.Dto
{
    public class ActiveInternetUser
    {
        public bool IsActive { get; set; }

        public DateTime User_ActiveDate { get; set; }

        public DateTime User_EndDate { get; set; }

        public string User_Card { get; set; }

        public decimal User_Price { get; set; }


    }
}
