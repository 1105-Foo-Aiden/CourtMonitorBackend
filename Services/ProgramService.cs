using System.Net;
using System.Reflection;
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
        public bool DoesProgramIDExist(int programId){
            return _context.ProgramInfo.SingleOrDefault(id => id.ProgramID == programId) != null;
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
                    Description = NewProgram.Description,
                    AdminID = NewProgram.AdminID,
                };
                if(DoesProgramExist(NewProgram.ProgramName)){
                    return "Program already exists";
                }
                _context.ProgramInfo.Add(ProgramToAdd);
                _context.SaveChanges();
                int AdminIdNumber = int.Parse(NewProgram.AdminID);
                UserModel User = _context.UserInfo.SingleOrDefault(u => u.ID == AdminIdNumber);
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
                    UserID = AdminIdNumber,
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
                _context.Remove(foundProgram);
                result = _context.SaveChanges() != 0;
            }
            return result;
        }
        public IEnumerable<ProgramModel> GetProgramsBySport(string sport){
            return _context.ProgramInfo.Where(p => p.ProgramSport.ToLower() == sport.ToLower());
        } 

        public ProgramModel GetProgramByProgramName(string ProgramName){
            return _context.ProgramInfo.FirstOrDefault(p => p.ProgramName == ProgramName);
        }

        public string AddUserToProgram(AddUserToProgramDTO newProgramUser){
            //Similar to the Create Program, I want to add A user to an existing Program
            //First, I need to find the program
            //Second, I use the User Id from the DTO to add it to whichever level in the Program based on the "Status"
            //Then I add the Program Name to the User's Progam Point, will use the same logic as before
            try{
                ProgramModel program = _context.ProgramInfo.SingleOrDefault(p => p.ProgramID == newProgramUser.ProgramID);
                if(program == null){
                    return "Program Not Found, Try again";
                }
                switch(newProgramUser.Status.ToLower()){
                    case "genuser":
                    if(string.IsNullOrEmpty(program.GenUserID)){
                        program.GenUserID = newProgramUser.UserId.ToString() + ",";
                    }
                    else{
                        program.GenUserID += newProgramUser.UserId.ToString() + ",";
                    }
                    break;
                    case "general":
                    if(string.IsNullOrEmpty(program.GenUserID)){
                        program.GenUserID = newProgramUser.UserId.ToString() + ",";
                    }
                    else{
                        program.GenUserID += newProgramUser.UserId.ToString() + ",";
                    }
                    break;
                    case "coach":
                    if(string.IsNullOrEmpty(program.CoachID)){
                        program.CoachID = newProgramUser.UserId.ToString() + ",";
                    }
                    else{
                        program.CoachID += newProgramUser.UserId.ToString() + ",";
                    }
                    break;
                    case "admin":
                    if(string.IsNullOrEmpty(program.AdminID)){
                        program.AdminID = newProgramUser.UserId.ToString() + ",";
                    }
                    else{
                        program.AdminID += newProgramUser.UserId.ToString() + ",";
                    }
                    break;
                    default:
                    return "Invalid Status";
                }
                _context.ProgramInfo.Update(program);
                _context.SaveChanges();
                UserModel userToAdd = _context.UserInfo.SingleOrDefault(u => u.ID == newProgramUser.UserId);
                if(userToAdd == null){
                    return "Cannot find user to add";
                }
                if(string.IsNullOrEmpty(userToAdd.Programs)){
                    userToAdd.Programs = program.ProgramName;
                }
                else{
                    userToAdd.Programs += program.ProgramName + ",";
                }
                _context.UserInfo.Update(userToAdd);
                _context.SaveChanges();
                return "Success";
            }catch(Exception ex){
                return ex.Message;
            }
        }
        public UserModel GetUserByID(int id){
            return _context.UserInfo.SingleOrDefault(u => u.ID == id);
        }
        public class UserData {
                public string UserName { get; set; }
                public string RealName { get; set; }
        }
        public class ProgramUsers{
            public List<UserData> Admins {get; set;}
            public List<UserData> Coaches {get; set;}
            public List<UserData> General {get; set;}
        }
        public object GetUsernameByProgram(string ProgramName){
            //create an object, the object will have 3 object arrays in it, Admin, Coach, and General
            //for each object in the list, there will be arrays, 
            //in each array, there will be the the UserName and the Real Name 
            //if the section in the Program is Empty, then we'll skip over that section or return nothing
            //Each user will be obtained through GetUesrByID function above user ID parsed from each Program's section under the same name
            ProgramModel foundProgram = _context.ProgramInfo.SingleOrDefault(p => p.ProgramName == ProgramName);
            ProgramUsers programUsers = new(){
                Admins = new List<UserData>(),
                Coaches = new List<UserData>(),
                General = new List<UserData>(),
            };

            if(foundProgram != null){
                List<(string, string)> Admins = new();
                List<(string, string)> Coaches = new();
                List<(string, string)> GenUsers = new();
                if(!string.IsNullOrEmpty(foundProgram.AdminID)){
                    string[] AdminIds = foundProgram.AdminID.Split(",");
                    foreach(string Id in AdminIds){
                        int ID = int.Parse(Id);
                        UserModel foundUser = GetUserByID(ID);
                        if(foundUser != null){
                            programUsers.Admins.Add(new UserData{RealName = foundUser.RealName, UserName = foundUser.UserName});
                        }
                    }
                }

                if(!string.IsNullOrEmpty(foundProgram.CoachID)){
                    string[] CoachIDs = foundProgram.CoachID.Split(",");
                    foreach(string Id in CoachIDs){
                        int ID = int.Parse(Id);
                        UserModel foundUser = GetUserByID(ID);
                        if(foundUser != null){
                            programUsers.Coaches.Add(new UserData{RealName = foundUser.RealName, UserName = foundUser.UserName});
                        }
                    }
                }

                if(!string.IsNullOrEmpty(foundProgram.GenUserID)){
                    string[] GenUserIDs = foundProgram.GenUserID.Split(",");
                    foreach(string Id in GenUserIDs){
                        int ID = int.Parse(Id);
                        UserModel foundUser = GetUserByID(ID);
                        if(foundUser != null){
                            programUsers.General.Add(new UserData{RealName = foundUser.RealName, UserName = foundUser.UserName});
                        }
                    }
                }
            }
            return programUsers;
        }

        // public IEnumerable<UserModel> GetUsersByProgramId(int ID){
        //     //Validate Program's existance through Getting the program by ID
        //     try{

        //     var ProgramToGet = GetProgramById(ID);
        //     //if the Program Exists, Go through Each Transer table to get the User Id's assigned to the Program ID
        //     //do this three times for Coach, Admin, and GeneralUsers
        //     if(ProgramToGet != null){
        //         var Admins = _context.AdminInfo.Where(p => p.ProgramID == ID).Select(admin => admin.UserID);
        //         var AdminUsers = from user in _context.AdminInfo
        //                         where Admins.Contains(user.Id)
        //                         select user;

        //         var Coaches = _context.CoachInfo.Where(p => p.ProgramID == ID).Select(coach => coach.UserID);
        //         var CoachUsers = from user in _context.UserInfo 
        //                         where Coaches.Contains(user.ID) 
        //                         select user;
                
        //         var GenUsers = _context.GenUserInfo.Where(p => p.ProgramID == ID).Select(GenUser => GenUser.UserID);
        //         var generalUsers = from user in _context.UserInfo
        //                             where GenUsers.Contains(user.ID)
        //                             select user;
        //         return generalUsers;
        //     }

        //     }catch (Exception ex){
        //         throw new Exception(ex.Message);
        //     }
        // }
            
            // var foundProgram = _context.ProgramInfo.SingleOrDefault(program => program.ProgramID == newProgramUser.ProgramID);
            // if(foundProgram != null){
            //     if(newProgramUser.Status.ToLower() == "genuser" || newProgramUser.Status.ToLower() == "general" ){
            //         ProgramModel programModel = new(){
            //             GenUserID = newProgramUser.UserId,
            //             ProgramID = newProgramUser.ProgramID
            //         };
            //         _context.ProgramInfo.Add(programModel);
            //     }
            //     else if(newProgramUser.Status.ToLower() == "coach"){
            //         ProgramModel programModel = new(){
            //             CoachID = newProgramUser.UserId,
            //             ProgramID = newProgramUser.ProgramID
            //         };
            //         _context.ProgramInfo.Add(programModel);
            //     }
            //     else if(newProgramUser.Status.ToLower() == "admin"){
            //         ProgramModel programModel = new(){
            //             AdminID = newProgramUser.UserId,
            //             ProgramID = newProgramUser.ProgramID
            //         };
            //         _context.ProgramInfo.Add(programModel);
            //     }
            //     else{
            //         return "Status is not valid, please enter 'genuser', 'admin', or 'coach'. ";
            //     }
            //     try{
            //         _context.SaveChanges();
            //         return "Saved Successfully.";
            //     }
            //     catch(Exception ex){ 
            //         return ex.Message;
            //     }
            // }
            // else{
            //     return "This Program Doesn't exist, please try again.";
            // }
    }
}
