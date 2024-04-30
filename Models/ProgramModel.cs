using System.ComponentModel.DataAnnotations;

namespace CourtMonitorBackend.Models.DTO{
    public class ProgramModel{
        [Key]
        public int ProgramID { get; set; }
        public int AdminID { get; set; }
        public int? CoachID { get; set; }
        public int? GenUserID { get; set; }
        public string ProgramName { get; set; }
        public string ProgramSport { get; set; }
        public int? EventID { get; set; }
        public ProgramModel(){
            
        }
    }
}