using AppointmentCalendar.Domain.Implementations;
using FluentValidation;

namespace AppointmentCalendar.Repository.Validators
{
    public class AppointmentValidator : AbstractValidator<Appointment>
    {
        public AppointmentValidator()
        {
            RuleFor(x => x.Location).NotEmpty().MaximumLength(255);
            RuleFor(x => x.Summary).NotEmpty().MaximumLength(255);
            RuleFor(x => x.EndDate).NotEmpty().GreaterThanOrEqualTo(y => y.StartDate);
        }
    }
}
