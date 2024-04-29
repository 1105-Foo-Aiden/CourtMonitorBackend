using CourtMonitorBackend.Models.DTO;
using CourtMonitorBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourtMonitorBackend.Controllers
{
    [Route("[controller]")]
    public class EventController : ControllerBase
    {
        private readonly EventService _eventService;
        public EventController(EventService eventService){
            _eventService = eventService;
        }

        [HttpGet]
        [Route("GetAllEvents")]
        public IEnumerable<EventModel> GetAllEvents(){
            return _eventService.GetAllEvents();
        }
        
       

        [HttpPost]
        [Route("CreateEvent")]
        public ProgramModel CreateEvent([FromBody] EventModel newEvent){
            return _eventService.CreateEvent(newEvent);
        }
    }
}