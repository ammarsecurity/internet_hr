using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models
{
    public class Attachment
    {
        [Key]
        public Guid Id { get; set; }

        public string Attachment_Name { get; set; }

        public Guid Attachment_Id { get; set; }

        [DefaultValue("false")]
        public bool IsDelete { get; set; }


    }
}
