using System;
using System.Collections.Generic;
using System.Text;

namespace AppointmentCalendar.Service.DTOs
{
    public class AppointmentDTO
    {
        public int Id { get; set; }
        public string Summary { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
