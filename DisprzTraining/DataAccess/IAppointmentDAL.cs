using DisprzTraining.Models;

namespace DisprzTraining.DataAccess
{
    public interface IAppointmentDAL
    {
        public Task<bool> CreateAppointment(Appointment data);
        public  Task<List<Appointment>> GetAppointments();
        public bool DeleteAppointment(Guid id); 
        public List<Appointment> SearchAppointment(string name);
        public bool updateAppointment(Appointment eventData);
        public List<Appointment> GetAppointmentsByRange(DateTime range);
        public bool delete(DateTime startTime);
        public List<string> HolidayData(string date);
    }

}
