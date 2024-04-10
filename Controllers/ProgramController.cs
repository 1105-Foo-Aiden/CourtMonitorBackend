using CourtMonitorBackend.Models.DTO;
using CourtMonitorBackend.Services;
using Microsoft.AspNetCore.Mvc;


namespace CourtMonitorBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProgramController : Controller
    {
        private readonly ProgramService _model;

        public ProgramController(ProgramService model)
        {
            _model = model;
        }

        [HttpPost]
        [Route("CreateProgram/{name}")]
        public bool CreateProgram(string name, int adminID){
            return _model.CreateProgram(name, adminID); 
        }
    }
}