using Org.BouncyCastle.Asn1.Cms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models
{
    public class Teams
    {
        [Key]
        public Guid Id { get; set; }
        public string Team_Name { get; set; }
        public string Team_Roles { get; set; }
        public string Team_Note { get; set; }
        public string In_Time { get; set; }
        public string Out_Time { get; set; }
        [DefaultValue("false")]
        public bool IsDelete { get; set; }


    }
}
