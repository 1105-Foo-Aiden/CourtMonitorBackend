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
        
        [HttpGet]
        [Route("GetEventsByprogram/{program}")]
        public IActionResult GetEventsBySProgram(string program){
            return _eventService.GetEventsByProgram(program);
        }

        [HttpGet]
        [Route("GetProgramByName/{name}")]
        public ProgramModel GetProgramByName(string program){
            return _eventService.GetProgramByName(program);
        }

        [HttpPost]
        [Route("CreateEvent")]
        public bool CreateEvent([FromBody] EventModel newEvent){
            return _eventService.CreateEvent(newEvent);
        }
    }
}