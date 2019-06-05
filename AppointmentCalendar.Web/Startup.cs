using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using AppointmentCalendar.Repository.Contexts;
using AppointmentCalendar.Repository.Contracts;
using AppointmentCalendar.Repository.Implementations;
using AppointmentCalendar.Service.Contracts;
using AppointmentCalendar.Service.Implementations;
using AppointmentCalendar.Web.Models;
using AppointmentCalendar.Web.Validators;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppointmentCalendar.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services
                .AddAutoMapper()
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddFluentValidation();

            string migrationAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            //services.AddDbContext<AppointmentContext>(options =>
            //    options.UseSqlServer(Configuration.GetConnectionString("AppointmentDatabase"),
            //        sql => sql.MigrationsAssembly(migrationAssembly)));

            services.AddDbContext<AppointmentContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("AppointmentDatabase")));

            services.AddTransient<IValidator<NewAppointmentViewModel>, NewAppointmentValidator>();
            services.AddTransient<IValidator<AppointmentViewModel>, AppointmentValidator>();

            services.AddTransient<IAppointmentsService, AppointmentServices>();

            services.AddTransient<IAppointmentRepository, AppointmentRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            IList<CultureInfo> supportedCultures = new List<CultureInfo>
            {
                new CultureInfo("en-gb"),
            };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-gb"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Appointments}/{action=Index}/{year?}/{month?}");
            });
        }
    }
}
