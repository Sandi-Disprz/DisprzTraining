using DisprzTraining.Models;
namespace DisprzTraining.DataAccess
{
    public interface IAppointmentDAL
    {
        public bool CreateAppointment(string date,Appointment eventData);
        public List<Appointment> GetAppointments(string date);
        public bool DeleteAppointment(string date,DateTime startTime);
        public bool UpdateAppointment(string date,Appointment updateData);
        public List<string> HolidayData(string date);
        public List<Appointment> SearchEvents(string search,string type);
        public List<Appointment> EventsTimeRange(DateTime endRange);
    }
}