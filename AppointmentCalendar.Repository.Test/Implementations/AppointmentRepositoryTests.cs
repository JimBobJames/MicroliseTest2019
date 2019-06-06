using System;
using System.Linq;
using AppointmentCalendar.Domain.Implementations;
using AppointmentCalendar.Repository.Contexts;
using AppointmentCalendar.Repository.Contracts;
using AppointmentCalendar.Repository.Exceptions;
using AppointmentCalendar.Repository.Implementations;
using AppointmentCalendar.Repository.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace AppointmentCalendar.Repository.Test.Implementations
{
    [TestFixture]
    public class AppointmentRepositoryTests
    {
        private AppointmentContext _dbContext;
        private IAppointmentRepository _repository;
        private IValidator<Appointment> _validator;

        [OneTimeSetUp]
        public void Init()
        {
            var builder = new DbContextOptionsBuilder<AppointmentContext>();
            builder.UseInMemoryDatabase("AppointmentCalendar");
            DbContextOptions<AppointmentContext>  options = builder.Options;
            _dbContext = new AppointmentContext(options);

            AddSampleDataToContext();

            _validator = new AppointmentValidator();

            _repository = new AppointmentRepository(_dbContext, _validator);
        }

        [TearDown]
        public void TearDown()
        {

        }

        private void AddSampleDataToContext()
        {
            _dbContext.Set<Appointment>().AddRange(new Appointment[]
            {
                new Appointment(){Id=1,Summary="Service for ABC0123VYW, Nigel ", Location = "Watford", StartDate = new DateTime(2019,6,13), EndDate = new DateTime(2019,6,13)},
                new Appointment(){Id=2,Summary="Service for BBC0123VYW, Bill ", Location = "Leeds", StartDate = new DateTime(2019,4,7), EndDate = new DateTime(2019,4,7)},
                new Appointment(){Id=3,Summary="Service for CBC0123VYW, Ted ", Location = "Nottingham", StartDate = new DateTime(2019,8,12), EndDate = new DateTime(2019,8,12)},
                new Appointment(){Id=4,Summary="Service for DBC0123VYW, Julian ", Location = "Grimbsy", StartDate = new DateTime(2019,4,21), EndDate = new DateTime(2019,4,21)},
                new Appointment(){Id=5,Summary="Service for EBC0123VYW, Andrew ", Location = "Kettering", StartDate = new DateTime(2019,2,14), EndDate = new DateTime(2019,2,14)},
                new Appointment(){Id=6,Summary="Service for FBC0123VYW, Tim ", Location = "Mansfield", StartDate = new DateTime(2019,3,23), EndDate = new DateTime(2019,3,23)},
                new Appointment(){Id=7,Summary="Service for GBC0123VYW, Peter ", Location = "Enfield", StartDate = new DateTime(2019,9,27), EndDate = new DateTime(2019,9,27)},
                new Appointment(){Id=8,Summary="Service for HBC0123VYW, Grant ", Location = "Windermere", StartDate = new DateTime(2019,10,2), EndDate = new DateTime(2019,10,2)},
                new Appointment(){Id=9,Summary="Service for IBC0123VYW, Henry ", Location = "Leek", StartDate = new DateTime(2019,6,16), EndDate = new DateTime(2019,6,16)},
                new Appointment(){Id=10,Summary="Service for JBC0123VYW, Jill ", Location = "Ashford", StartDate = new DateTime(2019,7,3), EndDate = new DateTime(2019,7,3)},
            });

            _dbContext.SaveChanges();
        }

        [Test]
        public void GivenSampleDatabaseWhenGettingAllAppointmentsReturnsAllRows()
        {


            var appointments = _repository.GetAll().ToListAsync().Result;
            var count = _dbContext.Appointments.Count();

            Assert.That(appointments.Count,Is.EqualTo(count));
        }

        [TestCase(1)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(7)]
        [TestCase(8)]
        [TestCase(10)]
        public void GivenSampleDatabaseWhenGettingExistingAppointmentResultsAppointment(int id)
        {
            var appointment = _repository.GetById(id).Result;

            Assert.That(appointment, Is.Not.Null);
            Assert.That(appointment.Id, Is.EqualTo(id));
        }

        [TestCase(-4)]
        [TestCase(-7)]
        [TestCase(-8)]
        [TestCase(100)]
        public void GivenSampleDatabaseWhenGettingNonExistingAppointmentResultsNull(int id)
        {
            var appointment = _repository.GetById(id).Result;

            Assert.That(appointment, Is.Null);
        }

        [TestCase("Jessica Holiday","Derby", 30, 30)]
        public void GivenSampleDatabaseWhenAddingValidAppointmentAppointmentIsAdded(string summary, string location, int startDateDayDiff, int endDateDayDiff)
        {
            DateTime startDate = DateTime.Today.AddDays(startDateDayDiff);
            DateTime endDate = DateTime.Today.AddDays(endDateDayDiff);

            //Indexes are not being autocreated
            int nextId = _repository.GetAll().Max(x => x.Id) + 1;
            _repository.Create(new Appointment( ){Id = nextId, Summary  = summary, Location = location, StartDate = startDate, EndDate = endDate} );

            var appointment = _repository.GetById(nextId);

            Assert.That(appointment, Is.Not.Null);
        }

        [Test]
        public void GivenSampleDatabaseWhenAddingInValidAppointmentAppointmentExceptionIsThrown()
        {
            int nextId = _repository.GetAll().Max(x => x.Id) + 1;
            Assert.Throws<ArgumentException>(() => { _repository.Create(new Appointment() {Id = nextId}); });
        }

        [Ignore("Failing in parallel testing")] 
        //[TestCase(1)]
        //[TestCase(3)]
        //[TestCase(4)]
        //[TestCase(7)]
        //[TestCase(8)]
        //[TestCase(10)]
        public void GivenSampleDatabaseWhenUpdatingValidAppointmentAppointmentIsUpdated(int id)
        {
            var appointment = _repository.GetAll().FirstOrDefault(x => x.Id == id);
            appointment.Summary = Guid.NewGuid().ToString();
            appointment.Location = Guid.NewGuid().ToString();
            appointment.StartDate = appointment.StartDate.AddDays(1);
            appointment.EndDate = appointment.EndDate.AddDays(1);

            //_repository.Update(new Appointment(){Id=id,Summary = summary, Location = location, StartDate = DateTime.Today.AddDays(1), EndDate = DateTime.Today.AddDays(1)}).Wait();

            var originalAppointment = _repository.Update(appointment);

            var updatedAppointment = _repository.GetById(id).Result;
            
            Assert.That(updatedAppointment.StartDate, Is.EqualTo(appointment.StartDate));
            Assert.That(updatedAppointment.EndDate, Is.EqualTo(appointment.EndDate));
            Assert.That(updatedAppointment.Summary, Is.EqualTo(appointment.Summary));
            Assert.That(updatedAppointment.Location, Is.EqualTo(appointment.Location));
        }

        [TestCase(1)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(7)]
        [TestCase(8)]
        [TestCase(10)]
        public void GivenSampleDatabaseWhenUpdatingInValidAppointmentAppointmentExceptionIsThrown(int id)
        {
            var appointment = _repository.GetAll().FirstOrDefault(x => x.Id == id);
            appointment.Summary = null;
            appointment.Location = null;
            appointment.StartDate = appointment.StartDate.AddDays(1);
            appointment.EndDate = appointment.StartDate.AddDays(-1);

            //_repository.Update(new Appointment(){Id=id,Summary = summary, Location = location, StartDate = DateTime.Today.AddDays(1), EndDate = DateTime.Today.AddDays(1)}).Wait();

            Assert.Throws<ArgumentException>(() => { _repository.Update(appointment); });

        }

        [Test]
        public void GivenSampleDatabaseWhenDeletingValidAppointmentAppointmentIsDeleted()
        {
            int nextId = _repository.GetAll().Max(x => x.Id) + 1;

            _repository.Create(new Appointment{Id= nextId, Summary = Guid.NewGuid().ToString(), Location = Guid.NewGuid().ToString(), StartDate = DateTime.Today, EndDate = DateTime.Today});

            _repository.Delete(nextId);

            var deletedAppointment = _repository.GetById(nextId).Result;

            Assert.That(deletedAppointment, Is.Null);
        }

        [Test]
        public void GivenSampleDatabaseWhenDeletingInValidAppointmentAppointmentExceptionIsThrown()
        {
            Assert.Throws<EntityNotFoundException>(() => { _repository.Delete(-1); });
        }
    }
}
