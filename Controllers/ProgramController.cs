using CourtMonitorBackend.Models;
using CourtMonitorBackend.Models.DTO;
using CourtMonitorBackend.Services;
using Microsoft.AspNetCore.Mvc;


namespace CourtMonitorBackend.Controllers{
    [ApiController]
    [Route("[controller]")]
    public class ProgramController : Controller{
        private readonly ProgramService _model;
        public ProgramController(ProgramService model){
            _model = model;
        }

        [HttpPost]
        [Route("CreateProgram")]
        public bool CreateProgram(ProgramDTO newProgram){
            return _model.CreateProgram(newProgram);
        }

        [HttpGet]
        [Route("GetAdminById/{id}")]
        public UserModel GetAdminById(int id){
            return _model.GetAdminById(id);
        }

        [HttpGet]
        [Route("GetAllPrograms")]
        public IEnumerable<ProgramModel> GetAllPrograms(){
            return _model.GetAllPrograms();
        }

        [HttpGet]
        [Route("GetEventsByprogram/{program}")]
        public IActionResult GetEventsBySProgram(string program){
            return _model.GetEventsByProgram(program);
        }

        [HttpGet]
        [Route("GetProgramById/{programId}")]
        public ProgramModel GetProgramById(int programId){
            return _model.GetProgramById(programId);
        }

        [HttpDelete]
        [Route("DeleteProgram/program")]
        public bool DeleteProgram(string program){
            return _model.DeleteProgram(program);
        }

        [HttpGet]
        [Route("GetProgramsBySport/sport")]
        public IEnumerable<ProgramModel> GetProgramsBySport(string sport){
            return _model.GetProgramsBySport(sport);
        }

        [HttpPost]
        [Route("AddUserToProgram")]
        public bool AddUserToProgram(AddUserToProgramDTO userProgram){
            return _model.AddUserToProgram(userProgram);
        }

    }
}