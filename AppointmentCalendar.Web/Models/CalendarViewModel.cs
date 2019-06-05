using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppointmentCalendar.Web.Models
{
    public class CalendarViewModel
    {

        public IEnumerable<AppointmentViewModel> Appointments { get; set; }

        public DateTime CurrentMonth { get; set; }

        [NotMapped]
        public DateTime DisplayStartDate
        {
            get
            {
                DateTime monthStartDate = new DateTime(CurrentMonth.Year, CurrentMonth.Month, 1);
                return monthStartDate.AddDays(-(int) monthStartDate.DayOfWeek);
            }
        }

        [NotMapped]
        public DateTime DisplayEndDate
        {
            get
            {
                DateTime monthEndDate = new DateTime(CurrentMonth.Year, CurrentMonth.Month , 1).AddMonths(1).AddDays(-1);
                return monthEndDate.AddDays(7 - (int)monthEndDate.DayOfWeek);
            }
        }

    }
}
