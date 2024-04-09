using System.ComponentModel.DataAnnotations.Schema;
using CourtMonitorBackend.Models.DTO;

namespace CourtMonitorBackend.Models
{
    public class CoachModel
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserID { get; set; }
        
        [ForeignKey("Program")]
        public int ProgramID { get; set; }

        public UserModel? User { get; set; }
        public ProgramModel? Program { get; set; }
    }
}