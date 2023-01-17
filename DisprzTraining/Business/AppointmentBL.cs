using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DisprzTraining.DataAccess;
using DisprzTraining.Models;
using DisprzTraining.Data;
using DisprzTraining.CustomExceptions;
namespace DisprzTraining.Business
{
    public class AppointmentBL:IAppointmentBL
    {
        private readonly IAppointmentDAL _appointmentDAL;
        public AppointmentBL(IAppointmentDAL appointmentDAL){
            _appointmentDAL=appointmentDAL;
        }
        public Task<bool> AppointmentTimeMatchCheck(Appointment appointment){
            if(appointment.StartTime == appointment.EndTime){
                throw new CustomExceptions.DateTimeMisMatchException(){StatusCode=200,ErrorMessage="no"};
            }
            else if(appointment.StartTime>appointment.EndTime){
                throw new CustomExceptions.DateTimeMisMatchException();
            }
            else if(EventData.count>0){
                foreach(Appointment obj in EventData.meetingData){
                    if(obj.StartTime < appointment.EndTime &&  obj.EndTime>appointment.StartTime){
                        return Task.FromResult(false);
                    }
                }   
            }
            return Task.FromResult(true);
        }
        public  Task<bool> createAppointment(Appointment eventdata){

            if(eventdata.StartTime == eventdata.EndTime){
                throw new CustomExceptions.DateTimeMisMatchException(){};
            }
            else if(eventdata.StartTime>eventdata.EndTime){
                throw new CustomExceptions.DateTimeMisMatchException();
            }
            else if(EventData.count>0){
                foreach(Appointment obj in EventData.meetingData){
                    if(obj.StartTime < eventdata.EndTime &&  obj.EndTime>eventdata.StartTime){
                        throw new CustomExceptions.MeetingOverLapException();
                    }
                }   
            }
            return _appointmentDAL.CreateAppointment(eventdata);
        }

        public Task<List<Appointment>> getAppoinments()
        {
            return _appointmentDAL.GetAppointments();
        }
        public Task<Appointment> getAppointmentById(Guid id){
           foreach(Appointment obj in EventData.meetingData){
                if(obj.Id==id){
                    return Task.FromResult(obj);
                }
           }
           throw new CustomExceptions.NotIdFoundException();
        }
        public Task<List<Appointment>> getAppointmentByDate(string date){
            List<Appointment> meetingList=new List<Appointment>(){};
            foreach(Appointment obj in EventData.meetingData){
                if(obj.EventDate.Equals(date)){
                    meetingList.Add(obj);
                }
            }
            return Task.FromResult(meetingList);
        }

        public Task<bool> DeleteAppointment(Guid id)
        {
            bool IsDeleted=_appointmentDAL.DeleteAppointment(id);
            if(IsDeleted){
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
            
        }

        public Task<List<Appointment>> SearchAppointmentByName(string name)
        {

            List<Appointment> TargetData= _appointmentDAL.SearchAppointment(name);
            
            if(TargetData.Any()){
                return Task.FromResult(TargetData);
            }
            else{
                throw new CustomExceptions.NoDataFoundException();
            }
        }

        public Task<bool> UpdateAppointment(Appointment eventData)
        {
            if(eventData.StartTime==eventData.EndTime || eventData.StartTime>eventData.EndTime){
                throw new CustomExceptions.DateTimeMisMatchException();
            }
            foreach(Appointment obj in EventData.meetingData){
                if(obj.Id==eventData.Id){
                    continue;
                }
                if((eventData.StartTime<obj.EndTime)&&(obj.StartTime<eventData.EndTime)){
                    throw new CustomExceptions.MeetingOverLapException();
                }   
            }
            return Task.FromResult(_appointmentDAL.updateAppointment(eventData)); 
            
        }
      
        public Task<List<Appointment>> getAppointmentsByRange(DateTime range){
            return Task.FromResult(_appointmentDAL.GetAppointmentsByRange(range));
        }
        public bool delete(DateTime startTime){
            bool IsDeleted=_appointmentDAL.delete(startTime);
            if(IsDeleted){
                return true; 
            }
            return false;
        }
        public List<string> getHolidays(string date){
            List<string> holidayList= new List<string>(){};
            if(EventData.holidays.ContainsKey(date)){
                return _appointmentDAL.HolidayData(date);
            }
            else{
                return holidayList;
            }
        }

    }
}