using CourtMonitorBackend.Models;
using CourtMonitorBackend.Models.DTO;
using CourtMonitorBackend.Services.Context;
using Microsoft.AspNetCore.Mvc;

namespace CourtMonitorBackend.Services{
    public class ProgramService{
        private readonly DataContext _context;
        public ProgramService(DataContext context){
            _context = context;
        }

        public bool DoesProgramExist(string Program){
            return _context.ProgramInfo.SingleOrDefault(name => name.ProgramName == Program) != null;
        }

        public bool CreateProgram(ProgramDTO NewProgram){
            AdminModel adminModel = new();
            //create a new admin model to tie to the Program
            //if the id already exits, just add the program id to that admin
            AdminModel IsAlreadyAdmin = _context.AdminInfo.SingleOrDefault(Id => Id.UserID == NewProgram.AdminID);
            // if the admin doesn't exist, create a new instance of the model
            if(IsAlreadyAdmin == null){
                adminModel.UserID = NewProgram.AdminID;
                // creating an admin to save the Program to
                if(string.IsNullOrEmpty(adminModel.ProgramID)){
                    adminModel.ProgramID = NewProgram.ID.ToString() + "-";
                }else if(!adminModel.ProgramID.Contains(NewProgram.ID.ToString())){
                    adminModel.ProgramID += NewProgram.ID.ToString() + "-";
                }else{
                    return false;
                }
                adminModel.ProgramID = NewProgram.ID.ToString();
                _context.AdminInfo.Add(adminModel);
                _context.SaveChanges();
            }else{
                if(string.IsNullOrEmpty(adminModel.ProgramID)){
                    adminModel.ProgramID = NewProgram.ID.ToString() + "-";
                }else if(!adminModel.ProgramID.Contains(NewProgram.ID.ToString())){
                    adminModel.ProgramID += NewProgram.ID.ToString() + "-";
                }else{
                    return false;
                }
                adminModel.ProgramID = NewProgram.ID.ToString();
                _context.AdminInfo.Update(adminModel);
                _context.SaveChanges();
            }
            //creating a blank program with the required varibales
            ProgramModel program = new(){
                AdminID = NewProgram.AdminID,
                ProgramID = NewProgram.ID,
                ProgramName = NewProgram.ProgramName,
                ProgramSport = NewProgram.ProgramSport,
                Discription = NewProgram.Description
            };
            _context.ProgramInfo.Add(program);
            return _context.SaveChanges() !=0;
        }
        public UserModel GetAdminById(int id){
            AdminModel foundAdmin =  _context.AdminInfo.SingleOrDefault(user => user.Id == id);
            return _context.UserInfo.SingleOrDefault(user => user.ID == foundAdmin.UserID);
        }

        public IEnumerable<ProgramModel> GetAllPrograms(){
            return _context.ProgramInfo;
        }

        public ProgramModel GetProgramById(int ProgramId){
            return _context.ProgramInfo.SingleOrDefault(p => p.ProgramID == ProgramId);
        }

        public IActionResult GetEventsByProgram(string program){
            var EventIds = _context.ProgramInfo
                .Where(x => x.ProgramName == program)
                .Select(x => x.EventIds)
                .ToList();
            return new OkObjectResult(EventIds);
        }

        public IEnumerable<EventModel> RemoveEventsByProgramID(int id){
            var events = _context.EventInfo.Where(e => e.ProgramID == id.ToString());
            return events;
        }
        
        public bool DeleteProgram(string program){
            bool result = false;
            ProgramModel foundProgram = _context.ProgramInfo.SingleOrDefault(ProgramToDelete => ProgramToDelete.ProgramName == program);
            if(foundProgram != null){
                _context.Remove(foundProgram);
                var events = RemoveEventsByProgramID(foundProgram.ProgramID);
                _context.Remove(events);
                result = _context.SaveChanges() != 0;
            }
            return result;
        }
        public IEnumerable<ProgramModel> GetProgramsBySport(string sport){
            return _context.ProgramInfo.Where(p => p.ProgramSport.ToLower() == sport.ToLower());
        } 

        public bool AddUserToProgram(AddUserToProgramDTO newProgramUser){
            bool result = false;
            ProgramModel foundProgram = _context.ProgramInfo.SingleOrDefault(program => program.ProgramID == newProgramUser.ProgramID);
            if(foundProgram != null){
                if(newProgramUser.Status.ToLower() == "genuser" || newProgramUser.Status.ToLower() == "general" ){
                    if(string.IsNullOrEmpty(foundProgram.GenUserID)){
                        foundProgram.GenUserID = newProgramUser.UserId.ToString() + "-";
                    }else{
                        foundProgram.GenUserID += newProgramUser.UserId.ToString() + "-";
                    }
                }
                if(newProgramUser.Status.ToLower() == "coach"){
                    if(string.IsNullOrEmpty(foundProgram.CoachID)){
                        foundProgram.CoachID = newProgramUser.UserId.ToString() + "-";
                    }else{
                        foundProgram.CoachID += newProgramUser.UserId.ToString() + "-";
                    }
                }
                if(newProgramUser.Status.ToLower() == "admin"){
                    if(string.IsNullOrEmpty(foundProgram.CoachID)){
                        foundProgram.AdminID = newProgramUser.UserId;
                    }else{
                        foundProgram.AdminID += newProgramUser.UserId;
                    }
                }
            }else{
                result = false;
            }
            return result;
        }
    }
}
