using System;
using System.Collections.Generic;
using System.Text;
using AppointmentCalendar.Domain.Implementations;
using AppointmentCalendar.Service.DTOs;

namespace AppointmentCalendar.Service.Profile
{
    public class ServiceProfile : AutoMapper.Profile
    {
        public ServiceProfile()
        {
            CreateMap<AppointmentDTO, Appointment>();
            CreateMap<Appointment, AppointmentDTO>();
        }
    }
}
