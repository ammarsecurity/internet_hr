using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models.Form
{
    public class AddTowerBroadcasting
    {
        public Guid Tower_Id { get; set; }
        public string Broadcasting_SSID { get; set; }
        public Guid Employee_Id { get; set; }
        public string Broadcasting_Type { get; set; }
        public string Broadcasting_UserName { get; set; }
        public string Broadcasting_Password { get; set; }
        public string Broadcasting_SerailNamber { get; set; }
        public DateTime Broadcasting_Time { get; set; }
        public string Broadcasting_Ip { get; set; }

    }
}
