using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CourtMonitorBackend.Models.DTO;

namespace CourtMonitorBackend.Models
{
    public class GenUserModel
    {
        [Key]
        public int? Id { get; set; } 
        
        [ForeignKey("User")]
        public int? UserID { get; set; }

        [ForeignKey("Program")]
        public int? ProgramID { get; set; }
        public UserModel? User {get; set;}
      
    }
}