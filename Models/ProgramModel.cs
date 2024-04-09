using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourtMonitorBackend.Models.DTO
{
    public class ProgramModel
    {
        [Key]
        public int ProgramID { get; set; }

        [ForeignKey("Admin")]
        public int AdminID { get; set; }
        [ForeignKey("Coach")]
        public int CoachID { get; set; }
        
        [ForeignKey("GenUser")]
        public int GenUserID { get; set; }
        public string? ProgramName { get; set; }
        public string? ProgramSport { get; set; }
        public int EventID { get; set; }
        public AdminModel? Admin { get; set; }
        public CoachModel? Coach { get; set; }
        public GenUserModel? GenUser { get; set; }
        public EventModel? Event { get; set; }
    }
}