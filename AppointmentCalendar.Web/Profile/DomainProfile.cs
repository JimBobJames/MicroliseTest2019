using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentCalendar.Service.DTOs;
using AppointmentCalendar.Web.Models;

namespace AppointmentCalendar.Web.Profile
{
    public class DomainProfile : AutoMapper.Profile
    {
        public DomainProfile()
        {
            CreateMap<NewAppointmentViewModel, AppointmentDTO>()
                .ForMember(x=>x.Id,s=>s.Ignore());

            CreateMap<AppointmentViewModel, AppointmentDTO>();

            CreateMap<AppointmentDTO, AppointmentViewModel>();
        }
    }
}
