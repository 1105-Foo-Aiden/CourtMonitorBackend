using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourtMonitorBackend.Models.DTO
{
    public class EmailModel
    {
        public string Sender { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}