using System.ComponentModel.DataAnnotations.Schema;
using CourtMonitorBackend.Models.DTO;

namespace CourtMonitorBackend.Models
{
    public class CoachModel
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public int? ProgramID { get; set; }
    }
}