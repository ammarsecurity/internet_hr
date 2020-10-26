using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Formatting;
using System.Security.Claims;
using System.Threading.Tasks;
using GeoCoordinatePortable;
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

namespace hr_master.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize(Roles = "Support,Store,Technical,Account")]
    [EnableCors("cross")]
    [ApiController]
    public class FingerPrintController : ControllerBase
    {

        private readonly Db.Context _context;
        private readonly IConfiguration _configuration;
        public FingerPrintController(Db.Context context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        [HttpPut]
        public ActionResult<IEnumerable<string>> AddIn([FromBody] AddInOut form)

        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var time = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
            var time1 = time.AddHours(3);

            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var _clientid = Guid.Parse(currentUserId);
            //DateTime date = Convert.ToDateTime(DateTime.Now).Date;
            // var nextDay = form.In_Out_Date.AddDays(1);
            var employeeinfo = _context.EmployessUsers.Where(x => x.Id == _clientid).FirstOrDefault();
            var admininfo = _context.AdminUser.FirstOrDefault();

           var inoutinfo = _context.InOut.Where(x => x.EmplyeeId == _clientid && x.In_Out_Status == form.In_Out_Status && x.In_Out_Date.Date == time1.Date).ToList();
          
            if (inoutinfo.Count != 0)
            {

                return BadRequest(new Response
                {
                    Message = "لقد قمت بالصمة سابقا",
                    Data = "",
                    Error = true
                });

            }
            double sLatitude = Convert.ToDouble(admininfo.Company_Latitude);
            double sLongitude = Convert.ToDouble(admininfo.Company_Longitude);
            double eLatitude = Convert.ToDouble(form.Employee_Latitude);
            double eLongitude = Convert.ToDouble(form.Employee_Longitude);


            int distansm = 0;

            if ((sLatitude == eLatitude) && (sLongitude == eLongitude))
            {
                distansm = 0;
            }
            else
            {
                double theta = sLongitude - eLongitude;
                double dist = Math.Sin(deg2rad(sLatitude)) * Math.Sin(deg2rad(eLatitude)) + Math.Cos(deg2rad(sLatitude)) * Math.Cos(deg2rad(eLatitude)) * Math.Cos(deg2rad(theta));
                dist = Math.Acos(dist);
                dist = rad2deg(dist);
                dist = dist * 60 * 1.1515;
                var distans = ((dist * 1.609344).ToString()).Substring(0, 5);
                var ddistans = Convert.ToDouble(distans) * 1000;
                distansm = Convert.ToInt32(ddistans);
            }



            string time_fromDb = employeeinfo.Employee_In_Time;

            string fromDb = time1.Date.ToString("yyyy-MM-dd") + " " + time_fromDb;
            DateTime FromDB = DateTime.Parse(fromDb);

            DateTime DCurrent = DateTime.Parse(time1.ToString("yyyy-MM-dd HH:mm:ss"));


            TimeSpan diffResult = DCurrent.ToUniversalTime().Subtract(FromDB.ToUniversalTime());
            string x = diffResult.ToString();



            string nHour = x.Substring(0, 2);
            int xHour;
            
             if (!int.TryParse(nHour, out xHour))
                 xHour = 0;

            int xMinites = xHour * 60;

            string ntime =  x.Substring(3, 2);
            int xTime;
            if (!int.TryParse(ntime, out xTime))
                xTime = 0;

            xMinites += xTime;

            var late = _context.ScheduleDelayPenalties.ToList();
            decimal PenaltiesPrice = 0;
            foreach (var i in late)
            {
                if((i.formtime) <= xMinites && xMinites <= i.totime)
                {

                    PenaltiesPrice = i.PenaltiesPrice;


                    var AddPenalties = new Penalties
                    {
                        
                        Penalties_Date = time1.Date,
                        Penalties_Note = "تأخير" + xMinites + "دقيقة" ,
                        Penalties_Price = PenaltiesPrice ,
                        Employees_Id = _clientid ,
                        Penalties_Enterid = Guid.Parse("3ef34045-bbbb-49e6-880e-7e7bcb9c9a16") ,

                    };

                    _context.Penalties.Add(AddPenalties);

                    _context.SaveChanges();
                }

            }
           

         

            var AddInOut = new InOut
            {
               distance = distansm ,
               In_Out_Date = time1.Date,
               In_Out_Status = form.In_Out_Status,
               EmplyeeId = _clientid,
               Employee_Latitude = form.Employee_Latitude,
               Employee_Longitude = form.Employee_Longitude ,
               In_Out_Time = time1.ToString("hh:mm tt"),


            };

            _context.InOut.Add(AddInOut);

            _context.SaveChanges();


            _context.SaveChanges();
            return Ok(new Response
            {
                Message = "Done !",
                Data = "Thanks",
                Error = false
            });
        }



        [HttpPut]
        public ActionResult<IEnumerable<string>> AddOut([FromBody] AddInOut form)

        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var time = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
            var time1 = time.AddHours(3);

            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var _clientid = Guid.Parse(currentUserId);

           
    
            var employeeinfo = _context.EmployessUsers.Where(x => x.Id == _clientid).FirstOrDefault();
            var admininfo = _context.AdminUser.FirstOrDefault();

            var inoutinfo = _context.InOut.Where(x => x.EmplyeeId == _clientid && x.In_Out_Status == form.In_Out_Status && x.In_Out_Date == time1.Date).ToList();

            if (inoutinfo.Count != 0)
            {

                return BadRequest(new Response
                {
                    Message = "لقد قمت بالصمة سابقا",
                    Data = "",
                    Error = true
                });

            }


            double sLatitude = Convert.ToDouble(admininfo.Company_Latitude);
            double sLongitude = Convert.ToDouble(admininfo.Company_Longitude);
            double eLatitude = Convert.ToDouble(form.Employee_Latitude);
            double eLongitude = Convert.ToDouble(form.Employee_Longitude);


            int distansm = 0;

            if ((sLatitude == eLatitude) && (sLongitude == eLongitude))
            {
                distansm = 0;
            }
            else
            {
                double theta = sLongitude - eLongitude;
                double dist = Math.Sin(deg2rad(sLatitude)) * Math.Sin(deg2rad(eLatitude)) + Math.Cos(deg2rad(sLatitude)) * Math.Cos(deg2rad(eLatitude)) * Math.Cos(deg2rad(theta));
                dist = Math.Acos(dist);
                dist = rad2deg(dist);
                dist = dist * 60 * 1.1515;
                var distans = ((dist * 1.609344).ToString()).Substring(0, 5);
                var ddistans = Convert.ToDouble(distans) * 1000;
                distansm = Convert.ToInt32(ddistans);
            }



            string time_fromDb = employeeinfo.Employee_Out_Time;

            string fromDb = time1.Date.ToString("yyyy-MM-dd") + " " + time_fromDb;
            DateTime FromDB = DateTime.Parse(fromDb);

            DateTime DCurrent = DateTime.Parse(time1.ToString("yyyy-MM-dd HH:mm:ss"));


            TimeSpan diffResult = DCurrent.ToUniversalTime().Subtract(FromDB.ToUniversalTime());
            string x = diffResult.ToString();



            string nHour = x.Substring(0, 2);
            int xHour;

            if (!int.TryParse(nHour, out xHour))
                xHour = 0;

            int xMinites = xHour * 60;

            string ntime = x.Substring(3, 2);
            int xTime;
            if (!int.TryParse(ntime, out xTime))
                xTime = 0;

            xMinites += xTime;

            var late = _context.OverTime.ToList();
            decimal OverTimePrice = 0;
            foreach (var i in late)
            {
                if ((i.formtime) <= xMinites && xMinites <= i.totime)
                {

                    OverTimePrice = i.OverTimePrice;


                    var AddOverTimeRewards = new OverTimeRewards
                    {

                        OverTimeRewards_Date = DateTime.Now.Date,
                        OverTimeRewards_Note = "اضافة فوق وقت انتهاء العمل" + xMinites + "دقيقة",
                        OverTimeRewards_Price = i.OverTimePrice,
                        Employees_Id = _clientid,
                        OverTimeRewards_Enterid = Guid.Parse("3ef34045-bbbb-49e6-880e-7e7bcb9c9a16"),

                    };

                    _context.OverTimeRewards.Add(AddOverTimeRewards);

                    _context.SaveChanges();
                }

            }




            var AddInOut = new InOut
            {
                distance = distansm,
                In_Out_Date = time1.Date,
                In_Out_Status = form.In_Out_Status,
                EmplyeeId = _clientid,
                Employee_Latitude = form.Employee_Latitude,
                Employee_Longitude = form.Employee_Longitude,
                In_Out_Time = time1.ToString("hh:mm tt"),


            };

            _context.InOut.Add(AddInOut);

            _context.SaveChanges();


            _context.SaveChanges();
            return Ok(new Response
            {
                Message = "Done !",
                Data = "Thanks",
                Error = false
            });
        }






        private double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }


        private double rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }



        [AllowAnonymous]
        [HttpGet]
        public ActionResult<string> CompanreTime()
        {
            string time_fromDb = "08:54:00";

            string fromDb = DateTime.Now.Date.ToString("dd/MM/yyyy") + " " + time_fromDb;
            DateTime FromDB = DateTime.Parse(fromDb);

            DateTime DCurrent = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));


            TimeSpan diffResult = DCurrent.ToUniversalTime().Subtract(FromDB.ToUniversalTime());
            string x = diffResult.ToString();




            return x.Substring(0, 5);
        }

    }



}
