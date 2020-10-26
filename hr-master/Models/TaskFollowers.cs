using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models
{
    public class TaskFollowers
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid TaskId { get; set; }

    }
}
