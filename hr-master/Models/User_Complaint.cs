using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace hr_master.Models
{
    public class User_Complaint
    {
        [Key]
        public Guid Id { get; set; }

        public string Complain { get; set; }

        public Guid InternetUser_Id { get; set; }


        public DateTime Complain_Date { get; set; }

        public int Complain_Status { get; set; }

        [DefaultValue("false")]
        public bool IsDelete { get; set; }
    }
}
