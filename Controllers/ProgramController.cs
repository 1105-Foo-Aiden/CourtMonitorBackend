using CourtMonitorBackend.Models;
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
        [Route("CreateProgram")]
        public string CreateProgram(ProgramDTO newProgram){
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
    }
}