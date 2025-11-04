using CourtMonitorBackend.Models;
using CourtMonitorBackend.Models.DTO;
using CourtMonitorBackend.Services.Context;

namespace CourtMonitorBackend.Services{
    public class ProgramService{
        private readonly DataContext _context;
        public ProgramService(DataContext context) =>_context = context;
        public bool DoesProgramExist(string Program) => _context.ProgramInfo.SingleOrDefault(name => name.ProgramName == Program) != null;
        public bool DoesProgramIDExist(int programId) => _context.ProgramInfo.SingleOrDefault(id => id.ProgramID == programId) != null;
        public string CreateProgram(ProgramDTO NewProgram){
            //Create a blank template of a program using the DTO to fill out the required information
            //Then take the user from the DTO and add the Program Name to the account under the "Programs" Data point
            //Then Create a new Admin in the Admin Table with the ID of the program instead of the name
            //Save the database after each step to ensure that each new data point is saved.
            //Trims for the possibility of spaces
            try{
                ProgramModel ProgramToAdd = new(){
                    ProgramName = NewProgram.ProgramName.Trim(),
                    ProgramSport = NewProgram.ProgramSport,
                    Description = NewProgram.Description,
                    AdminID = NewProgram.AdminID + ","
                };
                
                if(DoesProgramExist(NewProgram.ProgramName.Trim())){ return "Program already exists"; }
                _context.ProgramInfo.Add(ProgramToAdd);
                _context.SaveChanges();
                int AdminIdNumber = int.Parse(NewProgram.AdminID); //Setting the admin's ID from the DTO recieved from front end
                UserModel? User = _context.UserInfo.SingleOrDefault(u => u.ID == AdminIdNumber);
                //Adding the new program to the admin user's program list
                if(User !=null){
                    User.Programs = string.IsNullOrEmpty(User.Programs) ? ProgramToAdd.ProgramName.Trim() + "," : User.Programs + ProgramToAdd.ProgramName.Trim() + ",";
                    _context.UserInfo.Update(User);
                }else return "User not found";
                //Creating a new admin in the admin lookup table
                AdminModel admin = new(){
                    UserID = AdminIdNumber,
                    ProgramID = ProgramToAdd.ProgramID,
                };
                _context.AdminInfo.Add(admin);
                _context.SaveChanges(); 
                return "Success";
            }catch(Exception ex){
                return ex.Message;
            }
        }    
        public UserModel? GetAdminById(int id){
            var foundAdmin =  _context.AdminInfo.SingleOrDefault(user => user.Id == id);
            return _context.UserInfo.SingleOrDefault(user => user.ID == foundAdmin!.UserID);
        }
        public IEnumerable<ProgramModel> GetAllPrograms() => _context.ProgramInfo;
        public ProgramModel? GetProgramById(int ProgramId) => _context.ProgramInfo.SingleOrDefault(p => p.ProgramID == ProgramId);
        public IEnumerable<EventModel> GetAllEventsByProgramID(int programId){
            var foundProgram = _context.ProgramInfo.FirstOrDefault(e => e.ProgramID == programId);
            return _context.EventInfo.Where(e => e.ProgramID == foundProgram!.ProgramID);
        }
        
