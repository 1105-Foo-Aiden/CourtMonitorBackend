namespace CourtMonitorBackend.Models
{
    public class CoachModel
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public List<int>? ProgramID { get; set; }
        CoachModel(){
            ProgramID = new List<int>();
        }
    }
}