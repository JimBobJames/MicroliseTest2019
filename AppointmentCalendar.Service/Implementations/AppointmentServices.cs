using System;
using System.Collections.Generic;
using System.Linq;
using AppointmentCalendar.Domain.Implementations;
using AppointmentCalendar.Repository.Contracts;
using AppointmentCalendar.Service.Contracts;
using AppointmentCalendar.Service.DTOs;
using AutoMapper;

namespace AppointmentCalendar.Service.Implementations
{
    public class AppointmentServices : IAppointmentsService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;

        public AppointmentServices(IAppointmentRepository appointmentRepository,
                                    IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
        }
        public AppointmentDTO GetById(int id)
        {
            return _mapper.Map<AppointmentDTO>(_appointmentRepository.GetById(id).Result);
        }

        public void Add(AppointmentDTO appointment)
        {
            _appointmentRepository.Create(_mapper.Map<Appointment>(appointment));
            return ;
        }

        public AppointmentDTO Update(AppointmentDTO appointment)
        {
            return _mapper.Map<AppointmentDTO>(_appointmentRepository.Update(_mapper.Map<Appointment>(appointment)));
        }

        public void Delete(int id)
        {
            _appointmentRepository.Delete(id);
        }

        public IEnumerable<AppointmentDTO> List(DateTime? dateFrom, DateTime? dateTo)
        {
            var minDate = dateFrom ?? DateTime.MinValue;
            var maxDate = dateTo ?? DateTime.MaxValue;

            return _mapper.Map<IEnumerable<AppointmentDTO>>( _appointmentRepository.GetAll().Where(x => (x.StartDate >= minDate) && (x.EndDate <= maxDate)).AsEnumerable());
        }
    }
}
