using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models.Dto
{
    public class AttachmentDto
    {
        public Guid Id { get; set; }

        public string Attachment_Name { get; set; }

        public Guid Attachment_Id { get; set; }
    }
}
