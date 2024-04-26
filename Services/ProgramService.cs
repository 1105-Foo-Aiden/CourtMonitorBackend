using CourtMonitorBackend.Models;
using CourtMonitorBackend.Models.DTO;
using CourtMonitorBackend.Services.Context;

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

        public string CreateProgram(ProgramDTO NewProgram){
            try{
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
                _context.SaveChanges();
                return "Yes";
            }
            catch(Exception ex){
                return (ex.InnerException).ToString();
            }

        }
        public UserModel GetAdminById(int id){
            return _context.UserInfo.SingleOrDefault(user => user.ID == id);
        }
    }
}
