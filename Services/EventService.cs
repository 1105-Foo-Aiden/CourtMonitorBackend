using CourtMonitorBackend.Models.DTO;
using CourtMonitorBackend.Services.Context;

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

        public bool CreateEvent(EventModel newEvent){
            _context.Add(newEvent);
            //saves new event
            _context.SaveChanges();
            ProgramModel ProgramToAddEvent = _context.ProgramInfo.FirstOrDefault(p => p.ProgramID == newEvent.ProgramID);
            if(ProgramToAddEvent != null){
               ProgramToAddEvent.EventID = newEvent.ID; 
               return _context.SaveChanges() !=0;
            }
            return false;
            
        }
        public IEnumerable<EventModel> GetAllEvents(){
            return _context.EventInfo;
        }

        public EventModel GetEventById(int id){
            return _context.EventInfo.FirstOrDefault(e => e.ID == id);
        }

        public IEnumerable<EventModel> GetAllEventsByProgramID(int programId){
            int ProgramID;
            ProgramModel foundProgram = _context.ProgramInfo.FirstOrDefault(e => e.ProgramID == programId);
            return _context.EventInfo.Where(e => e.ProgramID == foundProgram.ProgramID);
        }
    }
}