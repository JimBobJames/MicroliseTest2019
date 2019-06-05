using System;
using System.ComponentModel.DataAnnotations;

namespace AppointmentCalendar.Web.Models
{
    public class AppointmentViewModel
    {
        public int Id { get; set; }
        public string Summary { get; set; }
        public string Location { get; set; }
        [Display(Name = "Start Date")]
        public DateTime StartDate  { get; set; }
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
    }
}