using CourtMonitorBackend.Models;
using CourtMonitorBackend.Models.DTO;
using CourtMonitorBackend.Services.Context;
using Microsoft.AspNetCore.Mvc;

namespace CourtMonitorBackend.Services
{
    public class ProgramService
    {
        private readonly DataContext _context;

        public ProgramService(DataContext context){
            _context = context;
        }

        public bool DoesProgramExist(string Program){
            return _context.ProgramInfo.SingleOrDefault(name => name.ProgramName == Program) != null;
        }
        // public string CreateProgram(ProgramModel newProgram){
        //         try{
        //             _context.ProgramInfo.Add(newProgram);
        //             _context.SaveChanges();
        //             return ("Passed");
        //         }
        //         catch(Exception ex){
        //             return (ex.Message);
        //         }
        // }

        public bool CreateProgram(ProgramDTO NewProgram){
         
                AdminModel adminModel = new();
                //create a new admin model to tie to the Program
                //if the id already exits, just add the program id to that admin
                AdminModel IsAlreadyAdmin = _context.AdminInfo.SingleOrDefault(Id => Id.UserID == NewProgram.AdminID);
                //if the admin doesn't exist, create a new instance of the model
                //creating an admin to save the Program to
                if (IsAlreadyAdmin == null){
                    adminModel.UserID = NewProgram.AdminID;
                    adminModel.ProgramID = NewProgram.ID;
                    _context.AdminInfo.Add(adminModel);
                    _context.SaveChanges();
                }
                //creating a blank program with the required varibales

                ProgramModel program = new(){
                    AdminID = NewProgram.AdminID,
                    ProgramID = NewProgram.ID,
                    ProgramName = NewProgram.ProgramName,
                    ProgramSport = NewProgram.ProgramSport
                };

                _context.ProgramInfo.Add(program);
                return _context.SaveChanges() !=0;
                

        }
        public UserModel GetAdminById(int id){
            return _context.UserInfo.SingleOrDefault(user => user.ID == id);
        }

        public IEnumerable<ProgramModel> GetAllPrograms(){
            return _context.ProgramInfo;
        }

        public ProgramModel GetProgramByName(string name){
            return _context.ProgramInfo.SingleOrDefault(p => p.ProgramName == name);
        }
        public IActionResult GetEventsByProgram(string program){
            var EventIds = _context.ProgramInfo
                .Where(x => x.ProgramName == program)
                .Select(x => x.EventID)
                .ToList();
            return new OkObjectResult(EventIds);
        }

         public bool DeleteProgram(string program){
            ProgramModel foundProgram = _context.ProgramInfo.SingleOrDefault(ProgramToDelete => ProgramToDelete.ProgramName == program);
            _context.Remove(foundProgram);
            return _context.SaveChanges() !=0;
        }
    }
}
