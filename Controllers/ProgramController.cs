using CourtMonitorBackend.Models;
using CourtMonitorBackend.Models.DTO;
using CourtMonitorBackend.Services;
using Microsoft.AspNetCore.Mvc;


namespace CourtMonitorBackend.Controllers{
    [ApiController]
    [Route("[controller]")]
    public class ProgramController : ControllerBase{
        private readonly ProgramService _model;
        public ProgramController(ProgramService model){
            _model = model;
        }
        [HttpGet]
        [Route("GetAdminById/{id}")]
        public UserModel GetAdminById(int id){
            return _model.GetAdminById(id);
        }

        [HttpGet]
        [Route("GetUsersByProgramName/{ProgramName}")]
        public (List<ProgramUserDTO> Admins, List<ProgramUserDTO> Coaches, List<ProgramUserDTO> GenUsers) GetUsersByProgramName(string ProgramName){
            return _model.GetUsernameByProgram(ProgramName);
        }

        [HttpGet]
        [Route("GetAllPrograms")]
        public IEnumerable<ProgramModel> GetAllPrograms(){
            return _model.GetAllPrograms();
        }

        [HttpGet]
        [Route("GetProgramById/{programId}")]
        public ProgramModel GetProgramById(int programId){
            return _model.GetProgramById(programId);
        }

        [HttpGet]
        [Route("GetProgramByName/{ProgamName}")]
        public ProgramModel GetProgramByName(string ProgamName){
            return _model.GetProgramByProgramName(ProgamName);
        }

        [HttpGet]
        [Route("GetProgramsBySport/{sport}")]
        public IEnumerable<ProgramModel> GetProgramsBySport(string sport){
            return _model.GetProgramsBySport(sport);
        }
        
        // [HttpGet]
        // [Route("GetUsersByProgramId/{ID}")]
        // public IEnumerable<UserModel> GetUsersByProgramId(int ID){
        //     return _model.GetUsersByProgramId(ID);
        // }

        [HttpPost]
        [Route("CreateProgram")]
        public string CreateProgram(ProgramDTO newProgram){
            return _model.CreateProgram(newProgram);
        }

        [HttpPost]
        [Route("AddUserToProgram")]
        public string AddUserToProgram(AddUserToProgramDTO userProgram){
            try{
                return _model.AddUserToProgram(userProgram);
            }catch(Exception ex){
                return ex.Message;
            }
        }

        [HttpDelete]
        [Route("DeleteProgram/{program}")]
        public bool DeleteProgram(string program){
            return _model.DeleteProgram(program);
        }
    }
}