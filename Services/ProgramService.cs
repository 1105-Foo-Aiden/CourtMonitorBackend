using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public bool DoesProgramExist(string Program){
            return _context.ProgramInfo.SingleOrDefault(name => name.ProgramName == Program) != null;
        }
        public bool CreateProgram(string nameofProgram, int adminId){
            bool result = false;
            if(!DoesProgramExist(nameofProgram)){
                ProgramModel newProgram = new();
                newProgram.ProgramName = nameofProgram;
               newProgram.AdminID = adminId;
            }
            return result;
        }
    }
}