namespace CourtMonitorBackend.Models.DTO{
    public class AdminModel
    {
        public int Id { get; set; }
        public int UserID { get; set; } 
        public List<int>? ProgramID { get; set; }
        AdminModel(){
            ProgramID = new List<int>();
        }
    }
}