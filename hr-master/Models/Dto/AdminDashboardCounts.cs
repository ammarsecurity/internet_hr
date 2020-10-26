using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models.Dto
{
    public class AdminDashboardCounts
    {

        public int Teamcount { get; set; }

        public int Employeescount { get; set; }

        public int thisdayabsence { get; set; }

        public int thismonthabsence { get; set; }


        public decimal SallerySum { get; set; }

        public decimal thismonthpenaltiesSun { get; set; }

        public decimal thismonthOverTimeReawradSum { get; set; }


        public decimal thismonthExpenses { get; set; }
        public decimal thismonthRevenues { get; set; }






    }
}
