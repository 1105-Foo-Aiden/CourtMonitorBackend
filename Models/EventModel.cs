using System.ComponentModel.DataAnnotations.Schema;

namespace CourtMonitorBackend.Models.DTO
{
    public class EventModel
    {
        public int EventID { get; set; }
        [ForeignKey("User")]
        public string UserID { get; set; }
        public string Title { get; set; }
        public string Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Color { get; set; }
        public string? Sport { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsPulished { get; set; }
        [ForeignKey("Program")]
        public int ProgramID { get; set; }
        public ProgramModel Program { get; set; }
        public UserModel User { get; set; }
        public EventModel()
        {

        }
    }
}