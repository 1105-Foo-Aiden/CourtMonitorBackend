using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourtMonitorBackend.Models.DTO
{
    public class CreateAccountDTO
    {
        public int ID {get; set;}
        public string UserName {get; set;}
        public string Password {get; set;}
        public string FullName  {get; set;}
        public string Email {get; set;}
    }
}