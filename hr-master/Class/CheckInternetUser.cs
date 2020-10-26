using hr_master.Db;
using hr_master.Models.Form;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Class
{
    public class CheckInternetUser
    {

        private readonly Context _context;
        private readonly IConfiguration _configuration;

        public CheckInternetUser(Context context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

        }


        public CheckInternetUser()
        {
        }




        public void checkInternetUserbefor3days()
        {


            var User = _context.InternetUsers.Where(x => x.IsDelete == false && x.IsActive == true).ToList();

            foreach (var i in User)
            {

                DateTime futurDate = Convert.ToDateTime(i.User_EndDate.Date);
                DateTime TodayDate = DateTime.Now;
                var numberOfDays = (futurDate - TodayDate).TotalDays;


                if(numberOfDays == 3)
                {

                    var noitictioneform = new NotificationsForm
                    {

                        contents = "سينتهي الاشتراك بعد ثلاثة ايام" ,
                        headings = "يرجى الانتباه",
                        url = "http://sys.center-wifi.com",
                        //included_segments = "All",
                        include_external_user_ids = i.Id.ToString(),

                    };

                    _ = SendNoitications(noitictioneform);




                }

                if (numberOfDays == 2)
                {

                    var noitictioneform = new NotificationsForm
                    {

                        contents = "سينتهي الاشتراك بعد يومين",
                        headings = "يرجى الانتباه",
                        url = "http://sys.center-wifi.com",
                        //included_segments = "All",
                        include_external_user_ids = i.Id.ToString(),

                    };

                    _ = SendNoitications(noitictioneform);




                }



            }

     

        }



        public void checkInternetUserDeActive()
        {


            var _User = _context.InternetUsers.Where(x => x.IsDelete == false && x.IsActive == true).ToList();

            foreach (var i in _User)
            {

                DateTime todaydate = DateTime.Now.Date;
                if (i.User_EndDate == todaydate)
                {


                    var noitictioneform = new NotificationsForm
                    {

                        contents = "اتتهاء صلاحية اشتراكك يرجى التفعيل",
                        headings = "يرجى الانتباه",
                        url = "http://sys.center-wifi.com",
                        //included_segments = "All",
                        include_external_user_ids =  i.Id.ToString(),

                    };

                    _ = SendNoitications(noitictioneform);


                    var InternetUsers = _context.InternetUsers.Where(x => x.Id == i.Id).FirstOrDefault();




                    InternetUsers.IsActive = false;
              


                    _context.Entry(InternetUsers).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.SaveChanges();



                }


            }



        }



        private string  SendNoitications(NotificationsForm form)
        {

            var client = new RestClient(_configuration["onesginelUsers:Url"]);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", _configuration["onesginelUsers:Authorization"]);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Cookie", "__cfduid=d8a2aa2f8395ad68b8fd27b63127834571600976869");

            var body = new
            {
                app_id = _configuration["onesginelUsers:app_id"],
                contents = new { en = form.contents },
                headings = new { en = form.headings },
                url = form.url,
                //included_segments = new string[] { form.included_segments },
                include_external_user_ids = new string[] { form.include_external_user_ids },

            };

            request.AddJsonBody(body);

            IRestResponse response = client.Execute(request);

            return (response.Content);
        }



    }
}
