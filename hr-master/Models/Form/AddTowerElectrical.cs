using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models.Form
{
    public class AddTowerElectrical
    {
        public Guid Tower_Id { get; set; }
        public string Electrical_Name { get; set; }
        public Guid Employee_Id { get; set; }
        public string Electrical_Type { get; set; }
        public string Electrical_SerailNamber { get; set; }
        public DateTime Electrical_Date { get; set; }

    }
}
