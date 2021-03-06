﻿using System;
using System.Collections.Generic;
using AppointmentCalendar.Service.DTOs;

namespace AppointmentCalendar.Service.Contracts
{
    public interface IAppointmentsService
    {
        AppointmentDTO GetById(int id);
        void Add(AppointmentDTO appointment);
        AppointmentDTO Update(AppointmentDTO appointment);
        void Delete(int  Id);
        IEnumerable<AppointmentDTO> List(DateTime? dateFrom, DateTime? dateTo);
    }
}
