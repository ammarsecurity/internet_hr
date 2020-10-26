using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models.Dto
{
    public class InternetUserDto
    {
        public Guid Id { get; set; }

        public string User_FullName { get; set; }

        public string User_Name { get; set; }

        public string User_Password { get; set; }

        public string User_Card { get; set; }

        public string User_Phone { get; set; }

        public string User_Adress { get; set; }

        public string User_Tower { get; set; }
        public string User_Price { get; set; }

        public DateTime User_ActiveDate { get; set; }

        public DateTime User_EndDate { get; set; }

        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
    }
}
