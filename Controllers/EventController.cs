using CourtMonitorBackend.Models.DTO;
using CourtMonitorBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourtMonitorBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventController : ControllerBase
    {
        private readonly EventService _data;
        public EventController(EventService data)
        {
            _data = data;
        }

        [HttpPost]
        [Route("CreateEvent")]
        public bool CreateEvent(EventModel newEventItem)
        {
            return _data.CreateEvent(newEventItem);
        }

        
        [HttpGet]
        [Route("GetEventsBySport")]
        public IEnumerable<EventModel> GetEventsBySport(string sport){
            return _data.GetEventsBySport(sport);
        }


        [HttpDelete]
        [Route("DeleteEvent")]
        public bool DeleteEvent(EventModel eventToDelete){
            return _data.DeleteEvent(eventToDelete);
        }

    }
}