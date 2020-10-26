using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models.ResponseDto
{
    public class AuthResponse
    {
        public bool Error { get; set; }
        public string external_id { get; set; }
        public string Tower { get; set; }
        public string Role { get; set; }
        public object Data { get; set; }

    }
}
