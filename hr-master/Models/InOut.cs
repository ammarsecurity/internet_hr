using Org.BouncyCastle.Asn1.Cms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models
{
    public class InOut
    {
        [Key]
        public Guid Id { get; set; }

        public Guid EmplyeeId { get; set; }

        public int In_Out { get; set; }

        public DateTime In_Out_Date { get; set; }

        public string In_Out_Time { get; set; }

        public int In_Out_Status { get; set; }

        public int distance { get; set; }

        public string Employee_Latitude { get; set; }

        public string Employee_Longitude { get; set; }


        [DefaultValue("false")]
        public bool IsDelete { get; set; }
    }
}
