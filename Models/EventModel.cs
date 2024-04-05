using Microsoft.EntityFrameworkCore;



namespace CourtMonitorBackend.Models.DTO
{       
    [Keyless]
    public class EventModel
    {
        public string Title {get; set;}
        public int Date {get; set;}
        public int StartTime {get; set;}
        public int EndTime {get; set;}
        public string Color { get; set; }

        public EventModel()
        {

        }
    }
}