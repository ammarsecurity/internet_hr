using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models.Dto
{
    public class TowerBroadcastingDto
    {
        public Guid Id { get; set; }
        public Guid Tower_Id { get; set; }
        public string Broadcasting_SSID { get; set; }
        public Guid Employee_Id { get; set; }
        public string Broadcasting_Type { get; set; }
        public string Broadcasting_UserName { get; set; }
        public string Broadcasting_Password { get; set; }
        public string Broadcasting_SerailNamber { get; set; }
        public string Broadcasting_Time { get; set; }
        public string Broadcasting_Ip { get; set; }
    }
}
