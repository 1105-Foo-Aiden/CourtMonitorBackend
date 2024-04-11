using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourtMonitorBackend.Models.DTO
{
    public class ProgramDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }    
        public int AdminID { get; set; }
    }
}