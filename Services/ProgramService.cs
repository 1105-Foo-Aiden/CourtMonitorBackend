using CourtMonitorBackend.Models;
using CourtMonitorBackend.Models.DTO;
using CourtMonitorBackend.Services.Context;

namespace CourtMonitorBackend.Services{
    public class ProgramService{
        private readonly DataContext _context;
        public ProgramService(DataContext context){
            _context = context;
        }

        public bool DoesProgramExist(string Program){
            return _context.ProgramInfo.SingleOrDefault(name => name.ProgramName == Program) != null;
        }

        public string CreateProgram(ProgramDTO NewProgram){
            //Create a blank template of a program using the DTO to fill out the required information
            //Then take the user from the DTO and add the Program Name to the account under the "Programs" Data point
            //Then Create a new Admin in the Admin Table with the ID of the program instead of the name
            //Save the database after each step to ensure that each new data point is saved.
            try{
                ProgramModel ProgramToAdd = new(){
                    ProgramName = NewProgram.ProgramName,
                    ProgramSport = NewProgram.ProgramSport,
                    Discription = NewProgram.Description,
                    AdminID = NewProgram.AdminID,
                };
                if(DoesProgramExist(NewProgram.ProgramName)){
                    return "Program already exists";
                }
                _context.ProgramInfo.Add(ProgramToAdd);
                _context.SaveChanges();
                UserModel User = _context.UserInfo.SingleOrDefault(u => u.ID == NewProgram.AdminID);
                if(User !=null){
                    if(string.IsNullOrEmpty(User.Programs)){
                        User.Programs = NewProgram.ProgramName;
                    }
                    else{
                        User.Programs += "," + NewProgram.ProgramName;
                    }
                    _context.UserInfo.Update(User);
                }
                else{
                    return "User not found";
                }
                AdminModel admin = new(){
                    UserID = NewProgram.AdminID,
                    ProgramID = ProgramToAdd.ProgramID,
                };
                _context.AdminInfo.Add(admin);
                _context.SaveChanges(); 
                return "Success";
            }catch(Exception ex){
                return ex.InnerException.ToString();
            }
        }    
        public UserModel GetAdminById(int id){
            var foundAdmin =  _context.AdminInfo.SingleOrDefault(user => user.Id == id);
            return _context.UserInfo.SingleOrDefault(user => user.ID == foundAdmin.UserID);
        }

        public IEnumerable<ProgramModel> GetAllPrograms(){
            return _context.ProgramInfo;
        }

        public ProgramModel GetProgramById(int ProgramId){
            return _context.ProgramInfo.SingleOrDefault(p => p.ProgramID == ProgramId);
        }

        public IEnumerable<EventModel> GetAllEventsByProgramID(int programId){
            var foundProgram = _context.ProgramInfo.FirstOrDefault(e => e.ProgramID == programId);
            return _context.EventInfo.Where(e => e.ProgramID == foundProgram.ProgramID);
        }
        
        public bool DeleteProgram(string program){
            bool result = false;
            var foundProgram = _context.ProgramInfo.SingleOrDefault(ProgramToDelete => ProgramToDelete.ProgramName == program);
            if(foundProgram != null){
                var foundEvents = GetAllEventsByProgramID(foundProgram.ProgramID);
                _context.Remove(foundEvents);
                _context.Remove(foundProgram);
                result = _context.SaveChanges() != 0;
            }

            return result;
        }
        public IEnumerable<ProgramModel> GetProgramsBySport(string sport){
            return _context.ProgramInfo.Where(p => p.ProgramSport.ToLower() == sport.ToLower());
        } 

        public string AddUserToProgram(AddUserToProgramDTO newProgramUser){
            var foundProgram = _context.ProgramInfo.SingleOrDefault(program => program.ProgramID == newProgramUser.ProgramID);
            if(foundProgram != null){
                if(newProgramUser.Status.ToLower() == "genuser" || newProgramUser.Status.ToLower() == "general" ){
                    ProgramModel programModel = new(){
                        GenUserID = newProgramUser.UserId,
                        ProgramID = newProgramUser.ProgramID
                    };
                    _context.ProgramInfo.Add(programModel);
                }
                else if(newProgramUser.Status.ToLower() == "coach"){
                    ProgramModel programModel = new(){
                        CoachID = newProgramUser.UserId,
                        ProgramID = newProgramUser.ProgramID
                    };
                    _context.ProgramInfo.Add(programModel);
                }
                else if(newProgramUser.Status.ToLower() == "admin"){
                    ProgramModel programModel = new(){
                        AdminID = newProgramUser.UserId,
                        ProgramID = newProgramUser.ProgramID
                    };
                    _context.ProgramInfo.Add(programModel);
                }
                else{
                    return "Status is not valid, please enter 'genuser', 'admin', or 'coach'. ";
                }
                try{
                    _context.SaveChanges();
                    return "Saved Successfully.";
                }
                catch(Exception ex){ 
                    return ex.Message;
                }
            }
            else{
                return "This Program Doesn't exist, please try again.";
            }
        }
    }
}
