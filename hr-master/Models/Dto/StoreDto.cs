using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models.Dto
{
    public class StoreDto
    {
        public string Item_Name { get; set; }
        public string Item_Model { get; set; }
        public bool Item_IsUsed { get; set; }
        public string Item_SerialNumber { get; set; }
        public int Item_Count { get; set; }
        public Guid Item_Part { get; set; }

    }
}
