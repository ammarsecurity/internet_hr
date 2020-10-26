using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models.Dto
{
    public class TowerElectricalDto
    {

        public Guid Id { get; set; }
        public Guid Tower_Id { get; set; }
        public string Electrical_Name { get; set; }
        public Guid Employee_Id { get; set; }
        public string Electrical_Type { get; set; }
        public string Electrical_SerailNamber { get; set; }
        public string Electrical_Date { get; set; }
    }
}
