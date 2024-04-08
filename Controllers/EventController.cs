using CourtMonitorBackend.Models.DTO;
using CourtMonitorBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourtMonitorBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventController : ControllerBase
    {
        private readonly EventService _eventService;
        public EventController(EventService eventService)
        {
            _eventService = eventService;
        }

        [HttpPost]
        [Route("CreateEvent")]
        public bool CreateEvent(EventModel newEventItem)
        {
            return _eventService.CreateEvent(newEventItem);
        }

        [HttpDelete]
        [Route("DeleteEvent")]
        public bool DeleteEvent(EventModel eventToDelete){
            return _eventService.DeleteEvent(eventToDelete);
        }

    }
}