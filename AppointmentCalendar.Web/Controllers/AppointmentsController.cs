using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentCalendar.Service.Contracts;
using AppointmentCalendar.Service.DTOs;
using AppointmentCalendar.Web.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AppointmentCalendar.Web.Areas.Calendar.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentsService _appointmentService;
        private readonly IMapper _mapper;

        public AppointmentsController(IAppointmentsService appointmentService,
                                        IMapper mapper)
        {
            _appointmentService = appointmentService;
            _mapper = mapper;
        }

        public IActionResult Index(int? year, int? month)
        {
            if (year.HasValue && (year < DateTime.MinValue.Year || year > DateTime.MaxValue.Year))
            {
                ModelState.AddModelError("All","The requested year is invalid"); 
            }
            if (month.HasValue && (month < 1 || month > 12))
            {
                ModelState.AddModelError("All", "The requested month is invalid");
            }

            if (ModelState.IsValid)
            {

                var displayMonth = new DateTime(year ?? DateTime.Today.Year, month ?? DateTime.Today.Month, 1);

                var model = new CalendarViewModel()
                {
                    CurrentMonth = displayMonth
                };

                model.Appointments =
                    _mapper.Map<IEnumerable<AppointmentViewModel>>(_appointmentService.List(model.DisplayStartDate,
                        model.DisplayEndDate));
                return View(model);
            }

            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_Create", new NewAppointmentViewModel() { StartDate = DateTime.Today, EndDate = DateTime.Today });
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewAppointmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_Create", model);
            }

            AddAppointment(model);

            return PartialView("_Create", model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var appointment = _mapper.Map<AppointmentViewModel>(_appointmentService.GetById(id));
            return PartialView("_Edit", appointment);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AppointmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_Edit", model);
            }

            UpdateAppointment(model);

            return PartialView("_Edit", model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            _appointmentService.Delete(id);

            return RedirectToAction("Index");
        }

        private void AddAppointment(NewAppointmentViewModel appointment)
        {
            _appointmentService.Add(_mapper.Map<AppointmentDTO>(appointment));
        }

        private void UpdateAppointment(AppointmentViewModel appointment)
        {
            _appointmentService.Update(_mapper.Map<AppointmentDTO>(appointment));
        }

        private void DeleteAppointment(int Id)
        {
            _appointmentService.Delete(Id);
        }
    }
}