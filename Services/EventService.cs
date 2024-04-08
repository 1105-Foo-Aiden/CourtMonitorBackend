using CourtMonitorBackend.Models.DTO;
using CourtMonitorBackend.Services.Context;

namespace CourtMonitorBackend.Services
{
    public class EventService
    {
        private readonly DataContext _context;
        public EventService(DataContext context)
        {
            _context = context;
        }
        public bool CreateEvent(EventModel newEvent){
            _context.Add(newEvent);
            return _context.SaveChanges() != 0;
        }

        public bool DeleteEvent(EventModel eventToDelete){
            _context.Remove(eventToDelete);
            return _context.SaveChanges() != 0;
        }
    }
}