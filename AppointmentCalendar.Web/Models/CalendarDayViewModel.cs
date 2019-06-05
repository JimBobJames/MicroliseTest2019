using System;
using System.Collections.Generic;
using System.Linq;

namespace AppointmentCalendar.Web.Models
{
    public class CalendarDayViewModel : CalendarViewModel
    {
        public int OffSet { get; set; }

        public DateTime CurrentDate => DisplayStartDate.AddDays(OffSet);

        public int Date => CurrentDate.Day;

        public bool IsCurrentMonth => CurrentDate.Month == CurrentMonth.Month;

        public IEnumerable<AppointmentViewModel> TodaysAppointments =>
            Appointments.Where(x => x.StartDate <= CurrentDate && x.EndDate >= CurrentDate);

    }
}