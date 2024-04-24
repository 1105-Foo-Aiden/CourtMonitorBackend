using CourtMonitorBackend.Models.DTO;
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

        public bool DoesProgramExist(string Program)
        {
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

        public bool CreateProgram(ProgramDTO newProgram)
        {
            ProgramModel createdProgram = new()
            {
                ProgramName = newProgram.ProgramName,
                AdminID = newProgram.AdminID,
                ProgramSport = newProgram.ProgramSport
            };
            
            _context.Add(createdProgram);
            return _context.SaveChanges()!=0;
        }
    }            
}
