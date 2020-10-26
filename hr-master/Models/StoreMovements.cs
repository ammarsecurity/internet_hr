using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models
{
    public class StoreMovements
    {
        [Key]
        public Guid Id { get; set; }

        public Guid Movment_Employee { get; set; }

        public Guid ItemId { get; set; }

        public DateTime Movement_date { get; set; }

        public int Movement_type { get; set; }

        public int Movement_Count { get; set; }

        public string Movement_Note { get; set; }


        public string Movement_Received { get; set; }

        [DefaultValue("false")]
        public bool IsDelete { get; set; }
    }


}
