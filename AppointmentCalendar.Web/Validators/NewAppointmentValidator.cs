using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentCalendar.Web.Models;
using FluentValidation;

namespace AppointmentCalendar.Web.Validators
{
    public class NewAppointmentValidator: AbstractValidator<NewAppointmentViewModel>
    {
        public NewAppointmentValidator()
        {
            RuleFor(x => x.Location).NotEmpty().MaximumLength(255);
            RuleFor(x => x.Summary).NotEmpty().MaximumLength(255);
            RuleFor(x => x.StartDate).NotEmpty();
            RuleFor(x => x.EndDate).NotEmpty().GreaterThanOrEqualTo(y=>y.StartDate).WithMessage("Date To must be greater than Date From");
        }
    }
}
