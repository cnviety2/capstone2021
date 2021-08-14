using System;

namespace Capstone2021.DTO
{
    public class SendMailToRetrievePasswordDTO
    {
        public int code { get; set; }
        public String randomString { get; set; }
    }
}