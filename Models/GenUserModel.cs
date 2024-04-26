using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CourtMonitorBackend.Models.DTO;

namespace CourtMonitorBackend.Models
{
    public class GenUserModel
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public int? ProgramID { get; set; }

    }
}