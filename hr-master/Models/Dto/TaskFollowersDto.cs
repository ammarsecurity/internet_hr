using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models.Dto
{
    public class TaskFollowersDto
    {
        public Guid Id { get; set; }
        public string Employee_FullName { get; set; }

        public string Employee_Photo { get; set; }
    }
}
