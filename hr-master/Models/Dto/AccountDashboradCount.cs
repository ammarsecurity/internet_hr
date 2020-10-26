using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models.Dto
{
    public class AccountDashboradCount
    {

        public int Employeescount { get; set; }
        public decimal SallerySum { get; set; }

        public decimal thismonthpenaltiesSun { get; set; }

        public decimal thismonthOverTimeReawradSum { get; set; }
        public decimal thismonthExpenses { get; set; }
        public decimal thismonthRevenues { get; set; }
        public decimal ExpensesToday { get; set; }
        public decimal RevenuesToday { get; set; }
    }
}
