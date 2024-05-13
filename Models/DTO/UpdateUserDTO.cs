using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourtMonitorBackend.Models.DTO
{
    public class UpdateUserDTO
    {
        public string UserName {get; set;} 
        public string RealName {get; set;}
        public string? Image {get; set;}
        public string? Birthday {get; set;}
        public string? FunFact {get; set;}
        public string Email {get; set;}
    }
}