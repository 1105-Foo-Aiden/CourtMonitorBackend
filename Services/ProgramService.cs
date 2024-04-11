using CourtMonitorBackend.Services.Context;

namespace CourtMonitorBackend.Services
{
    public class ProgramService
    {
        private readonly DataContext _context;

        public ProgramService(DataContext context)
        {
            _context = context;
        }
        public bool DoesProgramExist(string Program){
            return _context.ProgramInfo.SingleOrDefault(name => name.ProgramName == Program) != null;
        }
        // public bool CreateProgram(ProgramDTO program){
        //     bool result = false;

        //     if(!DoesProgramExist(program)){
        //         ProgramModel newProgram = new();
        //         newProgram.ProgramName = program.Name;
        //         newProgram.AdminID = program.AdminID;
        //         _context.Add(newProgram);
        //         result = _context.SaveChanges() !=0;
        //     }
        //     return result;
        // }
    }
}