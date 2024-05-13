namespace CourtMonitorBackend.Models{
    public class UserModel{
        public int ID {get; set;}
        public string UserName {get; set;} 
        public string RealName {get; set;}
        public string? Image {get; set;}
        public string? Birthday {get; set;}
        public string? Programs {get; set;}
        public string? FunFact {get; set;}
        public string Email {get; set;}
        public string? Salt {get; set;}
        public string? Hash {get; set;}
        public UserModel(){
         
        }
    }
}