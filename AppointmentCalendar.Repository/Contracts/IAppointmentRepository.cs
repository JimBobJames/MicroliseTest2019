using System;
using System.Collections.Generic;
using System.Text;
using AppointmentCalendar.Domain.Implementations;

namespace AppointmentCalendar.Repository.Contracts
{
    public interface IAppointmentRepository : IRepository<Appointment, int>
    {
    }
}
