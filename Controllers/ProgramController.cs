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

        // [HttpPost]
        // [Route("CreateProgram/{name}/{adminID}")]
        // public bool CreateProgram(ProgramDTO program){
        //     return _model.CreateProgram(program); 
        // }
    }
}