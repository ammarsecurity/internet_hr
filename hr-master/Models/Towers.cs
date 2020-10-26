using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models
{
    public class Towers
    {
        [Key]
        public Guid Id { get; set; }
        public string Tower_Name { get; set; }
        public string Tower_Ip{ get; set; }
        public string Tower_locition { get; set; }
        public string Tower_Owner { get; set; }
        public string Tower_Owner_Number { get; set; }
        public string Tower_Note { get; set; }
        [DefaultValue("false")]
        public bool IsDelete { get; set; }




    }
}
