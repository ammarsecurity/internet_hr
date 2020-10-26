using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models.Form
{
    public class NotificationsForm
    {
       
        public string contents { get; set; }


        public string headings { get; set; }

        public string url { get; set; }

        public string included_segments { get; set; }

        public string include_external_user_ids { get; set; }
    }
}
