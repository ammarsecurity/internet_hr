using Org.BouncyCastle.Asn1.Cms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models.Form
{
    public class AddTeam
    {
        public string Team_Name { get; set; }
        public string Team_Roles { get; set; }
        public string Team_Note { get; set; }
        public string In_Time { get; set; }
        public string Out_Time { get; set; }
    }
}
