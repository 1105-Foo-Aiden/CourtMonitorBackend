using CourtMonitorBackend.Models.DTO;
using CourtMonitorBackend.Services.Context;
using Microsoft.AspNetCore.Mvc;

namespace CourtMonitorBackend.Services
{
    public class EventService
    {
        private readonly DataContext _context;
        public EventService(DataContext context){
            _context = context;
        }
        private readonly ProgramModel _program;

        public EventService(ProgramModel program){
            _program = program;
        }

        public ProgramModel CreateEvent(EventModel newEvent){
           
            _context.Add(newEvent);
            _context.SaveChanges();
            ProgramModel foundProgram = _context.ProgramInfo.FirstOrDefault(p => p.ProgramID == newEvent.ProgramID);
            if(foundProgram != null){
                if(foundProgram.EventID == null){
                    foundProgram.EventID = newEvent.EventID;
                }
                else{
                    foundProgram.EventID += newEvent.EventID;
                }
               
                 _context.SaveChanges();
            }
            return foundProgram;
        }
        public IEnumerable<EventModel> GetAllEvents(){
            return _context.EventInfo;
        }

        public EventModel GetEventById(int id){
            return _context.EventInfo.FirstOrDefault(e => e.EventID == id);
        }
       

       
    
    }
}