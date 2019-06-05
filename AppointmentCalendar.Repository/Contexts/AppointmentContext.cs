using System;
using System.Collections.Generic;
using System.Text;
using AppointmentCalendar.Domain;
using AppointmentCalendar.Domain.Implementations;
using Microsoft.EntityFrameworkCore;

namespace AppointmentCalendar.Repository.Contexts
{
    public class AppointmentContext : DbContext
    {
        public AppointmentContext(DbContextOptions<AppointmentContext> options) : base(options)
        {

        }
        public DbSet<Appointment> Appointments { get; set; }
    }
}
