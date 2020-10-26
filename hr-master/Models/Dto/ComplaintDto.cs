using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models.Dto
{
    public class ComplaintDto
    {
        public Guid Id { get; set; }

        public string Complain { get; set; }

        public Guid InternetUser_Id { get; set; }

        public string InternetUser_FullName { get; set; }

        public string  Tower_Name { get; set; }

        public DateTime Complain_Date { get; set; }

        public int Complain_Status { get; set; }


        public string User_Name { get; set; }

        public string User_Adress { get; set; }

        public string User_Phone { get; set; }

        public string User_Password { get; set; }

        public string User_Card { get; set; }

        public decimal User_Price { get; set; }

        public DateTime User_ActiveDate { get; set; }

        public DateTime User_EndDate { get; set; }

        public bool IsActive { get; set; }



    }
}
