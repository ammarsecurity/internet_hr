using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models
{
    public class TasksRepalys
    {
        [Key]
        public Guid Id { get; set; }

        public Guid EmployeeId { get; set; }
        public Guid TaskId { get; set; }
        public string Repaly_Note { get; set; }

        public DateTime RepalyDate { get; set; }

        [DefaultValue("false")]
        public bool IsDelete { get; set; }
    }
}
