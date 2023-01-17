using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DisprzTraining.Models;

namespace DisprzTraining.Business
{
    public interface IAppointmentBL
    {
        public Task<bool> AppointmentTimeMatchCheck(Appointment appointment);
        public Task<bool> createAppointment(Appointment eventdata);
        public Task<List<Appointment>> getAppoinments();
        public Task<Appointment> getAppointmentById(Guid id);
        public Task<List<Appointment>> getAppointmentByDate(string date);
        public Task<bool> DeleteAppointment(Guid id);
        public Task<List<Appointment>> SearchAppointmentByName(string name);
        public Task<bool> UpdateAppointment(Appointment appointment);
        public Task<List<Appointment>> getAppointmentsByRange(DateTime range);
        public bool delete(DateTime startTime);
        public List<string> getHolidays(string date);
    }
}