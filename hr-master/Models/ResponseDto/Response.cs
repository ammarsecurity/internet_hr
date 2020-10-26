using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Models.ResponseDto
{
    public class Response
    {
        public bool Error { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
