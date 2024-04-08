using Microsoft.EntityFrameworkCore;



namespace CourtMonitorBackend.Models.DTO
{       
    [Keyless]
    public class EventModel
    {
        public string Title {get; set;}
        public string Date {get; set;}
        public string StartTime {get; set;}
        public string EndTime {get; set;}
        public string Color { get; set; }
        public string? Tags { get; set; }

        public EventModel()
        {
            
        }
    }
}