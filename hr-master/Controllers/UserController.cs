using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Formatting;
using System.Security.Claims;
using System.Threading.Tasks;
using hr_master.Db;
using hr_master.Filter;
using hr_master.Models;
using hr_master.Models.Dto;
using hr_master.Models.Form;
using hr_master.Models.ResponseDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace hr_master.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize(Roles = "InternetUser")]
    [EnableCors("cross")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly Context _context;
        private readonly IConfiguration _configuration;
        public UserController(Context context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


    


        [HttpGet]
        public ActionResult<IEnumerable<string>> GetUserInfo()
        {


            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var _clientid = Guid.Parse(currentUserId);

            var list = _context.InternetUsers.Where(x => x.Id == _clientid).ToList();

            return Ok(new Response
            {
                Message = "Done !",
                Data = list,
                Error = false
            });


        }



        [HttpPut]
        public ActionResult<IEnumerable<string>> Complain([FromBody] AddComplaint form)
        {

            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var _clientid = Guid.Parse(currentUserId);

            var internetuser = _context.InternetUsers.Where(x => x.Id == _clientid).FirstOrDefault();
            var conplain = new User_Complaint
            {
                InternetUser_Id = _clientid,
                Complain = form.Complain,
                Complain_Date = DateTime.Now,
                Complain_Status = 0

            } ;
            _context.User_Complaint.Add(conplain);
            _context.SaveChanges();


            var noitictioneform = new NotificationsForm
            {

                contents = "تم اضافة شكوى او رسالة من احد المشتركين",
                headings = "يرجى الانتباه",
                url = "http://tatwer.tech",
                included_segments = "Support",


            };

            _ = SendNoitications(noitictioneform);

            return Ok(new Response
            {
                Message = "Done !",
                Data = conplain,
                Error = false
            });


        }

        private IActionResult SendNoitications(NotificationsForm form)
        {

            var client = new RestClient(_configuration["onesginel:Url"]);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", _configuration["onesginel:Authorization"]);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Cookie", "__cfduid=d8a2aa2f8395ad68b8fd27b63127834571600976869");




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

            return Ok(response.Content);
        }






    }



}
