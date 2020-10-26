using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models
{
    public class StoredParts
    {
        [Key]
        public Guid Id { get; set; }
        public string PartName { get; set; }
        public string PartNote { get; set; }
        [DefaultValue("false")]
        public bool IsDelete { get; set; }



    }
}
