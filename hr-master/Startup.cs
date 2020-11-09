using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.MySql.Core;
using hr_master.Class;
using hr_master.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;


namespace hr_master
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



            //services.AddControllers();
            services.AddControllers().AddNewtonsoftJson();
            services.AddMvc().AddWebApiConventions();
           
            services.AddSignalR();
            services.AddDbContext<Db.Context>(options =>
            options.UseMySql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddHangfire(cfg =>
            {
                cfg.UseStorage(
         new MySqlStorage(Configuration["hangfiredatabase:hangfiredb"],
         new MySqlStorageOptions
        {
            TransactionIsolationLevel = IsolationLevel.ReadCommitted,
            QueuePollInterval = TimeSpan.FromSeconds(15),
            JobExpirationCheckInterval = TimeSpan.FromHours(1),
            CountersAggregateInterval = TimeSpan.FromMinutes(5),
            PrepareSchemaIfNecessary = true,
            DashboardJobListLimit = 50000,
            TransactionTimeout = TimeSpan.FromMinutes(1),
            TablePrefix = "wifiHangfire"
        }));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "My API", Version = "v1" });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });
            services.AddScoped<IUserConnectionManager, UserConnectionManager>();
            services.AddScoped<IEmployeeAddTasks, EmployeeAddTasks>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(jwtBearerOptions =>
            {
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateActor = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "Issuer",
                    ValidAudience = "Audience",
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes("wwBI_HtXqI3UgQHQ_rDRnSQRxFL1SR8fbQoS-Hsau1"))
                };
            });
            services.AddMvc(option => option.EnableEndpointRouting = false)
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddCors(o => o.AddPolicy("cross", builder =>
            {
                builder.WithOrigins(

                    "http://localhost:8080",
                    "http://localhost:8081",
                    "http://localhost",    
                    "http://wifi.tatwer.tech",
                    "http://wifihr.tatwer.tech",
                    "https://sys.center-wifi.com",
                    "https://backent.center-wifi.com",
                    "http://localhost:4200"

)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            }));
        
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseStaticFiles();
            app.UseHangfireDashboard();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            
            app.UseAuthentication();
            RecurringJob.AddOrUpdate(() => new AbsenceClass().checkAbsenceClass(), "0 20 * * *");
            RecurringJob.AddOrUpdate(() => new AbsenceClass().checkAbsenceClass1(), "0 20 * * *");
            RecurringJob.AddOrUpdate(() => new CheckInternetUser().checkInternetUserbefor3days(), "0 20 * * *");
            RecurringJob.AddOrUpdate(() => new CheckInternetUser().checkInternetUserDeActive(), "0 20 * * *");
            app.UseHangfireServer(
            new BackgroundJobServerOptions
            {

                WorkerCount = 1
            });
            app.UseRouting();

            app.UseCors("cross");
            app.UseMvc(routes =>
            {
                routes.MapWebApiRoute(
                        name: "default",
                        template: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseHangfireDashboard("/hangfireadmin", new DashboardOptions
            {
                Authorization = new[] { new MyAuthorizationFilter() }
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                  endpoints.MapHub<NotificationHub>("/Notification");
                  endpoints.MapHub<NotificationUserHub>("/NotificationUserHub");
            });
            //app.UseEndpoints(endpoints =>
            //{

            //    endpoints.MapHub<NotificationHub>("/Notification");
            //    endpoints.MapHub<NotificationUserHub>("/NotificationUserHub");
            //});
        }


    }
}
