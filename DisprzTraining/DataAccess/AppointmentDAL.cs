using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DisprzTraining.Models;
using DisprzTraining.Data;
namespace DisprzTraining.DataAccess
{
    public class AppointmentDAL : IAppointmentDAL
    {

        public Task<bool> CreateAppointment(Appointment eventdata)
        { 
            EventData.meetingData.Add(eventdata);
            EventData.count++;
            EventData.meetingData=EventData.meetingData.Where(p=>p.StartTime!=null).OrderBy(p=>p.StartTime).ToList();
            return Task.FromResult(true);   
        }

        public Task<List<Appointment>> GetAppointments()
        {
            return Task.FromResult(EventData.meetingData);
        }
        public bool DeleteAppointment(Guid id){
            
            if(EventData.meetingData.Any()){
                foreach(Appointment obj in EventData.meetingData){
                    if (obj.Id==id){
                        EventData.meetingData.Remove(obj);
                        EventData.count--;
                        return true;
                    }
                }
            }
            return false;
            
        }
        public List<Appointment> SearchAppointment(string name){
            List<Appointment> TargetData=new List<Appointment>();
            if(EventData.meetingData.Any()){
                foreach(Appointment obj in EventData.meetingData){
                    if(obj.EventName.ToLower().Contains(name.ToLower())){
                        TargetData.Add(obj);
                    }
                }
            }
            return (TargetData);
        }
        public bool updateAppointment(Appointment eventData){
            
            foreach(Appointment obj in EventData.meetingData){
                if(obj.Id==eventData.Id && obj.EventDate==eventData.EventDate){
                    obj.EventName=eventData.EventName;
                    obj.StartTime=eventData.StartTime;
                    obj.EndTime=eventData.EndTime;
                    obj.EventDate=eventData.EventDate;
                    obj.EventDescription=eventData.EventDescription;
                    return true;
                }
            }
            return false;
        }
        public List<Appointment> GetAppointmentsByRange(DateTime range){
            List<Appointment> rangeData=new List<Appointment>();
            foreach(Appointment obj in EventData.meetingData){
                if(obj.EndTime<range){
                    rangeData.Add(obj);
                }
                else if(obj.StartTime>range){
                    break;
                }
            }
            return rangeData;
        }
        public bool delete(DateTime startTime){
            int start=0;
            int end=EventData.meetingData.Count;
            while(start<end){
                int mid=(start+end)/2;
                if(EventData.meetingData[mid].StartTime==startTime){
                    EventData.meetingData.Remove(EventData.meetingData[mid]);
                    return true;
                }
                else if(EventData.meetingData[mid].StartTime>startTime){
                    end=mid;
                }
                else{
                    start=mid;
                }
            }
            return false;
        }
        public List<string> HolidayData(string date){
            return EventData.holidays[date];
        }
       
    }
}