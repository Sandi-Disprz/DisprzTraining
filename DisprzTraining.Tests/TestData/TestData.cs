using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DisprzTraining.Models;
namespace DisprzTraining.Tests.TestData
{
    public static  class TestData
    {
        public static List<Appointment> getAllAppoinmentsWithData()=>
            new(){
                 new(){
                    EventName="daily-standup",
                    StartTime=new DateTime(2022,01,03,12,39,12),
                    EndTime=new DateTime(2022,01,03,2,39,12),
                    EventDescription="Everyone should attend the meeting without absent",
                    EventDate="2022-01-03"
                },
                new(){
                    EventName="mentor-standup",
                    StartTime=new DateTime(2022,01,03,12,39,12),
                    EndTime=new DateTime(2022,01,03,2,39,12),
                    EventDescription="Mentie should attend",
                    EventDate="2022-01-03"
                }
            
            };
        public static List<Appointment> GetAllAppointments(){
            return new List<Appointment>(){};
        }
        public static Appointment empty(){
            return new Appointment();
        }
        public static Appointment GetAppointmentById(){
            return new Appointment(){
                Id=new Guid(),
                EventName="daily-standup",
                StartTime=new DateTime(2022,12,22,12,39,12),
                EndTime=new DateTime(2022,12,22,2,39,12),
                EventDescription="Everyone should attend the meeting without absent",
                EventDate="2022-01-03"
            };
        }
    }
}
    
