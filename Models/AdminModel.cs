using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourtMonitorBackend.Models.DTO
{
    public class AdminModel
    {
        [ForeignKey("User")]
        public int UserID { get; set; } 

        [ForeignKey("Program")]
        public string? ProgramID { get; set; }

        public UserModel? Uesr {get; set;}
        public ProgramModel? Program { get; set;}
    }
}