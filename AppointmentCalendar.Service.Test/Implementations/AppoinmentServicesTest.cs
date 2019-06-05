using System;
using System.Linq;
using System.Threading.Tasks;
using AppointmentCalendar.Domain.Implementations;
using AppointmentCalendar.Repository.Contracts;
using AppointmentCalendar.Service.Contracts;
using AppointmentCalendar.Service.DTOs;
using AppointmentCalendar.Service.Implementations;
using AppointmentCalendar.Service.Profile;
using AutoMapper;
using Moq;
using NUnit.Framework;

namespace AppointmentCalendar.Service.Test.Implementations
{
    public class AppointmentServicesTests
    {
        private Mock<IAppointmentRepository> _repository;
        private IMapper _mapper;
        private IAppointmentsService _service;


        [OneTimeSetUp]
        public void Init()
        {
            _repository = new Mock<IAppointmentRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ServiceProfile());
            });
            _mapper = config.CreateMapper();

            _repository.Setup(t => t.GetById(It.IsAny<int>())).Returns(Task.FromResult(new Appointment
            {
                Id = 1, Summary = "Test Appointment", Location = "Eastwood", StartDate = DateTime.Today,
                EndDate = DateTime.Today
            }));

            _repository.Setup(t => t.GetAll()).Returns((new Appointment[]
            {
                new Appointment()
                {
                    Id = 1, Summary = "Service for ABC0123VYW, Nigel ", Location = "Watford",
                    StartDate = new DateTime(2019, 6, 13), EndDate = new DateTime(2019, 6, 13)
                },
                new Appointment()
                {
                    Id = 2, Summary = "Service for BBC0123VYW, Bill ", Location = "Leeds",
                    StartDate = new DateTime(2019, 4, 7), EndDate = new DateTime(2019, 4, 7)
                },
                new Appointment()
                {
                    Id = 3, Summary = "Service for CBC0123VYW, Ted ", Location = "Nottingham",
                    StartDate = new DateTime(2019, 8, 12), EndDate = new DateTime(2019, 8, 12)
                },
                new Appointment()
                {
                    Id = 4, Summary = "Service for DBC0123VYW, Julian ", Location = "Grimbsy",
                    StartDate = new DateTime(2019, 4, 21), EndDate = new DateTime(2019, 4, 21)
                },
                new Appointment()
                {
                    Id = 5, Summary = "Service for EBC0123VYW, Andrew ", Location = "Kettering",
                    StartDate = new DateTime(2019, 2, 14), EndDate = new DateTime(2019, 2, 14)
                },
                new Appointment()
                {
                    Id = 6, Summary = "Service for FBC0123VYW, Tim ", Location = "Mansfield",
                    StartDate = new DateTime(2019, 3, 23), EndDate = new DateTime(2019, 3, 23)
                },
                new Appointment()
                {
                    Id = 7, Summary = "Service for GBC0123VYW, Peter ", Location = "Enfield",
                    StartDate = new DateTime(2019, 9, 27), EndDate = new DateTime(2019, 9, 27)
                },
                new Appointment()
                {
                    Id = 8, Summary = "Service for HBC0123VYW, Grant ", Location = "Windermere",
                    StartDate = new DateTime(2019, 10, 2), EndDate = new DateTime(2019, 10, 2)
                },
                new Appointment()
                {
                    Id = 9, Summary = "Service for IBC0123VYW, Henry ", Location = "Leek",
                    StartDate = new DateTime(2019, 6, 16), EndDate = new DateTime(2019, 6, 16)
                },
                new Appointment()
                {
                    Id = 10, Summary = "Service for JBC0123VYW, Jill ", Location = "Ashford",
                    StartDate = new DateTime(2019, 7, 3), EndDate = new DateTime(2019, 7, 3)
                },

            }).AsQueryable() );

            _service = new AppointmentServices(_repository.Object,_mapper);
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GivenAppointmentIdWhenRequestingByIdThenReturnsAppointmentDTO()
        {
            var appointment = _repository.Object.GetById(1).Result;

            var appointmentDto = _service.GetById(1);

            Assert.That(appointmentDto.GetType(), Is.EqualTo(typeof(AppointmentDTO)));
            Assert.That(appointment.Id, Is.EqualTo(appointmentDto.Id));
            Assert.That(appointment.Summary, Is.EqualTo(appointmentDto.Summary));
            Assert.That(appointment.Location, Is.EqualTo(appointmentDto.Location));
            Assert.That(appointment.StartDate, Is.EqualTo(appointmentDto.StartDate));
            Assert.That(appointment.EndDate, Is.EqualTo(appointmentDto.EndDate));

        }

        [Test]
        public void GivenNewAppointmentWhenAddingThenCreatesWithRepository()
        {
            _service.Add(new AppointmentDTO());

            _repository.Verify(x=>x.Create(It.IsAny<Appointment>()),Times.Once());
        }

        [Test]
        public void GivenEditedAppointmentWhenEditingThenUpdatesWithRepository()
        {
            _service.Update(new AppointmentDTO());

            _repository.Verify(x => x.Update(It.IsAny<Appointment>()), Times.Once());
        }

        [Test]
        public void GivenDeletedAppointmentWhenDeletingThenDeletesFromRepository()
        {
            _service.Delete(3);

            _repository.Verify(x => x.Delete(3), Times.Once());
        }

        [TestCase(null, null, 10)]
        [TestCase("1 January 2019", "30 June 2019", 6)]
        [TestCase("1 March 2019", "30 April 2019", 3)]
        [TestCase("1 July 2019", "30 November 2019", 4)]
        public void GivenDateRangeWhenFilteringThenReturnsFilteredDTOs(DateTime? dateFrom, DateTime? dateTo, int expectedResult)
        {
            var count = _service.List(dateFrom, dateTo).Count();

            Assert.That(count, Is.EqualTo(expectedResult));
        }
    }
}