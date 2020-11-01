using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models.Dto
{
    public class NotificationHubDto
    {
        public string articleHeading { get; set; }
        public string articleContent { get; set; }
        public string userId { get; set; }
    }
}