        public UserModel? GetUserByUsername(string Username)=> _context.UserInfo.SingleOrDefault(u => u.UserName == Username);
        public bool DeleteProgram(string program){
            bool result = false;
            var foundProgram = _context.ProgramInfo.SingleOrDefault(ProgramToDelete => ProgramToDelete.ProgramName == program);
            if(foundProgram != null){
                //Get all the users from the found program and remove the name from their Programs list.
                var AllUsers = GetUsernameByProgram(foundProgram.ProgramName);
                foreach(var user in AllUsers.Item1){
                    UserModel? foundUser = GetUserByUsername(user.UserName);
                    if(foundUser != null && !string.IsNullOrEmpty(foundUser.Programs)){
                        string[] Programs = foundUser.Programs.Split(","); //Split the user's program array
                        Programs = Programs.Where(p => p != foundProgram.ProgramName).ToArray(); //Removing the now deleted program
                        foundUser.Programs = string.Join(",", Programs); //Rejoin the programs
                    }
                }
                _context.Remove(foundProgram);
                result = _context.SaveChanges() != 0;
            }
            return result;
        }
        public IEnumerable<ProgramModel> GetProgramsBySport(string sport) => _context.ProgramInfo.Where(p => p.ProgramSport.ToLower() == sport.ToLower());
        public ProgramModel? GetProgramByProgramName(string ProgramName) => _context.ProgramInfo.FirstOrDefault(p => p.ProgramName == ProgramName);
        public string AddUserToProgram(AddUserToProgramDTO newProgramUser){
            //Similar to the Create Program, I want to add A user to an existing Program
            //First, I need to find the program
            //Second, I use the User Id from the DTO to add it to whichever level in the Program based on the "Status"
            //Then I add the Program Name to the User's Progam Point, will use the same logic as before
            try{
                ProgramModel? program = _context.ProgramInfo.SingleOrDefault(p => p.ProgramID == newProgramUser.ProgramID);
                UserModel? userToAdd = _context.UserInfo.SingleOrDefault(u => u.ID == newProgramUser.UserId);
                if(program == null){ return "Program Not Found, Try again"; }
                if(userToAdd == null){ return "Cannot find user to add"; }
                if(!string.IsNullOrEmpty(program.GenUserID) && program.GenUserID.Split(",").Contains(newProgramUser.UserId.ToString()) || !string.IsNullOrEmpty(program.CoachID) && program.CoachID.Split(",").Contains(newProgramUser.UserId.ToString()) || program.AdminID.Split(",").Contains(newProgramUser.UserId.ToString())){
                    //Logic for making sure that the user is not already in the program
                    return "User is already a part of the program";
                }
                switch(newProgramUser.Status.ToLower()){
                    //Based on the type of user that is being added, a new row in the lookup table is being created for each possibility
                    case "genuser":
                        //If the program's status is blank, add the new user by itself, otherwise, add the new user to the existing "List"
                        //This is the same for all types of users
                        program.GenUserID = string.IsNullOrEmpty(program.GenUserID) ? newProgramUser.UserId.ToString() + "," : program.GenUserID + newProgramUser.UserId.ToString() + ",";
                        //Adding the user to the lookup tables, to never be used again, same for all cases
                        GenUserModel genUser = new(){
                            ProgramID = newProgramUser.ProgramID,
                            UserID = newProgramUser.UserId,
                        };
                        _context.GenUserInfo.Add(genUser);
                        break;
                    case "general":
                        program.GenUserID = string.IsNullOrEmpty(program.GenUserID) ? newProgramUser.UserId.ToString() + "," : program.GenUserID + newProgramUser.UserId.ToString() + ",";
                        GenUserModel generalUser = new(){
                            ProgramID = newProgramUser.ProgramID,
                            UserID = newProgramUser.UserId,
                        };
                        _context.GenUserInfo.Add(generalUser);
                    break;
                    case "coach":
                        program.CoachID = string.IsNullOrEmpty(program.CoachID) ? newProgramUser.UserId.ToString() + "," : program.CoachID + newProgramUser.UserId.ToString() + ",";
                        CoachModel coach = new(){
                            ProgramID = newProgramUser.ProgramID,
                            UserID = newProgramUser.UserId
                        };
                        _context.CoachInfo.Add(coach);
                        break;
                    case "admin":
                        program.AdminID = string.IsNullOrEmpty(program.AdminID) ? newProgramUser.UserId.ToString() + "," : program.AdminID + newProgramUser.UserId.ToString() +",";
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
                userToAdd.Programs = string.IsNullOrEmpty(userToAdd.Programs) ? program.ProgramName +"," : userToAdd.Programs + program.ProgramName + ",";
                _context.UserInfo.Update(userToAdd);
                _context.SaveChanges();
                return "Success";
            } catch(Exception ex){ return ex.Message; }
        }
        public UserModel? GetUserByID(int id) => _context.UserInfo.SingleOrDefault(u => u.ID == id);
        public Tuple<List<ProgramUserDTO>, List<ProgramUserDTO>, List<ProgramUserDTO>> GetUsernameByProgram(string ProgramName){
            ProgramModel? foundProgram = _context.ProgramInfo.SingleOrDefault(p => p.ProgramName == ProgramName);
            List<ProgramUserDTO> Admins = new();
            List<ProgramUserDTO> Coaches = new();
            List<ProgramUserDTO> General = new();
            //ProgramUserDTO
            //public string Status { get; set; }
            //public string UserName { get; set; }
            //public string RealName { get; set; }
            //public string? Image { get; set; }
            List<ProgramUserDTO> ListCreation(string ProgramStatus, string StatusIds){
                    List<ProgramUserDTO> Users = new(); //Empty list of users
                    string[] strings = StatusIds.Split(","); //Split the user ID's from the program's list in category
                    foreach(string User in strings){
                        int.TryParse(User, out int Id); //Change id's to integers
                        UserModel? foundUser = GetUserByID(Id); //Find the user with the ID
                        if(foundUser != null){
                            ProgramUserDTO userDTO = new(){
                                Status = ProgramStatus, //Using Parameter to confirm they are that status
                                UserName = foundUser.UserName,
                                RealName = foundUser.RealName,
                                Image = foundUser.Image 
                            };
                            Users.Add(userDTO); //Add to users list
                        }
                    }
                    return Users;
                } 
            if(foundProgram != null){
                //Each conditional is checking to make sure that there are users in each type of member of the program
                //Making sure the Tuple does not reaturn null and mess up the front end
                if(!string.IsNullOrEmpty(foundProgram.AdminID)){ Admins = ListCreation("Admin", foundProgram.AdminID); }
                if(!string.IsNullOrEmpty(foundProgram.CoachID)){ Coaches = ListCreation("Coach", foundProgram.CoachID); }
                if(!string.IsNullOrEmpty(foundProgram.GenUserID)){ General = ListCreation("General", foundProgram.GenUserID); }
            }
            return new Tuple<List<ProgramUserDTO>, List<ProgramUserDTO>, List<ProgramUserDTO>>(Admins, Coaches, General);
        }
        public string RemoveUserFromProgram(string ProgramName, int UserID){
            ProgramModel? foundProgram = _context.ProgramInfo.SingleOrDefault(p => p.ProgramName == ProgramName);
            if(foundProgram != null){
                UserModel? foundUser = _context.UserInfo.SingleOrDefault(u => u.ID == UserID);
                if(foundUser != null){
                    //Find the user's id in any of the Status' and remove from "Array". join Array back with ","
                    string[] AdminIds = foundProgram.AdminID.Split(",");
                    //All removals follow the same pattern =>
                    //Split all users in the specified area of the program,
                    //Remove the user from the list, and then join the list back together with ","
                    //Take each ID and remove the program name from their Program List
                    //Remove user and program ID from the Lookup Tables
                    if(!string.IsNullOrEmpty(foundProgram.GenUserID) && foundProgram.GenUserID.Split(",").Contains(foundUser.ID.ToString())){
                        string[] GenUserIds = foundProgram.GenUserID.Split(",");
                            GenUserIds = GenUserIds.Where(g => g != foundUser.ID.ToString()).ToArray();
                            foundProgram.GenUserID = string.Join(",", GenUserIds);
                            GenUserModel? genUser = _context.GenUserInfo.SingleOrDefault(g => g.UserID == foundUser.ID && g.ProgramID == foundProgram.ProgramID);
                            if(genUser != null){
                            UserModel? RemovedUser = _context.UserInfo.SingleOrDefault(u => u.ID == foundUser.ID);
                                if(RemovedUser != null && !string.IsNullOrEmpty(RemovedUser.Programs)){
                                    string[] Programs = RemovedUser.Programs.Split(",");
                                    Programs = Programs.Where(p => p != foundProgram.ProgramName).ToArray();
                                    RemovedUser.Programs = string.Join(",", Programs);
                                    _context.GenUserInfo.Remove(genUser);
                                }
                            }
                    }

                    if(!string.IsNullOrEmpty(foundProgram.CoachID) && foundProgram.CoachID.Split(",").Contains(foundUser.ID.ToString())){
                        string[] CoachIds = foundProgram.CoachID.Split(",");
                            CoachIds = CoachIds.Where(c => c != foundUser.ID.ToString()).ToArray();
                            foundProgram.CoachID = string.Join(",", CoachIds);
                            CoachModel? coach = _context.CoachInfo.SingleOrDefault(c => c.UserID == foundUser.ID && c.ProgramID == foundProgram.ProgramID);
                            if(coach != null){
                            UserModel? RemovedUser = _context.UserInfo.SingleOrDefault(u => u.ID == foundUser.ID);
                                if(RemovedUser != null && !string.IsNullOrEmpty(RemovedUser.Programs)){
                                    string[] Programs = RemovedUser.Programs.Split(",");
                                    Programs = Programs.Where(p => p != foundProgram.ProgramName).ToArray();
                                    RemovedUser.Programs = string.Join(",", Programs);
                                    _context.CoachInfo.Remove(coach);   
                                }
                            }
                    }

                    if(AdminIds.Contains(foundUser.ID.ToString())){
                        AdminModel? adminModel = _context.AdminInfo.SingleOrDefault(a => a.UserID == foundUser.ID && a.ProgramID == foundProgram.ProgramID);
                        if(adminModel != null){
                            UserModel? RemovedUser = _context.UserInfo.SingleOrDefault(u => u.ID == foundUser.ID);
                            if(RemovedUser != null && !string.IsNullOrEmpty(RemovedUser.Programs)){
                                string[] Programs = RemovedUser.Programs.Split(",");
                                Programs = Programs.Where(p => p != foundProgram.ProgramName).ToArray();
                                RemovedUser.Programs = string.Join(",", Programs);
                                _context.AdminInfo.Remove(adminModel);
                            }
                        }
                        AdminIds = AdminIds.Where(a => a != foundUser.ID.ToString()).ToArray();
                        foundProgram.AdminID = string.Join(",", AdminIds);
                    }
                    _context.ProgramInfo.Update(foundProgram);
                    _context.SaveChanges();
                    return "Sucessfully Removed";
                }else return "User Not Found";
            }else return "Program Not Found";
        }
        public string MoveUserInProgram(AddUserToProgramDTO UserToMove){
            var foundUser = _context.UserInfo.SingleOrDefault(u => u.ID == UserToMove.UserId);
            if(foundUser != null){
                var foundProgram = _context.ProgramInfo.SingleOrDefault(p => p.ProgramID == UserToMove.ProgramID);
                if(foundProgram != null){
                    RemoveUserFromProgram(foundProgram.ProgramName, UserToMove.UserId);
                    AddUserToProgram(UserToMove);
                }else return "Program Not found";
            }else return "User Not Found";
            _context.SaveChanges();
            return "Success";
        }
    }
}
