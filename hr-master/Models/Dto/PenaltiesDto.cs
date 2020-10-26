using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models.Dto
{
    public class PenaltiesDto
    {
        public Guid Id { get; set; }

        public Guid Employees_Id { get; set; }

        public string Employees_Name { get; set; }

        public decimal Penalties_Price { get; set; }

        public DateTime Penalties_Date { get; set; }

        public string Penalties_Note { get; set; }

        public string Penalties_Enterid { get; set; }

    }
}
