using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models
{
    public class AccounterInputAndOutput
    {
        [Key]

        public Guid Id { get; set; }
        public decimal InputAndOutput_Price { get; set; }

        public Guid InputAndOutput_Door { get; set; }

        public DateTime InputAndOutput_Date { get; set; }

        public string InputAndOutput_Note { get; set; }


        public int InputAndOutput_Status { get; set; }


        [DefaultValue("false")]
        public bool IsDelete { get; set; }
    }
}
