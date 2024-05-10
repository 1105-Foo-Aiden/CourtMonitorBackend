namespace CourtMonitorBackend.Models.DTO{
    public class ProgramDTO{
        public int ID { get; set; }
        public string ProgramName { get; set; }    
        public string ProgramSport { get; set; } 
        public string? Description { get; set; }
        public string AdminID { get; set; }
    }
}