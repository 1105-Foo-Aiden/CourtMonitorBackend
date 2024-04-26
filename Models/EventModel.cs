using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourtMonitorBackend.Models.DTO
{
    public class EventModel
    {
        [Key]
        public int EventID { get; set; }
        public string? Title { get; set; }
        public string? Date { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public string? Color { get; set; }
        public bool AllDay { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsPulished { get; set; }
        public int ProgramID { get; set; }
        public EventModel()
        {

        }
    }
}