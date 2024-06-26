using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourtMonitorBackend.Models.DTO{
    public class ProgramModel{
        [Key]
        public int ProgramID { get; set; }
        public string AdminID { get; set; }
        public string? CoachID { get; set; }
        public string? GenUserID { get; set; }
        public string ProgramName { get; set; }
        public string ProgramSport { get; set; }
        public string? Description { get; set; }
        public ProgramModel(){

        }
    }
}