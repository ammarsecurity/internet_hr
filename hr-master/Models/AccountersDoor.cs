using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models
{
    public class AccountersDoor
    {

        [Key]
        public Guid Id { get; set; }

        public string  AccounDoor_Name { get; set; }


        public int AccounDoor_Status { get; set; }


        public string AccounDoor_Info { get; set; }


        [DefaultValue("false")]
        public bool IsDelete { get; set; }

    }
}
