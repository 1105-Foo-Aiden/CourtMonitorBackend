using CourtMonitorBackend.Models.DTO;
using CourtMonitorBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourtMonitorBackend.Controllers{
    [ApiController]
    [Route("[controller]")]
    public class EventController : ControllerBase{
        private readonly EventService _eventService;
        public EventController(EventService eventService){
            _eventService = eventService;
        }

        [HttpPost]
        [Route("CreateEvent")]
        public bool CreateEvent([FromBody] EventModel newEvent){
            return _eventService.CreateEvent(newEvent);
        }
        
        [HttpGet]
        [Route("GetAllEvents")]
        public IEnumerable<EventModel> GetAllEvents(){
            return _eventService.GetAllEvents();
        }

        [HttpGet]
        [Route("GetEventsByProgramID/{programID}")]
        public IEnumerable<EventModel> GetEventsByProgramID(int programID){
            return _eventService.GetAllEventsByProgramID(programID);
        }


        [HttpDelete]
        [Route("DeleteEvent/{id}")]
        public string DeleteEvent(int id){
            return _eventService.DeleteEvent(id);
        }

    }
}