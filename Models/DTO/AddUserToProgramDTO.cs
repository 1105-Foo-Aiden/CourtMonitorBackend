namespace CourtMonitorBackend.Models.DTO{
    public class AddUserToProgramDTO{
        public int ProgramID {get; set;}
        public int UserId {get; set;}
        public string Status {get; set;}
    }
}