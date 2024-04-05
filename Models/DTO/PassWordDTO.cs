namespace CourtMonitorBackend.Models.DTO
{
    public class PassWordDTO
    {
        public string? Salt {get; set;}
        public string? Hash {get; set;}
    }
}