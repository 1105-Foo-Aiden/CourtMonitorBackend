using CourtMonitorBackend.Models.DTO;
using CourtMonitorBackend.Services.Context;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CourtMonitorBackend.Services
{
    public class EventService
    {
        private readonly DataContext _context;
        public EventService(DataContext context){
            _context = context;
        }
        private readonly ProgramDTO _programDTO;

        public EventService(ProgramDTO programDTO){
            _programDTO = programDTO;
        }

        public bool CreateEvent(EventModel newEvent){
            _context.Add(newEvent);
            return _context.SaveChanges() != 0;
        }
        public IEnumerable<EventModel> GetAllEvents(){
            return _context.EventInfo;
        }

        public EventModel GetEventById(int id){
            return _context.EventInfo.FirstOrDefault(e => e.EventID == id);
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
    

        public bool DeleteEvent(EventModel eventToDelete){
            eventToDelete.IsDeleted = true;
            _context.Update<EventModel>(eventToDelete);
            return _context.SaveChanges() != 0;
        }
    }
}