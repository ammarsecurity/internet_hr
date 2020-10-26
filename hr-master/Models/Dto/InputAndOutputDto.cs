using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models.Dto
{
    public class InputAndOutputDto
    {
        public Guid Id { get; set; }
        public decimal InputAndOutput_Price { get; set; }

        public string InputAndOutput_Door { get; set; }

        public DateTime InputAndOutput_Date { get; set; }

        public string InputAndOutput_Note { get; set; }

        public int InputAndOutput_Status { get; set; }
    }
}
