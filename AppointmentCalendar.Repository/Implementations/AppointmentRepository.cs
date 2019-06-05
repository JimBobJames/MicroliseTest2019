using AppointmentCalendar.Domain.Implementations;
using AppointmentCalendar.Repository.Contexts;
using AppointmentCalendar.Repository.Contracts;
using FluentValidation;

namespace AppointmentCalendar.Repository.Implementations
{
    public class AppointmentRepository :  Repository<Appointment, int>, IAppointmentRepository
    {
        public AppointmentRepository(AppointmentContext dbContext, IValidator<Appointment> validator) : base(dbContext, validator)
        {
        }
    }
}
