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
                    ProgramName = NewProgram.ProgramName.Trim(),
                    ProgramSport = NewProgram.ProgramSport,
                    Description = NewProgram.Description,
                    AdminID = NewProgram.AdminID + ","
                };
                
                if(DoesProgramExist(NewProgram.ProgramName.Trim())){
                    return "Program already exists";
                }
                _context.ProgramInfo.Add(ProgramToAdd);
                _context.SaveChanges();
                int AdminIdNumber = int.Parse(NewProgram.AdminID);
                UserModel User = _context.UserInfo.SingleOrDefault(u => u.ID == AdminIdNumber);
                if(User !=null){
                    if(string.IsNullOrEmpty(User.Programs)){
                        User.Programs = ProgramToAdd.ProgramName + ",";
                    }
                    else{
                        User.Programs +=  ProgramToAdd.ProgramName + ",";
                    }
                    _context.UserInfo.Update(User);
                }
                else return "User not found";
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
        
        public UserModel GetUserByUsername(string Username){
            return _context.UserInfo.SingleOrDefault(u => u.UserName == Username);
        }
        public bool DeleteProgram(string program){
            bool result = false;
            var foundProgram = _context.ProgramInfo.SingleOrDefault(ProgramToDelete => ProgramToDelete.ProgramName == program);
            if(foundProgram != null){
                //get all the users from the found program and remove the name from their Programs list.
                var AllUsers = GetUsernameByProgram(foundProgram.ProgramName);
                foreach(var user in AllUsers.Item1){
                    UserModel foundUser = GetUserByUsername(user.UserName);
                    if(!string.IsNullOrEmpty(foundUser.Programs)){
                        string[] Programs = foundUser.Programs.Split(",");
                        Programs = Programs.Where(p => p != foundProgram.ProgramName).ToArray();
                        foundUser.Programs = string.Join(",", Programs);
                    }
                }
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
                UserModel userToAdd = _context.UserInfo.SingleOrDefault(u => u.ID == newProgramUser.UserId);
                if(program == null){
                    return "Program Not Found, Try again";
                }
                if(userToAdd == null){
                    return "Cannot find user to add";
                }
                if(!string.IsNullOrEmpty(program.GenUserID) && program.GenUserID.Split(",").Contains(newProgramUser.UserId.ToString()) || !string.IsNullOrEmpty(program.CoachID) && program.CoachID.Split(",").Contains(newProgramUser.UserId.ToString()) || program.AdminID.Split(",").Contains(newProgramUser.UserId.ToString())){
                    return "User is already a part of the program";
                }
                switch(newProgramUser.Status.ToLower()){
                    case "genuser":
                    if(string.IsNullOrEmpty(program.GenUserID)){
                        program.GenUserID = newProgramUser.UserId.ToString() + ",";
                    }
                    else{
                        program.GenUserID += newProgramUser.UserId.ToString() + ",";
                    }
                    GenUserModel genUser = new(){
                        ProgramID = newProgramUser.ProgramID,
                        UserID = newProgramUser.UserId,
                    };
                    _context.GenUserInfo.Add(genUser);
                    break;
                    case "general":
                    if(string.IsNullOrEmpty(program.GenUserID)){
                        program.GenUserID = newProgramUser.UserId.ToString() + ",";
                    }
                    else{
                        program.GenUserID += newProgramUser.UserId.ToString() + ",";
                    }
                    GenUserModel generalUser = new(){
                        ProgramID = newProgramUser.ProgramID,
                        UserID = newProgramUser.UserId,
                    };
                    _context.GenUserInfo.Add(generalUser);
                    break;
                    case "coach":
                    if(string.IsNullOrEmpty(program.CoachID)){
                        program.CoachID = newProgramUser.UserId.ToString() + ",";
                    }
                    else{
                            program.CoachID += newProgramUser.UserId.ToString() + ",";
                    }
                    CoachModel coach = new(){
                        ProgramID = newProgramUser.ProgramID,
                        UserID = newProgramUser.UserId
                    };
                    _context.CoachInfo.Add(coach);
                    break;
                    case "admin":
                    if(string.IsNullOrEmpty(program.AdminID)){
                        program.AdminID = newProgramUser.UserId.ToString() + ",";
                    }
                    else{
                        program.AdminID += newProgramUser.UserId.ToString() + ",";
                    }
                    AdminModel admin = new(){
                        UserID = newProgramUser.UserId,
                        ProgramID = newProgramUser.ProgramID
                    };
                    _context.AdminInfo.Add(admin);
                    break;
                    default:
                    return "Invalid Status";
                }
                _context.ProgramInfo.Update(program);
                _context.SaveChanges();
                if(string.IsNullOrEmpty(userToAdd.Programs)){
                    userToAdd.Programs = program.ProgramName + ",";
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
        public Tuple<List<ProgramUserDTO>, List<ProgramUserDTO>, List<ProgramUserDTO>> GetUsernameByProgram(string ProgramName){
            ProgramModel foundProgram = _context.ProgramInfo.SingleOrDefault(p => p.ProgramName == ProgramName);
            List<ProgramUserDTO> Admins = new();
            List<ProgramUserDTO> Coaches = new();
            List<ProgramUserDTO> General = new();
            List<ProgramUserDTO> ListCreation(string ProgramStatus, string StatusIds){
                    List<ProgramUserDTO> Users = new();
                    string[] strings = StatusIds.Split(",");
                    foreach(string User in strings){
                        int.TryParse(User, out int Id);
                        UserModel foundUser = GetUserByID(Id);
                        if(foundUser != null){
                            ProgramUserDTO userDTO = new(){
                                Status = ProgramStatus,
                                UserName = foundUser.UserName,
                                RealName = foundUser.RealName,
                                Image = foundUser.Image
                            };
                            Users.Add(userDTO);
                        }
                    }
                    return Users;
                } 

            if(foundProgram != null){
                if(!string.IsNullOrEmpty(foundProgram.AdminID)){
                    Admins = ListCreation("Admin", foundProgram.AdminID);
                }

                if(!string.IsNullOrEmpty(foundProgram.CoachID)){
                    Coaches = ListCreation("Coach", foundProgram.CoachID);    
                }
                
                if(!string.IsNullOrEmpty(foundProgram.GenUserID)){
                    General = ListCreation("General", foundProgram.GenUserID);
                }
            }
            return new Tuple<List<ProgramUserDTO>, List<ProgramUserDTO>, List<ProgramUserDTO>>(Admins, Coaches, General);
        }

        public string RemoveUserFromProgram(string ProgramName, int UserID){
            ProgramModel foundProgram = _context.ProgramInfo.SingleOrDefault(p => p.ProgramName == ProgramName);
            if(foundProgram != null){
                UserModel foundUser = _context.UserInfo.SingleOrDefault(u => u.ID == UserID);
                if(foundUser != null){
                    //find the user's id in any of the Status' and remove from "Array". join Array back with ","
                    string[] AdminIds = foundProgram.AdminID.Split(",");
                    
                    if(!string.IsNullOrEmpty(foundProgram.GenUserID) && foundProgram.GenUserID.Split(",").Contains(foundUser.ID.ToString())){
                        string[] GenUserIds = foundProgram.GenUserID.Split(",");
                            GenUserIds = GenUserIds.Where(g => g != foundUser.ID.ToString()).ToArray();
                            foundProgram.GenUserID = string.Join(",", GenUserIds);
                            GenUserModel genUser = _context.GenUserInfo.SingleOrDefault(g => g.UserID == foundUser.ID && g.ProgramID == foundProgram.ProgramID);
                            if(genUser != null){
                                UserModel RemovedUser = _context.UserInfo.SingleOrDefault(u => u.ID == foundUser.ID);
                                string[] Programs = RemovedUser.Programs.Split(",");
                                Programs = Programs.Where(p => p != foundProgram.ProgramName).ToArray();
                                RemovedUser.Programs = string.Join(",", Programs);
                                _context.GenUserInfo.Remove(genUser);
                        }
                    }

                    if(!string.IsNullOrEmpty(foundProgram.CoachID) && foundProgram.CoachID.Split(",").Contains(foundUser.ID.ToString())){
                        string[] CoachIds = foundProgram.CoachID.Split(",");
                            CoachIds = CoachIds.Where(c => c != foundUser.ID.ToString()).ToArray();
                            foundProgram.CoachID = string.Join(",", CoachIds);
                            CoachModel coach = _context.CoachInfo.SingleOrDefault(c => c.UserID == foundUser.ID && c.ProgramID == foundProgram.ProgramID);
                            if(coach != null){
                                UserModel RemovedUser = _context.UserInfo.SingleOrDefault(u => u.ID == foundUser.ID);
                                string[] Programs = RemovedUser.Programs.Split(",");
                                Programs = Programs.Where(p => p != foundProgram.ProgramName).ToArray();
                                RemovedUser.Programs = string.Join(",", Programs);
                                _context.CoachInfo.Remove(coach);   
                            }
                    }

                    if(AdminIds.Contains(foundUser.ID.ToString())){
                        AdminModel adminModel = _context.AdminInfo.SingleOrDefault(a => a.UserID == foundUser.ID && a.ProgramID == foundProgram.ProgramID);
                        if(adminModel != null){
                            UserModel RemovedUser = _context.UserInfo.SingleOrDefault(u => u.ID == foundUser.ID);
                            string[] Programs = RemovedUser.Programs.Split(",");
                            Programs = Programs.Where(p => p != foundProgram.ProgramName).ToArray();
                            RemovedUser.Programs = string.Join(",", Programs);
                            _context.AdminInfo.Remove(adminModel);
                        }
                        AdminIds = AdminIds.Where(a => a != foundUser.ID.ToString()).ToArray();
                        foundProgram.AdminID = string.Join(",", AdminIds);
                    }
                    _context.ProgramInfo.Update(foundProgram);
                    _context.SaveChanges();
                    return "Sucessfully Removed";
                }
                else return "User Not Found";
            }else return "Program Not Found";
        }
        public string MoveUserInProgram(AddUserToProgramDTO UserToMove){
            var foundUser = _context.UserInfo.SingleOrDefault(u => u.ID == UserToMove.UserId);
            if(foundUser != null){
                var foundProgram = _context.ProgramInfo.SingleOrDefault(p => p.ProgramID == UserToMove.ProgramID);
                if(foundProgram != null){
                    RemoveUserFromProgram(foundProgram.ProgramName, UserToMove.UserId);
                    AddUserToProgram(UserToMove);
                }
                else return "Program Not found";
            }
            else return "User Not Found";
            _context.SaveChanges();
            return "Success";
        }
    }
}
