using CourtMonitorBackend.Models.DTO;
using CourtMonitorBackend.Services.Context;

// Events are not tied to the Programs, they each have a program ID but are not listed in the Program Table,
// Events should be linked with Programs in a refactor of the DB

namespace CourtMonitorBackend.Services{
    public class EventService{
        private readonly DataContext _context;
        public EventService(DataContext context) => _context = context;
        private readonly ProgramModel _program;
        public EventService(ProgramModel program) => _program = program;
        public ProgramModel GetProgramById(string id) => _context.ProgramInfo.SingleOrDefault(program => program.ProgramID.ToString() == id);
        public IEnumerable<EventModel> GetAllEvents() => _context.EventInfo;
        public EventModel GetEventById(int id) => _context.EventInfo.FirstOrDefault(e => e.id == id);
        public bool CreateEvent(EventModel newEvent){
            EventModel Event = new(){
                id = newEvent.id,
                Title = newEvent.Title,
                Start = newEvent.Start,
                End = newEvent.End,
                Color = newEvent.Color,
                ProgramID = newEvent.ProgramID,
                AllDay = newEvent.AllDay
            };
            _context.Add(Event);
            return _context.SaveChanges() != 0;
        }
        public string DeleteEvent(int EventId){
            EventModel foundEvent = GetEventById(EventId);
            if (foundEvent != null){
                _context.Remove(foundEvent);
                _context.SaveChanges();
                return "Event Deleted";
            }
            else return "Event Not Found";
        }
        public IEnumerable<EventModel> GetAllEventsByProgramName(string ProgramName){
            ProgramModel foundProgram = _context.ProgramInfo.FirstOrDefault(e => e.ProgramName == ProgramName);
            return _context.EventInfo.Where(e => e.ProgramID == foundProgram!.ProgramID);
        }
    }
}