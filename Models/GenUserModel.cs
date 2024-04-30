namespace CourtMonitorBackend.Models{
    public class GenUserModel
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public List<int>? ProgramID { get; set; }
        public GenUserModel()
        {
            ProgramID = new List<int>();
        }

    }
}