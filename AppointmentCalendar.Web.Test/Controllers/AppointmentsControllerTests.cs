using AppointmentCalendar.Service.Contracts;
using AutoMapper;
using Moq;
using NUnit.Framework;
using AppointmentCalendar.Web.Profile;
using AppointmentCalendar.Service.Implementations;
using AppointmentCalendar.Web.Areas.Calendar.Controllers;
using AppointmentCalendar.Service.DTOs;
using System;
using Microsoft.AspNetCore.Mvc;
using AppointmentCalendar.Web.Models;
using System.Linq;
using System.Threading.Tasks;
using AppointmentCalendar.Web.Validators;

namespace Tests
{
    [TestFixture]
    public class AppointmentsControllerTests
    {
        private IMapper _mapper;
        private Mock<IAppointmentsService> _service;
        private AppointmentsController _controller;

        private DayOfWeek _firstDayOfWeek = DayOfWeek.Sunday;

        [SetUp]
        public void Setup()
        {
            _service = new Mock<IAppointmentsService>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainProfile());
            });
            _mapper = config.CreateMapper();

            _service.Setup(t => t.GetById(It.IsAny<int>())).Returns(new AppointmentDTO
            {
                Id = 1,
                Summary = "Test Appointment",
                Location = "Eastwood",
                StartDate = DateTime.Today,
                EndDate = DateTime.Today
            });

            _service.Setup(t => t.List(It.IsAny<DateTime?>(), (It.IsAny<DateTime?>()))).Returns((new AppointmentDTO[]
            {
                new AppointmentDTO()
                {
                    Id = 1, Summary = "Service for ABC0123VYW, Nigel ", Location = "Watford",
                    StartDate = new DateTime(2019, 6, 13), EndDate = new DateTime(2019, 6, 13)
                },
                new AppointmentDTO()
                {
                    Id = 2, Summary = "Service for BBC0123VYW, Bill ", Location = "Leeds",
                    StartDate = new DateTime(2019, 4, 7), EndDate = new DateTime(2019, 4, 7)
                },
                new AppointmentDTO()
                {
                    Id = 3, Summary = "Service for CBC0123VYW, Ted ", Location = "Nottingham",
                    StartDate = new DateTime(2019, 8, 12), EndDate = new DateTime(2019, 8, 12)
                },
                new AppointmentDTO()
                {
                    Id = 4, Summary = "Service for DBC0123VYW, Julian ", Location = "Grimbsy",
                    StartDate = new DateTime(2019, 4, 21), EndDate = new DateTime(2019, 4, 21)
                },
                new AppointmentDTO()
                {
                    Id = 5, Summary = "Service for EBC0123VYW, Andrew ", Location = "Kettering",
                    StartDate = new DateTime(2019, 2, 14), EndDate = new DateTime(2019, 2, 14)
                },
                new AppointmentDTO()
                {
                    Id = 6, Summary = "Service for FBC0123VYW, Tim ", Location = "Mansfield",
                    StartDate = new DateTime(2019, 3, 23), EndDate = new DateTime(2019, 3, 23)
                },
                new AppointmentDTO()
                {
                    Id = 7, Summary = "Service for GBC0123VYW, Peter ", Location = "Enfield",
                    StartDate = new DateTime(2019, 9, 27), EndDate = new DateTime(2019, 9, 27)
                },
                new AppointmentDTO()
                {
                    Id = 8, Summary = "Service for HBC0123VYW, Grant ", Location = "Windermere",
                    StartDate = new DateTime(2019, 10, 2), EndDate = new DateTime(2019, 10, 2)
                },
                new AppointmentDTO()
                {
                    Id = 9, Summary = "Service for IBC0123VYW, Henry ", Location = "Leek",
                    StartDate = new DateTime(2019, 6, 16), EndDate = new DateTime(2019, 6, 16)
                },
                new AppointmentDTO()
                {
                    Id = 10, Summary = "Service for JBC0123VYW, Jill ", Location = "Ashford",
                    StartDate = new DateTime(2019, 7, 3), EndDate = new DateTime(2019, 7, 3)
                },

            }));

            _controller = new AppointmentsController(_service.Object, _mapper);
        }

        [Test]
        public void GivenNoYearOrMonthWhenCreatingMainPageReturnsCurrentMonthCalendar()
        {
            IActionResult result = _controller.Index(null, null);

            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = (ViewResult)result;

            Assert.That(viewResult.Model, Is.TypeOf<CalendarViewModel>());
            var model = (CalendarViewModel)viewResult.Model;

            Assert.That(model.Appointments.Count(), Is.EqualTo(10));
            Assert.That(model.CurrentMonth.Month, Is.EqualTo(DateTime.Today.Month));
            Assert.That(model.CurrentMonth.Year, Is.EqualTo(DateTime.Today.Year));

        }

        [TestCase(2019,1)]
        [TestCase(2019, 5)]
        [TestCase(2019, 8)]
        [TestCase(2019, 12)]
        public void GivenYearAndMonthWhenCreatingMainPageReturnsChosenMonthCalendar(int year, int month)
        {
            IActionResult result = _controller.Index(year, month);

            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = (ViewResult)result;

            Assert.That(viewResult.Model, Is.TypeOf<CalendarViewModel>());
            var model = (CalendarViewModel)viewResult.Model;

            Assert.That(model.CurrentMonth.Year, Is.EqualTo(year));
            Assert.That(model.CurrentMonth.Month, Is.EqualTo(month));

            Assert.That(model.DisplayStartDate, Is.LessThanOrEqualTo(new DateTime(model.CurrentMonth.Year, model.CurrentMonth.Month, 1)));
            Assert.That(model.DisplayEndDate, Is.GreaterThanOrEqualTo(new DateTime(model.CurrentMonth.Year, model.CurrentMonth.Month, 1).AddMonths(1).AddDays(-1)));
            Assert.That(((model.DisplayEndDate - model.DisplayStartDate).Days ) %7, Is.EqualTo(0) );
            Assert.That(model.DisplayStartDate.DayOfWeek, Is.EqualTo(_firstDayOfWeek));
            Assert.That(model.DisplayEndDate.DayOfWeek, Is.EqualTo(_firstDayOfWeek));
        }

        [TestCase(2019, -8)]
        [TestCase(2019, 13)]
        [TestCase(-1, 13)]
        public void GivenInvalidYearAndMonthWhenCreatingMainPageReturnsChosenMonthCalendar(int year, int month)
        {
            IActionResult result = _controller.Index(year, month);

            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = (ViewResult)result;

            Assert.That(viewResult.ViewData.ModelState.IsValid, Is.EqualTo(false));
        }

        [Test]
        public void GivenCalendarWhenCreatingNewAppointmentThenDatesAreToday()
        {
            IActionResult result = _controller.Create();

            Assert.That(result, Is.TypeOf<PartialViewResult>());
            var viewResult = (PartialViewResult)result;

            Assert.That(viewResult.Model, Is.TypeOf<NewAppointmentViewModel>());
            var model = (NewAppointmentViewModel)viewResult.Model;

            Assert.That(model.StartDate, Is.EqualTo(DateTime.Today));
            Assert.That(model.EndDate, Is.EqualTo(DateTime.Today));
        }

        [Test]
        public void GivenNewAppointmentWhenInvalidSummaryLengthThenReturnsInvalid()
        {
            var model = new NewAppointmentViewModel() {Summary = new string('-', 256)};
            NewAppointmentValidator validator = new NewAppointmentValidator();

            FluentValidation.Results.ValidationResult results = validator.Validate(model);

            Assert.That(results.IsValid, Is.False);
            Assert.That(results.Errors.Any(x=>x.PropertyName == nameof(NewAppointmentViewModel.Summary)), Is.True);
        }

        [Test]
        public void GivenNewAppointmentWhenInvalidLocationLengthThenReturnsInvalid()
        {
            var model = new NewAppointmentViewModel() { Location = new string('-', 256) };
            NewAppointmentValidator validator = new NewAppointmentValidator();

            FluentValidation.Results.ValidationResult results = validator.Validate(model);

            Assert.That(results.IsValid, Is.False);
            Assert.That(results.Errors.Any(x => x.PropertyName == nameof(NewAppointmentViewModel.Location)), Is.True);
        }

        [Test]
        public void GivenNewAppointmentWhenEndDateBeforeStartDateThenReturnsInvalid()
        {
            var model = new NewAppointmentViewModel() { StartDate = DateTime.Today, EndDate = DateTime.Today.AddDays(-1)};
            NewAppointmentValidator validator = new NewAppointmentValidator();

            FluentValidation.Results.ValidationResult results = validator.Validate(model);

            Assert.That(results.IsValid, Is.False);
            Assert.That(results.Errors.Any(x => x.PropertyName == nameof(NewAppointmentViewModel.EndDate)), Is.True);
        }

        [Test]
        public void GivenNewValidAppoinmentWhenAddingThenSubmitsToService()
        {
            IActionResult result = _controller.Create(new NewAppointmentViewModel(){
                StartDate = DateTime.Today,
                EndDate = DateTime.Today,
                Location = "Location",
                Summary = "Summary"
            }).Result;

            _service.Verify(x => x.Add(It.IsAny<AppointmentDTO>()), Times.Once());
        }

        [Test]
        public void GivenSpecificAppointemntWhenEditingThenReturnsAppoinmentDetails()
        {
            IActionResult result = _controller.Edit(1);

            Assert.That(result, Is.TypeOf<PartialViewResult>());
            var viewResult = (PartialViewResult)result;

            Assert.That(viewResult.Model, Is.TypeOf<AppointmentViewModel>());
            var model = (AppointmentViewModel)viewResult.Model;

            var appointmentDto = _service.Object.GetById(1);

            Assert.That(model.Id, Is.EqualTo(appointmentDto.Id));
            Assert.That(model.Summary, Is.EqualTo(appointmentDto.Summary));
            Assert.That(model.Location, Is.EqualTo(appointmentDto.Location));
            Assert.That(model.StartDate, Is.EqualTo(appointmentDto.StartDate));
            Assert.That(model.EndDate, Is.EqualTo(appointmentDto.EndDate));
        }

        [Test]
        public void GivenAppointmentWhenInvalidSummaryLengthThenReturnsInvalid()
        {
            var model = new AppointmentViewModel() { Summary = new string('-', 256) };
            AppointmentValidator validator = new AppointmentValidator();

            FluentValidation.Results.ValidationResult results = validator.Validate(model);

            Assert.That(results.IsValid, Is.False);
            Assert.That(results.Errors.Any(x => x.PropertyName == nameof(AppointmentViewModel.Summary)), Is.True);
        }

        [Test]
        public void GivenAppointmentWhenInvalidLocationLengthThenReturnsInvalid()
        {
            var model = new AppointmentViewModel() { Location = new string('-', 256) };
            AppointmentValidator validator = new AppointmentValidator();

            FluentValidation.Results.ValidationResult results = validator.Validate(model);

            Assert.That(results.IsValid, Is.False);
            Assert.That(results.Errors.Any(x => x.PropertyName == nameof(AppointmentViewModel.Location)), Is.True);
        }

        [Test]
        public void GivenAppointmentWhenEndDateBeforeStartDateThenReturnsInvalid()
        {
            var model = new AppointmentViewModel() { StartDate = DateTime.Today, EndDate = DateTime.Today.AddDays(-1) };
            AppointmentValidator validator = new AppointmentValidator();

            FluentValidation.Results.ValidationResult results = validator.Validate(model);

            Assert.That(results.IsValid, Is.False);
            Assert.That(results.Errors.Any(x => x.PropertyName == nameof(AppointmentViewModel.EndDate)), Is.True);
        }

        [Test]
        public async Task GivenValidAppoinmentWhenAddingThenSubmitsToService()
        {
            IActionResult result = await _controller.Edit(new AppointmentViewModel()
            {
                Id = 1,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today,
                Location = "Location",
                Summary = "Summary"
            });

            _service.Verify(x => x.Update(It.IsAny<AppointmentDTO>()), Times.Once());
        }

        [Test]
        public void GivenAppoinmentWhenDeletingThenSubmitsToService()
        {
            IActionResult result = _controller.Delete(1).Result;

            _service.Verify(x => x.Delete(It.IsAny<int>()), Times.Once());
        }
    }
}