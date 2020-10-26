using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models.Form
{
    public class EditEmployee
    {
        public string Employee_Name { get; set; }
        public string Employee_Fullname { get; set; }
        public string Employee_Email { get; set; }
        public string Employee_Phone { get; set; }
        public string Employee_Password { get; set; }
        public Guid Employee_Team { get; set; }
        public string Employee_Adress { get; set; }
        public decimal Employee_Saller { get; set; }
        public string Employee_Note { get; set; }

        public string Employee_In_Time { get; set; }
        public string Employee_Out_Time { get; set; }

        public DateTime Registration_Data { get; set; }
        public IFormFile Employee_image { get; set; }
    }
}
