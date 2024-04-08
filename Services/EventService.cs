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
        public IEnumerable<EventModel> GetAllEvents(){
            return _context.EventInfo;
        }

        public IEnumerable<EventModel> GetEventsBySport(string sport){
            var allItems = GetAllEvents().ToList();

            var filteredItems = allItems.Where(item => item.Sport.Split(',').Contains(sport));
            
            return filteredItems;
        }

        public bool DeleteEvent(EventModel eventToDelete){
            eventToDelete.IsDeleted = true;
            _context.Update<EventModel>(eventToDelete);
            return _context.SaveChanges() != 0;
        }
    }
}