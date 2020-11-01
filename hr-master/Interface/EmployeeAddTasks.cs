using hr_master.Db;
using hr_master.Filter;
using hr_master.Models;
using hr_master.Models.Dto;
using hr_master.Models.Form;
using hr_master.Models.ResponseDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace hr_master.Interface
{
    public class EmployeeAddTasks : IEmployeeAddTasks
    {
        private readonly Context _context;
        private readonly IConfiguration _configuration;
        private bool disposing;
        private bool _disposed;

        public EmployeeAddTasks(
           Context context,
           IConfiguration configuration
           )
        {
            _context = context;
            _context = context;
            _configuration = configuration;

        }



        public EmployeeAddTasks()
        {
        }

      


        void IEmployeeAddTasks.InsertTask(Tasks Taskform)
        {
            _context.Add(Taskform);

            var parts = _context.Teams.Where(x => x.Id == Taskform.Task_part).FirstOrDefault();
            var noitictioneform = new NotificationsForm
            {

                contents = "تم اضافة تاسك جديد",
                headings = "يرجى الانتباه",
                url = "http://sys.center-wifi.com",
                included_segments = parts.Team_Roles,


            };

            _ = SendNoitications(noitictioneform);
        }

    
    

        public void Save()
        {
            _context.SaveChanges();
        }

    

        public void Dispose()
        {
            if (!_disposed)
                if (disposing)
                    _context.Dispose();
            _disposed = true;
        }


        private async Task<bool> SendNoitications(NotificationsForm form)
        {

            var client = new RestClient(_configuration["onesginel:Url"]);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", _configuration["onesginel:Authorization"]);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Cookie", "__cfduid=d8a2aa2f8395ad68b8fd27b63127834571600976869");

            //request.AddParameter("application/json", "{\r\n\"app_id\" : \"b7d2542a-824a-4afa-9389-08880920baa8\",\r\n\"contents\" : {\"en\" : \"تم اضافة تاسك جديد\"},\r\n\"headings\" : {\"en\" : \"يرجى الانتباة\"},\r\n\"url\" : \"http://wifihr.tatwer.tech\",\r\n\"included_segments\" : [\"All\"]\r\n}", ParameterType.RequestBody);


            try {


                var body = new
                {
                    app_id = _configuration["onesginel:app_id"],
                    contents = new { en = form.contents },
                    headings = new { en = form.headings },
                    url = form.url,
                    included_segments = new string[] { form.included_segments }
                };

                request.AddJsonBody(body);

                IRestResponse response = client.Execute(request);

                return true;


            }
            catch (Exception e)
            {
                var data = e.Message;
                return false;
            }

        }
    }
}
