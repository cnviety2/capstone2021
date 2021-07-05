using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone2021.DTO
{
    /**
     * Class bao bên ngoài response trả về
     */
    public class ResponseDTO
    {
        public string message { get; set; }
        public Object data { get; set; }
    }
}