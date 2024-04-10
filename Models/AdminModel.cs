using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourtMonitorBackend.Models.DTO
{
    public class AdminModel
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public int? UserID { get; set; } 

        [ForeignKey("Program")]
        public int? ProgramID { get; set; }
        public UserModel? User {get; set;}
        public ProgramModel? Program { get; set;}
    }
}