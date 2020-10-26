using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models.Form
{
    public class EditUserAdmin
    {
        public string User_Name { get; set; }
        public string User_Firstname { get; set; }
        public string User_Mail { get; set; }
        public string User_Phone { get; set; }
        public string User_Password { get; set; }
        public string User_Level { get; set; }
        public string Company_Longitude { get; set; }
        public string Company_Latitude { get; set; }
        public decimal Delay_penalty { get; set; }
    }
}
