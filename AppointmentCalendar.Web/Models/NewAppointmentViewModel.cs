using System;
using System.ComponentModel.DataAnnotations;

namespace AppointmentCalendar.Web.Models
{
    public class NewAppointmentViewModel
    {
        [Display(Name = "Summary")]
        public string Summary { get; set; }

        [Display(Name = "Location")]
        public string Location { get; set; }

        [Display(Name="Start Date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
    }
}