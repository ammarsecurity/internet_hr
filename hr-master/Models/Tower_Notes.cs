using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models
{
    public class Tower_Notes
    {
        public Guid Id { get; set; }
        public Guid Tower_Id { get; set; }
        public string Notes { get; set; }
        public DateTime Notes_Date { get; set; }
    }
}
