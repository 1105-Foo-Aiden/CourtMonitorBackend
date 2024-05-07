using System.ComponentModel.DataAnnotations;

namespace CourtMonitorBackend.Models.DTO{
    public class EventModel{
        [Key]
        public int id { get; set; }
        public string Title { get; set; }
        public string Start { get; set; }
        public string? End { get; set; }
        public string Color { get; set; }
        public bool AllDay { get; set; }
        public string ProgramID { get; set; }

        public EventModel(){

        }
    }
}