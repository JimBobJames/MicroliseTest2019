using AppointmentCalendar.Domain.Implementations;

namespace AppointmentCalendar.Repository.Contracts
{
    public interface IAppointmentRepository : IRepository<Appointment, int>
    {
    }
}
