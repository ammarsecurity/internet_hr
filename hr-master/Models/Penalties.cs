using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models
{
    public class Penalties
    {

        [Key]
        public Guid Id { get; set; }

        public Guid Employees_Id { get; set; }

        public decimal Penalties_Price { get; set; }

        public DateTime Penalties_Date { get; set; }

        public string Penalties_Note { get; set; }


        public Guid Penalties_Enterid { get; set; }


        [DefaultValue("false")]
        public bool IsDelete { get; set; }


    }
}
