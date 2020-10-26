using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models.Form
{
    public class AddMovements
    {
        public Guid ItemId { get; set; }

        public int Movement_type { get; set; }

        public int Movement_Count { get; set; }

        public string Movement_Note { get; set; }

        public string Movement_Received { get; set; }
    }
}
