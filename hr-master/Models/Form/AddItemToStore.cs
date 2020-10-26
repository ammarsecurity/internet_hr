using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models.Form
{
    public class AddItemToStore
    {
        public string Item_Name { get; set; }
        public string Item_Model { get; set; }
        public bool Item_IsUsed { get; set; }
        public string Item_SerialNumber { get; set; }
        public int Item_Count { get; set; }
        public DateTime Item_EnteryDate { get; set; }
        public Guid Item_EmployeeEntery { get; set; }
        public Guid Item_Part { get; set; }

    }
}
