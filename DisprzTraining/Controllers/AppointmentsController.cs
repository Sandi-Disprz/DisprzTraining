using DisprzTraining.Extensions;
using DisprzTraining.Business;
using DisprzTraining.Models;
using Microsoft.AspNetCore.Mvc;
using DisprzTraining.Enums;
namespace DisprzTraining.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentBL _appointmentBL;
        public AppointmentsController(IAppointmentBL appointmentBL)
        {
            _appointmentBL=appointmentBL;
        }


        //design 
        //- POST /api/appointments
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Response))]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest,Type =typeof(Response))]

        public async Task<IActionResult> createAppointment(Appointment data){
           
           try{
            bool eventData= await _appointmentBL.createAppointment(data);
            return Created("~api/appointments",new Response(){StatusCode=(int)Statuscodes.ReturnCodeCreated,Message=ResponseMessage.Created.GetDescription()});  
           }
            catch(CustomExceptions.DateTimeMisMatchException)
            {
                return BadRequest(new CustomExceptions.DateTimeMisMatchException(){StatusCode=(int)Statuscodes.ReturnCodeBadRequest,ErrorMessage=ResponseMessage.MismatchDate.GetDescription()});
            }
             catch(CustomExceptions.MeetingOverLapException){
                return Conflict(new CustomExceptions.MeetingOverLapException(){StatusCode=(int)Statuscodes.ReturnCodeConflict,ErrorMessage=ResponseMessage.DateTimeOverLap.GetDescription()});
            } 
           
        }

        //- GET /api/appointments
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK,Type=typeof(List<Appointment>))]
        [ProducesResponseType(StatusCodes.Status404NotFound,Type =typeof(Response))]
        public async Task<IActionResult> getAppointments()
        {
            List<Appointment>  meetdata= await _appointmentBL.getAppoinments();
            if(meetdata.Any()){
                return Ok(meetdata);
            }
            return NotFound(new Response(){StatusCode=(int)Statuscodes.ReturnCodeNotFound,Message=ResponseMessage.NoData.GetDescription()});  
        }

        [HttpGet ("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK,Type=typeof(List<Appointment>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest,Type =typeof(Response))] 
        public async Task<IActionResult> getAppointmnetById(Guid id){
            try{
                Appointment targetData=await _appointmentBL.getAppointmentById(id);
                return Ok(targetData); 
            }   
            catch(CustomExceptions.NotIdFoundException){
                return BadRequest(new CustomExceptions.NotIdFoundException(){StatusCode=(int)Statuscodes.ReturnCodeBadRequest,ErrorMessage=ResponseMessage.IdNotFound.GetDescription()});
            }
        }

        [HttpGet("date/{date}")]
        [ProducesResponseType(StatusCodes.Status200OK,Type=typeof(List<Appointment>))]
        [ProducesResponseType(StatusCodes.Status204NoContent,Type =typeof(List<>))]
        public async Task<IActionResult> getAppointmentByDate(string date){
            List<Appointment> todayMeetings=await _appointmentBL.getAppointmentByDate(date);
            if(todayMeetings.Any()){
                return Ok(todayMeetings);
            }
            else{
                return NoContent();
            }
        }

        //- DELETE /api/appointments

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest,Type =typeof(Response))]
        public async Task<IActionResult> deleteAppointment(Guid id){
            bool IsDeleted=await _appointmentBL.DeleteAppointment(id);
            if (IsDeleted){
                return Ok(new Response(){StatusCode=(int)Statuscodes.ReturnCodeOK,Message=ResponseMessage.DeleteSuccess.GetDescription()});
            }
            return BadRequest(new Response(){StatusCode=(int)(Statuscodes.ReturnCodeBadRequest),Message=ResponseMessage.MeetingNotFound.GetDescription()});
        }

        [HttpGet("search/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        // [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> searchAppointmentByName(string name){
            try{
                List<Appointment> searchedData=await _appointmentBL.SearchAppointmentByName(name);
                if(searchedData.Any()){
                    return Ok(searchedData);
                } 
                else{
                    return NotFound();
                }
            }
            catch(CustomExceptions.NoDataFoundException){
                return NotFound(new CustomExceptions.NoDataFoundException(){StatusCode=(int)Statuscodes.ReturnCodeNotFound,ErrorMessage=ResponseMessage.DataNotFound.GetDescription()});
            }
        }
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response))]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> Update([FromBody]Appointment data)
        {
            try{
                bool updated=await _appointmentBL.UpdateAppointment(data);
                return Ok(updated);  
            }
            catch(CustomExceptions.DateTimeMisMatchException){
                return BadRequest(new CustomExceptions.DateTimeMisMatchException(){StatusCode=(int)Statuscodes.ReturnCodeConflict,ErrorMessage=ResponseMessage.DateTimeOverLap.GetDescription()});
            }
            catch(CustomExceptions.MeetingOverLapException){
                return Conflict(new CustomExceptions.MeetingOverLapException(){StatusCode=(int)Statuscodes.ReturnCodeConflict,ErrorMessage=ResponseMessage.DateTimeOverLap.GetDescription()});
            }
        }
        [HttpGet("range/{date}")]
        public async Task<ActionResult> Range(DateTime date){
            List<Appointment> rangeData= await _appointmentBL.getAppointmentsByRange(date);
            return Ok(rangeData);
        }

        [HttpDelete("delete/{startTime}")]
        
        public async Task<ActionResult> delete(DateTime startTime){
            bool IsDeleted=_appointmentBL.delete(startTime);
            if (IsDeleted){
                return Ok(new Response(){StatusCode=(int)Statuscodes.ReturnCodeOK,Message=ResponseMessage.DeleteSuccess.GetDescription()});
            }
            return BadRequest(new Response(){StatusCode=(int)(Statuscodes.ReturnCodeBadRequest),Message=ResponseMessage.MeetingNotFound.GetDescription()});
        }
        [HttpGet("holiday/{date}")]
        public async Task<ActionResult> GetHolidays(string date){
            List<string> holidayData=_appointmentBL.getHolidays(date);
            return Ok(holidayData);
        }
    }
}
