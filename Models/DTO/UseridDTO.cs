namespace CourtMonitorBackend.Models.DTO
{
    public class UseridDTO
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Programs { get; set; }
        public string? FunFact { get; set; }
        public string? Birthday { get; set; }
        public string? RealName { get; set; }  
        public bool IsAdmin { get; set; }
        public bool IsCoach { get; set; }
        public bool IsUser { get; set; }
    }
}