using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models.Dto
{
    public class RepalyTasksDto
    {
        public Guid Id { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeePhoto { get; set; }
        public string TaskName { get; set; }
        public string Repaly_Note { get; set; }
        public DateTime RepalyDate { get; set; }
        public Guid TaskId { get; set; }

        public List<RepalyAttachment> Attachmentlist { get; set; }
    }

    public class RepalyAttachment
    {
        public Guid Id { get; set; }

        public string Attachment_Name { get; set; }

        public Guid Attachment_Id { get; set; }

    }
}
