using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using hr_master.Db;
using hr_master.Filter;
using hr_master.Interface;
using hr_master.Models;
using hr_master.Models.Dto;
using hr_master.Models.Form;
using hr_master.Models.ResponseDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace hr_master.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize(Roles = "Admin")]
    [EnableCors("cross")]
    [ApiController]
    public class AdminController : ControllerBase
    {

        private readonly Context _context;
        private readonly IConfiguration _configuration;
        private readonly IHubContext<NotificationHub> _notificationHubContext;
        private readonly IHubContext<NotificationUserHub> _notificationUserHubContext;
        private readonly IUserConnectionManager _userConnectionManager;
        private readonly IEmployeeAddTasks _employeeAddTasks;
        public AdminController(
            Context context,
            IConfiguration configuration,
            IHubContext<NotificationHub> notificationHubContext,
            IHubContext<NotificationUserHub> notificationUserHubContext,
            IUserConnectionManager userConnectionManager,
            IEmployeeAddTasks employeeAddTasks
            )
        {
            _context = context;
            _configuration = configuration;
            _notificationHubContext = notificationHubContext;
            _notificationUserHubContext = notificationUserHubContext;
            _userConnectionManager = userConnectionManager;
            _employeeAddTasks = employeeAddTasks;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> SendToSpecificUser(NotificationHubDto model)
        {
            var connections = _userConnectionManager.GetUserConnections(model.userId);
            if (connections != null && connections.Count > 0)
            {
                foreach (var connectionId in connections)
                {
                    await _notificationUserHubContext.Clients.Client(connectionId).SendAsync("sendToUser", model.articleHeading, model.articleContent);
                }
            }
            return Ok(new Response
            {
                Message = "Done !",
                Data = "",
                Error = false
            });
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Index(NotificationHubDto model)
        {
            await _notificationHubContext.Clients.All.SendAsync("sendToUser", model.articleHeading, model.articleContent);
            return Ok(new Response
            {
                Message = "Done !",
                Data = "",
                Error = false
            });
        }


        [HttpPost]
        public ActionResult<IEnumerable<string>> EditAdminUser([FromBody] EditUserAdmin form)
        {
            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var _clientid = Guid.Parse(currentUserId);



            if (!ModelState.IsValid)
                return BadRequest(ModelState);



            var adminuser = _context.AdminUser.Where(x => x.Id == _clientid).FirstOrDefault();




            adminuser.Company_Longitude = form.Company_Longitude;
            adminuser.Company_Latitude = form.Company_Latitude;
            adminuser.Delay_penalty = form.Delay_penalty;
            adminuser.User_Firstname = form.User_Firstname;
            adminuser.User_Mail = form.User_Mail;
            adminuser.User_Name = form.User_Name;
            adminuser.User_Password = form.User_Password;
            adminuser.User_Phone = form.User_Phone;
            


            _context.Entry(adminuser).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return Ok(new Response
            {
                Message = "Done !",
                Data = adminuser,
                Error = false
            });


        }


        [HttpGet]
        public ActionResult<IEnumerable<string>> GetAdminUserInfo()
        {
            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var _clientid = Guid.Parse(currentUserId);

            var list = _context.AdminUser.Where(x => x.Id == _clientid).FirstOrDefault();

            return Ok(new Response
            {
                Message = "Done !",
                Data = list,
                Error = false
            });


        }



        [HttpPut]
        public ActionResult<IEnumerable<string>> AddTeam([FromBody] AddTeam form)

        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);



            var Teams = _context.Teams.Where(x => x.Team_Name == form.Team_Name && x.IsDelete == false).FirstOrDefault();
            if(Teams != null)
            {
                return BadRequest(new Response
                {
                    Message = "اسم الفريق موجود مسبقا",
                    Data = "",
                    Error = true
                });

            }
            var NewTeam = new Teams
            {
             Team_Name = form.Team_Name,
             Team_Note = form.Team_Note,
             Team_Roles = form.Team_Roles,
             In_Time = form.In_Time,
             Out_Time = form.Out_Time,
            };

            _context.Teams.Add(NewTeam);

            _context.SaveChanges();
            return Ok(new Response
            {
                Message = "Done !",
                Data = NewTeam,
                Error = false
            });
        }



        [HttpPut]
        public ActionResult<IEnumerable<string>> AddRewards([FromBody] AddRewards form)

        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);



            var Teams = _context.RewardsTable.Where(x => x.RewardsInfo == form.RewardsInfo && x.IsDelete == false).FirstOrDefault();
            if (Teams != null)
            {
                return BadRequest(new Response
                {
                    Message = "اسم المكافئة موجود",
                    Data = "",
                    Error = true
                });

            }
            var Rewards = new RewardsTable
            {
                RewardsInfo = form.RewardsInfo,
                RewardsPrice = form.RewardsPrice
            };

            _context.RewardsTable.Add(Rewards);

            _context.SaveChanges();
            return Ok(new Response
            {
                Message = "Done !",
                Data = Rewards,
                Error = false
            });
        }


        [HttpGet]
        public ActionResult<IEnumerable<string>> GetAllRewards()
        {




            var list = _context.RewardsTable.Where(x => x.IsDelete == false).ToList();

            return Ok(new Response
            {
                Message = "Done !",
                Data = list,
                Error = false
            });


        }


        [HttpPost]
        public ActionResult<IEnumerable<string>> EditRewards([FromBody] AddRewards form, Guid RewardsId)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);



            var Rewards = _context.RewardsTable.Where(x => x.Id == RewardsId).FirstOrDefault();




            Rewards.RewardsInfo = form.RewardsInfo;
            Rewards.RewardsPrice = form.RewardsPrice;


            _context.Entry(Rewards).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return Ok(new Response
            {
                Message = "Done !",
                Data = Rewards,
                Error = false
            });


        }


        [HttpPost]
        public ActionResult<IEnumerable<string>> DeleteRewards([FromBody] Delete form, Guid RewardsId)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var Rewards = _context.RewardsTable.Where(x => x.Id == RewardsId).FirstOrDefault();




            Rewards.IsDelete = form.IsDelete;

            _context.Entry(Rewards).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return Ok(new Response
            {
                Message = "Done !",
                Data = Rewards,
                Error = false
            });


        }




        [HttpPut]
        public ActionResult<IEnumerable<string>> AddScheduleDelayPenalties([FromBody] AddScheduleDelayPenalties form)

        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var Penalties = new ScheduleDelayPenalties
            {
                formtime = form.formtime,
                totime =form.totime,
                PenaltiesPrice = form.PenaltiesPrice
            };

            _context.ScheduleDelayPenalties.Add(Penalties);

            _context.SaveChanges();
            return Ok(new Response
            {
                Message = "Done !",
                Data = Penalties,
                Error = false
            });
        }

        [HttpPut]
        public ActionResult<IEnumerable<string>> AddOverTime([FromBody] AddOverTime form)

        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var overtime = new OverTime
            {
                formtime = form.formtime,
                totime = form.totime,
                OverTimePrice = form.OverTimePrice
            };

            _context.OverTime.Add(overtime);

            _context.SaveChanges();
            return Ok(new Response
            {
                Message = "Done !",
                Data = overtime,
                Error = false
            });
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> GetScheduleDelayPenalties()
        {




            var list = _context.ScheduleDelayPenalties.Where(x => x.IsDelete == false).ToList();

            return Ok(new Response
            {
                Message = "Done !",
                Data = list,
                Error = false
            });


        }


        [HttpGet]
        public ActionResult<IEnumerable<string>> GetOverTime()
        {




            var list = _context.OverTime.Where(x => x.IsDelete == false).ToList();

            return Ok(new Response
            {
                Message = "Done !",
                Data = list,
                Error = false
            });


        }


        [HttpPost]
        public ActionResult<IEnumerable<string>> EditScheduleDelayPenalties([FromBody] AddScheduleDelayPenalties form, Guid PenaltiesId)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);



            var Penalties = _context.ScheduleDelayPenalties.Where(x => x.Id == PenaltiesId).FirstOrDefault();




            Penalties.PenaltiesPrice = form.PenaltiesPrice;
            Penalties.formtime = form.formtime;
            Penalties.totime = form.totime;


            _context.Entry(Penalties).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return Ok(new Response
            {
                Message = "Done !",
                Data = Penalties,
                Error = false
            });


        }


        [HttpPost]
        public ActionResult<IEnumerable<string>> EditOverTime([FromBody] AddOverTime form, Guid OverTimeId)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);



            var overtime = _context.OverTime.Where(x => x.Id == OverTimeId).FirstOrDefault();




            overtime.OverTimePrice = form.OverTimePrice;
            overtime.formtime = form.formtime;
            overtime.totime = form.totime;


            _context.Entry(overtime).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return Ok(new Response
            {
                Message = "Done !",
                Data = overtime,
                Error = false
            });


        }


        [HttpPost]
        public ActionResult<IEnumerable<string>> DeleteScheduleDelayPenalties([FromBody] Delete form, Guid PenaltiesId)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var Penalties = _context.ScheduleDelayPenalties.Where(x => x.Id == PenaltiesId).FirstOrDefault();




            Penalties.IsDelete = form.IsDelete;

            _context.Entry(Penalties).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return Ok(new Response
            {
                Message = "Done !",
                Data = Penalties,
                Error = false
            });


        }


        [HttpPost]
        public ActionResult<IEnumerable<string>> DeleteOverTime([FromBody] Delete form, Guid OverTimeId)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var overitem = _context.OverTime.Where(x => x.Id == OverTimeId).FirstOrDefault();




            overitem.IsDelete = form.IsDelete;

            _context.Entry(overitem).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return Ok(new Response
            {
                Message = "Done !",
                Data = overitem,
                Error = false
            });


        }



        [HttpPut]
        public async Task<ActionResult<IEnumerable<string>>> AddEmployeeAsync([FromForm] AddEmployee form)

        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var Teams = _context.EmployessUsers.Where(x => x.Employee_Name == form.Employee_Name).FirstOrDefault();
            if (Teams != null)
            {
                return BadRequest(new Response
                {
                    Message = "الموظف موجود مسبقا",
                    Data = "",
                    Error = true
                });

            }


            string Employeeiamge = "";
            if (form.Employee_image != null)
            {
                //uploading image
                var fileName = Guid.NewGuid() + Path.GetExtension(form.Employee_image.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "EmployeePhotos");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                path = Path.Combine(path, fileName);
                using var stream = new FileStream(path, FileMode.Create);
                await form.Employee_image.CopyToAsync(stream);
                Employeeiamge = fileName;
            }
            else
            {
                Employeeiamge = "avatar.png";
            }



         
            var NewEmploye = new EmployessUsers
            {
                Employee_Fullname = form.Employee_Fullname,
                Employee_Name = form.Employee_Name,
                Employee_Adress = form.Employee_Adress,
                Employee_Email = form.Employee_Email,
                Employee_Note = form.Employee_Note,
                Employee_Password = form.Employee_Password,
                Employee_Phone = form.Employee_Phone,
                Employee_Photo = Employeeiamge ,
                Employee_Saller = form.Employee_Saller,
                Employee_Team = form.Employee_Team,
                Registration_Data = form.Registration_Data,
                Employee_Out_Time = form.Employee_Out_Time,
                Employee_In_Time = form.Employee_In_Time
            };

            _context.EmployessUsers.Add(NewEmploye);

            _context.SaveChanges();







            return Ok(new Response
            {
                Message = "Done !",
                Data = NewEmploye,
                Error = false
            });
        }

   


        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetAllEmployee([FromQuery] PaginationFilter filter  , string Employee_Fullname)
        {

            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var totalRecords = await _context.EmployessUsers.Where(x => x.IsDelete == false).CountAsync();
            var list = (from employee in _context.EmployessUsers.Where(x => x.IsDelete == false)
                             join team in _context.Teams on employee.Employee_Team equals team.Id
                        select new EmployeeInfo
                        {
                            Id = employee.Id,
                            Employee_Fullname = employee.Employee_Fullname,
                            Employee_Name = employee.Employee_Name,
                            Employee_Password = employee.Employee_Password,
                            Employee_Team = team.Team_Name,
                            Employee_Adress = employee.Employee_Adress,
                            Employee_Email = employee.Employee_Email,
                            Employee_Note = employee.Employee_Note,
                            Employee_Phone = employee.Employee_Phone,
                            Employee_Photo = _configuration["var:EmployeePhoto"] +  employee.Employee_Photo,
                            Employee_Saller = employee.Employee_Saller,
                            Registration_Data = employee.Registration_Data,
                            Team_Id = team.Id,
                            Employee_In_Time = employee.Employee_In_Time,
                            Employee_Out_Time = employee.Employee_Out_Time


                        }).ToList();

            if (Employee_Fullname != null && Employee_Fullname != default)
                list = list.Where(s => s.Employee_Fullname.Contains(Employee_Fullname)).ToList();
            totalRecords = list.Count();
           

            return Ok(new PagedResponse<List<EmployeeInfo>>(
                list.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToList(),
                validFilter.PageNumber,
                validFilter.PageSize,
                totalRecords, 0, 0));


        }



        [HttpPost]
        public async Task<ActionResult<IEnumerable<string>>> EditEmployeeAsync([FromForm] EditEmployee form, Guid EmployeeId)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var Employee = _context.EmployessUsers.Where(x => x.Id == EmployeeId).FirstOrDefault();


            string EmployeePhoto = "";

            if (form.Employee_image != null)
            {
                //uploading image
                var fileName = Guid.NewGuid() + Path.GetExtension(form.Employee_image.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "EmployeePhotos");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                path = Path.Combine(path, fileName);
                using var stream = new FileStream(path, FileMode.Create);
                await form.Employee_image.CopyToAsync(stream);
                EmployeePhoto = fileName;
            }
            else
            {
                EmployeePhoto = Employee.Employee_Photo;
            }

            Employee.Employee_Fullname = form.Employee_Fullname;
            Employee.Employee_Name = form.Employee_Name;
            Employee.Employee_Email = form.Employee_Email;
            Employee.Employee_Note = form.Employee_Note;
            Employee.Employee_Password = form.Employee_Password;
            Employee.Employee_Phone = form.Employee_Phone;
            Employee.Employee_Saller = form.Employee_Saller;
            Employee.Employee_Team = form.Employee_Team;
            Employee.Registration_Data = form.Registration_Data;
            Employee.Employee_Adress = form.Employee_Adress;
            Employee.Employee_Photo = EmployeePhoto;
            Employee.Employee_Out_Time = form.Employee_Out_Time;
            Employee.Employee_In_Time = form.Employee_In_Time;


            _context.Entry(Employee).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return Ok(new Response
            {
                Message = "Done !",
                Data = Employee,
                Error = false
            });


        }

        [HttpPost]
        public ActionResult<IEnumerable<string>> DeleteEmployee([FromBody] Delete form, Guid EmployeeId)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var Employee = _context.EmployessUsers.Where(x => x.Id == EmployeeId).FirstOrDefault();




            Employee.IsDelete = form.IsDelete;

            _context.Entry(Employee).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return Ok(new Response
            {
                Message = "Done !",
                Data = Employee,
                Error = false
            });


        }


        [HttpGet]
        public ActionResult<IEnumerable<string>> GetAllTeams()
        {


            

            var list = _context.Teams.Where(x => x.IsDelete == false).ToList();

            return Ok(new Response
            {
                Message = "Done !",
                Data = list,
                Error = false
            });


        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> AllInternetUser()
        {


            var list = _context.InternetUsers.Where(x => x.IsDelete == false).ToList();



            return Ok(new Response
            {
                Message = "Done !",
                Data = list,
                Error = false
            });


        }

        [HttpPost]
        public ActionResult<IEnumerable<string>> EditTeams([FromBody] EditTeams form, Guid TeamsId)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

     

            var Team = _context.Teams.Where(x => x.Id == TeamsId).FirstOrDefault();




            Team.Team_Name = form.Team_Name;
            Team.Team_Note = form.Team_Note;
            Team.Team_Roles = form.Team_Roles;
            Team.In_Time = form.In_Time;
            Team.Out_Time = form.Out_Time;


            _context.Entry(Team).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return Ok(new Response
            {
                Message = "Done !",
                Data = Team,
                Error = false
            });


        }


        [HttpPost]
        public ActionResult<IEnumerable<string>> DeleteTeams([FromBody] Delete form, Guid TeamsId)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var Team = _context.Teams.Where(x => x.Id == TeamsId).FirstOrDefault();




            Team.IsDelete = form.IsDelete;

            _context.Entry(Team).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return Ok(new Response
            {
                Message = "Done !",
                Data = Team,
                Error = false
            });


        }



        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetEmployeesByTeamAsync([FromQuery] PaginationFilter filter , Guid TeamId)
        {

            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var totalRecords = await _context.EmployessUsers.Where(x => x.Employee_Team == TeamId && x.IsDelete == false).CountAsync();
            var list = (from employee in _context.EmployessUsers.Where(x => x.Employee_Team == TeamId && x.IsDelete == false).AsQueryable()
                        join team in _context.Teams on employee.Employee_Team equals team.Id
                        select new EmployeeInfo
                        {
                            Id = employee.Id,
                            Employee_Fullname = employee.Employee_Fullname,
                            Employee_Name = employee.Employee_Name,
                            Employee_Password = employee.Employee_Password,
                            Employee_Team = team.Team_Name,
                            Employee_Adress = employee.Employee_Adress,
                            Employee_Email = employee.Employee_Email,
                            Employee_Note = employee.Employee_Note,
                            Employee_Phone = employee.Employee_Phone,
                            Employee_Photo = _configuration["var:EmployeePhoto"] + employee.Employee_Photo,
                            Employee_Saller = employee.Employee_Saller,
                            Registration_Data = employee.Registration_Data,
                            Team_Id = team.Id


                        }).ToList();


            return Ok(new PagedResponse<List<EmployeeInfo>>(
                list.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToList(),
                validFilter.PageNumber,
                validFilter.PageSize,
                totalRecords, 0, 0));


        }



        [HttpPut]
        public ActionResult<IEnumerable<string>> AddPenalties([FromBody] AddPenalties form)

        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

        
            var NewPenalties = new Penalties
            {
               Penalties_Date = form.Penalties_Date,
               Penalties_Enterid = form.Penalties_Enterid,
               Penalties_Note = form.Penalties_Note,
               Penalties_Price = form.Penalties_Price,
               Employees_Id = form.Employees_Id,
            };

            _context.Penalties.Add(NewPenalties);

            _context.SaveChanges();
            return Ok(new Response
            {
                Message = "Done !",
                Data = NewPenalties,
                Error = false
            });
        }

     
        [HttpGet]
        public ActionResult<IEnumerable<string>> dashbardcounts()
        {
            String Month = DateTime.Now.Month.ToString();
            String Year = DateTime.Now.Year.ToString();
            DateTime date = Convert.ToDateTime(DateTime.Now).Date;

            var thismonthabsence = _context.Absence.Where(x => x.AbsenceDate.Month.ToString() == Month && x.AbsenceDate.Year.ToString() == Year).Count();



            var thisdayabsence = _context.Absence.Where(x => x.AbsenceDate.Date == date).Count();



            var thismounthOverTimeReawrad = _context.OverTimeRewards.Where(x => x.IsDelete == false && x.OverTimeRewards_Date.Month.ToString() == Month && x.OverTimeRewards_Date.Year.ToString() == Year).ToList();
            decimal OverTimeReawradSum = 0;
            foreach (var i in thismounthOverTimeReawrad)
            {

                OverTimeReawradSum += i.OverTimeRewards_Price;
            }




            var thismonthpenalties = _context.Penalties.Where(x => x.IsDelete == false && x.Penalties_Date.Month.ToString() == Month && x.Penalties_Date.Year.ToString() == Year).ToList();
            decimal penaltiesSum = 0;
            foreach (var i in thismonthpenalties)
            {

                penaltiesSum += i.Penalties_Price;
            }



            var teams = _context.Teams.Where(x => x.IsDelete == false).Count();
            var Employees = _context.EmployessUsers.Where(x => x.IsDelete == false).Count();
            var EmployessSallary = _context.EmployessUsers.Where(x => x.IsDelete == false).ToList();
            decimal Sallary = 0;
            foreach (var i in EmployessSallary)
            {

                Sallary += i.Employee_Saller;
            }

            var Expenseslist = _context.AccounterInputAndOutput.Where(x => x.IsDelete == false && x.InputAndOutput_Date.Month.ToString() == Month && x.InputAndOutput_Date.Year.ToString() == Year && x.InputAndOutput_Status == 0).ToList();
            decimal Expenses = 0;
            foreach (var i in Expenseslist)
            {

                Expenses += i.InputAndOutput_Price;
            }


            var Revenueslist = _context.AccounterInputAndOutput.Where(x => x.IsDelete == false && x.InputAndOutput_Date.Month.ToString() == Month && x.InputAndOutput_Date.Year.ToString() == Year && x.InputAndOutput_Status == 1).ToList();
            decimal Revenues = 0;
            foreach (var i in Revenueslist)
            {

                Revenues += i.InputAndOutput_Price;
            }

            var Counts = new AdminDashboardCounts {
                Teamcount = teams ,
                Employeescount = Employees ,
                SallerySum = Sallary ,
                 thisdayabsence = thisdayabsence ,
                thismonthabsence = thismonthabsence ,
                thismonthOverTimeReawradSum  = OverTimeReawradSum ,
                thismonthpenaltiesSun = penaltiesSum ,
                thismonthExpenses = Expenses,
                thismonthRevenues = Revenues
            };

            return Ok(new Response
            {
                Message = "ok",
                Data = Counts,
                Error = false
            });


        }

       
        [HttpPost]
        public async Task<IActionResult> UploadAttachment(List<IFormFile> files, Guid ItemId)
        {
            var AttachmentList = new List<Attachment>();
            if (files == null || files.Count == 0)
                return Content("file not selected");
            long size = files.Sum(f => f.Length);
            var filePaths = new List<string>();
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    // full path to file in temp location
                    var fileName = Guid.NewGuid() + Path.GetExtension(formFile.FileName);
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Attachments");
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    path = Path.Combine(path, fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }


                    var attachment = new Attachment
                    {

                        Attachment_Id = ItemId,
                        Attachment_Name = fileName
                    };
                    _context.Attachment.Add(attachment);
                    AttachmentList.Add(attachment);
                    _context.SaveChanges();
                }
            }


            return Ok(new Response
            {
                Message = "Done ! Count : " + AttachmentList.Count,
                Data = AttachmentList,
                Error = false
            });
        }



        [HttpGet]
        public ActionResult<IEnumerable<string>> GetAttachment(Guid ItemId)
        {


            var list = (from attachment in _context.Attachment.Where(x => x.IsDelete == false && x.Attachment_Id == ItemId).AsQueryable()
                        select new AttachmentDto
                        {
                            Attachment_Id = attachment.Attachment_Id,
                            Id = attachment.Id,
                            Attachment_Name = _configuration["var:Attachments"] + attachment.Attachment_Name,

                        }).ToList();



            return Ok(new Response
            {
                Message = "Done !",
                Data = list,
                Error = false
            });


        }



        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetMyAllTask([FromQuery] PaginationFilter filter, string Task_Employee_WorkOn, DateTime? date, string Task_Employee_Open, int? Task_Status)
        {


            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var _clientid = Guid.Parse(currentUserId);


            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var totalRecords = await _context.Tasks.Where(x => x.IsDelete == false && x.Task_Employee_Open == _clientid && x.Task_Status != 3).CountAsync();


            var list = (from task in _context.Tasks.OrderByDescending(x => x.Task_Date).Where(x => x.IsDelete == false && x.Task_Employee_Open == _clientid && x.Task_Status != 3).AsNoTracking()
                        join Employee1 in _context.EmployessUsers.AsNoTracking() on task.Task_Employee_Open equals Employee1.Id
                        join tower in _context.Towers.AsNoTracking() on task.Tower_Id equals tower.Id into vtower
                        from tower in vtower.DefaultIfEmpty()
                        join Employee in _context.EmployessUsers.AsNoTracking() on task.Task_Employee_WorkOn equals Employee.Id into vEmployee
                        from Employee in vEmployee.DefaultIfEmpty()
                        join Employee2 in _context.EmployessUsers.AsNoTracking() on task.Task_Employee_Close equals Employee2.Id into vEmployee2
                        from Employee2 in vEmployee2.DefaultIfEmpty()
                        join part in _context.Teams.AsNoTracking() on task.Task_part equals part.Id
                        join reward in _context.RewardsTable.AsNoTracking() on task.Task_Price_rewards equals reward.Id into vRewards
                        from reward in vRewards.DefaultIfEmpty()
                        join internetuser in _context.InternetUsers on task.InternetUserId equals internetuser.Id into vinternetuser
                        from internetuser in vinternetuser.DefaultIfEmpty()


                        select new TasksDto
                        {
                            Id = task.Id,
                            Task_Title = task.Task_Title,
                            Task_part = part.Team_Name,
                            Task_Status = task.Task_Status,
                            Task_closed_Note = task.Task_closed_Note,
                            Task_Date = task.Task_Date,
                            Task_Done = task.Task_Done,
                            Task_Employee_Close = Employee2.Employee_Fullname ?? "انتظار موظف",
                            Task_Employee_Open = Employee1.Employee_Fullname,
                            Task_Employee_WorkOn = Employee.Employee_Fullname ?? "انتظار موظف",
                            Task_EndDate = task.Task_EndDate,
                            Task_Note = task.Task_Note,
                            Task_Open = task.Task_Open,
                            Task_Price_rewards = task.Task_Price_rewards,
                            Tower_Name = tower.Tower_Name ?? "لايوجد",
                            Tower_Id = tower.Id,
                            part_Id = part.Id,
                            Task_Price = reward.RewardsPrice,
                            InternetUserId = internetuser.Id,
                            Task_Employee_WorkOn_id = task.Task_Employee_WorkOn,





                        }).ToList();

            if (Task_Employee_WorkOn != null && Task_Employee_WorkOn != default)
                list = list.Where(s => s.Task_Employee_WorkOn.Contains(Task_Employee_WorkOn)).ToList();
            totalRecords = list.Count();
            if (Task_Employee_Open != null && Task_Employee_Open != default)
                list = list.Where(s => s.Task_Employee_Open.Contains(Task_Employee_Open)).ToList();
            totalRecords = list.Count();
            if (date != null && date != default)
                list = list.Where(s => s.Task_Date.Date == date).ToList();
            totalRecords = list.Count();
            if (Task_Status != null && Task_Status != default)
                list = list.Where(s => s.Task_Status == Task_Status).ToList();
            totalRecords = list.Count();

            return Ok(new PagedResponse<List<TasksDto>>(
                list.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToList(),
                validFilter.PageNumber,
                validFilter.PageSize,
                totalRecords, 0, 0));


        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetAllTask([FromQuery] PaginationFilter filter, string Task_Employee_WorkOn, DateTime? date, string Task_Employee_Open, int? Task_Status)
        {




            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var totalRecords = await _context.Tasks.Where(x => x.IsDelete == false && x.Task_Status != 3).CountAsync();


            var list = (from task in _context.Tasks.OrderByDescending(x => x.Task_Date).Where(x => x.IsDelete == false &&  x.Task_Status != 3).AsNoTracking()
                        join Employee1 in _context.EmployessUsers.AsNoTracking() on task.Task_Employee_Open equals Employee1.Id
                        join tower in _context.Towers.AsNoTracking() on task.Tower_Id equals tower.Id into vtower
                        from tower in vtower.DefaultIfEmpty()
                        join Employee in _context.EmployessUsers.AsNoTracking() on task.Task_Employee_WorkOn equals Employee.Id into vEmployee
                        from Employee in vEmployee.DefaultIfEmpty()
                        join Employee2 in _context.EmployessUsers.AsNoTracking() on task.Task_Employee_Close equals Employee2.Id into vEmployee2
                        from Employee2 in vEmployee2.DefaultIfEmpty()
                        join part in _context.Teams.AsNoTracking() on task.Task_part equals part.Id
                        join reward in _context.RewardsTable.AsNoTracking() on task.Task_Price_rewards equals reward.Id into vRewards
                        from reward in vRewards.DefaultIfEmpty()
                        join internetuser in _context.InternetUsers on task.InternetUserId equals internetuser.Id into vinternetuser
                        from internetuser in vinternetuser.DefaultIfEmpty()


                        select new TasksDto
                        {
                            Id = task.Id,
                            Task_Title = task.Task_Title,
                            Task_part = part.Team_Name,
                            Task_Status = task.Task_Status,
                            Task_closed_Note = task.Task_closed_Note,
                            Task_Date = task.Task_Date,
                            Task_Done = task.Task_Done,
                            Task_Employee_Close = Employee2.Employee_Fullname ?? "انتظار موظف",
                            Task_Employee_Open = Employee1.Employee_Fullname,
                            Task_Employee_WorkOn = Employee.Employee_Fullname ?? "انتظار موظف",
                            Task_EndDate = task.Task_EndDate,
                            Task_Note = task.Task_Note,
                            Task_Open = task.Task_Open,
                            Task_Price_rewards = task.Task_Price_rewards,
                            Tower_Name = tower.Tower_Name,
                            Tower_Id = tower.Id,
                            part_Id = part.Id,
                            Task_Price = reward.RewardsPrice,
                            InternetUserId = internetuser.Id





                        }).ToList();

            if (Task_Employee_WorkOn != null && Task_Employee_WorkOn != default)
                list = list.Where(s => s.Task_Employee_WorkOn.Contains(Task_Employee_WorkOn)).ToList();
            totalRecords = list.Count();
            if (Task_Employee_Open != null && Task_Employee_Open != default)
                list = list.Where(s => s.Task_Employee_Open.Contains(Task_Employee_Open)).ToList();
            totalRecords = list.Count();
            if (date != null && date != default)
                list = list.Where(s => s.Task_Date.Date == date).ToList();
            totalRecords = list.Count();
            if (Task_Status != null && Task_Status != default)
                list = list.Where(s => s.Task_Status == Task_Status).ToList();
            totalRecords = list.Count();

            return Ok(new PagedResponse<List<TasksDto>>(
                list.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToList(),
                validFilter.PageNumber,
                validFilter.PageSize,
                totalRecords, 0, 0));


        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetAllDoneTask([FromQuery] PaginationFilter filter, string Task_Employee_WorkOn, DateTime? date, string Task_Employee_Open, int? Task_Status)
        {



            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var totalRecords = await _context.Tasks.Where(x => x.IsDelete == false && x.Task_Status == 3).CountAsync();


            var list = (from task in _context.Tasks.OrderByDescending(x => x.Task_Date).Where(x => x.IsDelete == false && x.Task_Status == 3).AsNoTracking()
                        join Employee1 in _context.EmployessUsers.AsNoTracking() on task.Task_Employee_Open equals Employee1.Id
                        join tower in _context.Towers.AsNoTracking() on task.Tower_Id equals tower.Id into vtower
                        from tower in vtower.DefaultIfEmpty()
                        join Employee in _context.EmployessUsers.AsNoTracking() on task.Task_Employee_WorkOn equals Employee.Id into vEmployee
                        from Employee in vEmployee.DefaultIfEmpty()
                        join Employee2 in _context.EmployessUsers.AsNoTracking() on task.Task_Employee_Close equals Employee2.Id into vEmployee2
                        from Employee2 in vEmployee2.DefaultIfEmpty()
                        join part in _context.Teams.AsNoTracking() on task.Task_part equals part.Id
                        join reward in _context.RewardsTable.AsNoTracking() on task.Task_Price_rewards equals reward.Id into vRewards
                        from reward in vRewards.DefaultIfEmpty()
                        join internetuser in _context.InternetUsers on task.InternetUserId equals internetuser.Id into vinternetuser
                        from internetuser in vinternetuser.DefaultIfEmpty()


                        select new TasksDto
                        {
                            Id = task.Id,
                            Task_Title = task.Task_Title,
                            Task_part = part.Team_Name,
                            Task_Status = task.Task_Status,
                            Task_closed_Note = task.Task_closed_Note,
                            Task_Date = task.Task_Date,
                            Task_Done = task.Task_Done,
                            Task_Employee_Close = Employee2.Employee_Fullname ?? "انتظار موظف",
                            Task_Employee_Open = Employee1.Employee_Fullname,
                            Task_Employee_WorkOn = Employee.Employee_Fullname ?? "انتظار موظف",
                            Task_EndDate = task.Task_EndDate,
                            Task_Note = task.Task_Note,
                            Task_Open = task.Task_Open,
                            Task_Price_rewards = task.Task_Price_rewards,
                            Tower_Name = tower.Tower_Name ?? "لايوجد",
                            Tower_Id = tower.Id,
                            part_Id = part.Id,
                            Task_Price = reward.RewardsPrice,
                            InternetUserId = internetuser.Id,
                            Task_Employee_WorkOn_id = task.Task_Employee_WorkOn





                        }).ToList();

            if (Task_Employee_WorkOn != null && Task_Employee_WorkOn != default)
                list = list.Where(s => s.Task_Employee_WorkOn.Contains(Task_Employee_WorkOn)).ToList();
            totalRecords = list.Count();
            if (Task_Employee_Open != null && Task_Employee_Open != default)
                list = list.Where(s => s.Task_Employee_Open.Contains(Task_Employee_Open)).ToList();
            totalRecords = list.Count();
            if (date != null && date != default)
                list = list.Where(s => s.Task_Date.Date == date).ToList();
            totalRecords = list.Count();
            if (Task_Status != null && Task_Status != default)
                list = list.Where(s => s.Task_Status == Task_Status).ToList();
            totalRecords = list.Count();

            return Ok(new PagedResponse<List<TasksDto>>(
                list.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToList(),
                validFilter.PageNumber,
                validFilter.PageSize,
                totalRecords, 0, 0));


        }


        [HttpPut]
        public ActionResult<IEnumerable<string>> AddfollowerTask([FromBody] AddTaskFollowers form)

        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var followers = _context.TaskFollowers.Where(x => x.EmployeeId == form.EmployeeId && x.TaskId == form.TaskId).FirstOrDefault();
            if (followers != null)
            {

                return BadRequest(new Response
                {
                    Message = "Erorr !",
                    Data = followers,
                    Error = true
                });

            }

            var Newfollower = new TaskFollowers
            {
                EmployeeId = form.EmployeeId,
                TaskId = form.TaskId,

            };

            _context.TaskFollowers.Add(Newfollower);

            _context.SaveChanges();
            return Ok(new Response
            {
                Message = "Done !",
                Data = Newfollower,
                Error = false
            });
        }


        [HttpGet]
        public ActionResult<IEnumerable<string>> GetTaskFollowers(Guid TaskId)
        {


            var list = (from taskfollor in _context.TaskFollowers.Where(x => x.TaskId == TaskId)
                        join employee in _context.EmployessUsers on taskfollor.EmployeeId equals employee.Id
                        select new TaskFollowersDto
                        {

                            Employee_FullName = employee.Employee_Fullname,
                            Employee_Photo = _configuration["var:EmployeePhoto"] + employee.Employee_Photo,
                            Id = taskfollor.Id

                        }




                        ).ToList();



            return Ok(new Response
            {
                Message = "Done !",
                Data = list,
                Error = false
            });


        }


        [HttpGet]
        public ActionResult<IEnumerable<string>> GetEmployeeBySelectTeam(Guid TeamId)
        {


            var Team = _context.Teams.Where(x => x.Id == TeamId).FirstOrDefault();
            var employee = _context.EmployessUsers.Where(x => x.Employee_Team == Team.Id).ToList();




            return Ok(new Response
            {
                Message = "Done !",
                Data = employee,
                Error = false
            });


        }



            [HttpGet]
            public ActionResult<IEnumerable<string>> GetEmployeeByTeam(Guid TaskId)
            {


                var task = _context.Tasks.Where(x => x.Id == TaskId).FirstOrDefault();
                var employee = _context.EmployessUsers.Where(x => x.Employee_Team == task.Task_part).ToList();




                return Ok(new Response
                {
                    Message = "Done !",
                    Data = employee,
                    Error = false
                });


            }


            [HttpDelete]
        public ActionResult<IEnumerable<string>> Deletefollowers(Guid Id)
        {


            var task = _context.TaskFollowers.Where(x => x.Id == Id).FirstOrDefault();
            _context.TaskFollowers.Remove(task);
            _context.SaveChanges();


            return Ok(new Response
            {
                Message = "Done !",
                Data = task,
                Error = false
            });


        }



        [HttpPut]
        public ActionResult<IEnumerable<string>> AddEmployeeVacations([FromBody] AddEmployeeVacations form)

        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var NewVacations = new EmployeeVacations
            {
                EndDate = form.EndDate,
                StartDate = form.StartDate,
                EmployeeId = form.EmployeeId,
                VacationsNote = form.VacationsNote,



            };

            _context.EmployeeVacations.Add(NewVacations);

            _context.SaveChanges();
        
            return Ok(new Response
            {
                Message = "Done !",
                Data = NewVacations,
                Error = false
            });
        }

        
        [HttpDelete]
        public ActionResult<IEnumerable<string>> DeleteEmployeeVacations(Guid id) {

            var vactions = _context.EmployeeVacations.Where(x => x.Id == id).FirstOrDefault();

            _context.EmployeeVacations.Remove(vactions);
            _context.SaveChanges();

            return Ok(new Response
            {
                Message = "Done !",
                Data = vactions,
                Error = false
            });
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> GetAllEmployees()
        {


            var list = _context.EmployessUsers.Where(x => x.IsDelete == false).ToList();



            return Ok(new Response
            {
                Message = "Done !",
                Data = list,
                Error = false
            });


        }


        [HttpGet]
        public ActionResult<IEnumerable<string>> GetEmployeeVacations()
        {


            var list = (from vacations in _context.EmployeeVacations.AsQueryable()
                        join employee in _context.EmployessUsers on vacations.EmployeeId equals employee.Id
                        select new EmployeeVacationsDto { 
                        StartDate = vacations.StartDate,
                        VacationsNote = vacations.VacationsNote,
                        employee_FullName  = employee.Employee_Fullname,
                        EndDate = vacations.EndDate,
                        Id = vacations.Id,
                        
                        
                        
   
                        }).ToList();


            return Ok(new Response
            {
                Message = "Done !",
                Data = list,
                Error = false
            });


        }
        
        
        [HttpPut]
        public async Task<ActionResult<IEnumerable<string>>> AddTasksAsync([FromBody] AddTaskes form)

        {

            var time = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
            var time1 = time.AddHours(3);


            if (!ModelState.IsValid)
                return BadRequest(new Response { Message = "Error in information", Data = null, Error = true });
            int Task_Status = 1;
            DateTime Task_Open = default;
            if (form.Task_Employee_WorkOn != null && form.Task_Employee_WorkOn != default)
            {
                Task_Status = 2;
                Task_Open = time1;
            }
              

                var NewTask = new Tasks
            {
                Task_Title = form.Task_Title,
                Task_Date = time1,
                Task_Employee_Open = Guid.Parse("3ef34045-bbbb-49e6-880e-7e7bcb9c9a16"),
                Task_part = form.Task_part,
                Task_Price_rewards = form.Task_Price_rewards,
                Tower_Id = form.Tower_Id,
                Task_Note = form.Task_Note,
                Task_EndDate = form.Task_EndDate,
                Task_Status = Task_Status,
                InternetUserId = form.InternetUserId,
                Task_Employee_WorkOn = form.Task_Employee_WorkOn,
                Task_Open = Task_Open


                };
            try
            {
                await Task.Run(() =>
                {
                

                    _employeeAddTasks.InsertTask(NewTask);
                    _employeeAddTasks.Save();
                });
            }
            catch (Exception)
            {
                // ignored
            }


            return Ok(new Response
            {
                Message = "Done !",
                Data = NewTask,
                Error = false
            });
        }


        [HttpPost]
        public ActionResult<IEnumerable<string>> DeleteTask([FromBody] Delete form, Guid TaskId)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var task = _context.Tasks.Where(x => x.Id == TaskId).FirstOrDefault();




            task.IsDelete = form.IsDelete;

            _context.Entry(task).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return Ok(new Response
            {
                Message = "Done !",
                Data = task,
                Error = false
            });


        }


        [HttpPost]
        public ActionResult<IEnumerable<string>> CloseTask([FromBody] CloseDto from, Guid TaskId)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var time = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
            var time1 = time.AddHours(3);
            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var _clientid = Guid.Parse(currentUserId);

            var Task = _context.Tasks.Where(x => x.Id == TaskId).FirstOrDefault();




            Task.Task_Employee_Close = Guid.Parse("3ef34045-bbbb-49e6-880e-7e7bcb9c9a16");
            Task.Task_Done = time1;
            Task.Task_Status = 3;
            Task.Task_closed_Note = from.Task_closed_Note;

            _context.Entry(Task).State = Microsoft.EntityFrameworkCore.EntityState.Modified;


            var taskrewards = _context.RewardsTable.Where(x => x.Id == Task.Task_Price_rewards).FirstOrDefault();
            var followers = _context.TaskFollowers.Where(x => x.TaskId == TaskId).ToList();

            decimal num = taskrewards.RewardsPrice / (followers.Count + 1);


            var Rewards = new OverTimeRewards
            {


                Employees_Id = Task.Task_Employee_WorkOn,
                OverTimeRewards_Enterid = Task.Task_Employee_Open,
                OverTimeRewards_Date = Task.Task_Done,
                OverTimeRewards_Note = "مكافئة عمل" + " : " + Task.Task_Title,
                OverTimeRewards_Price = num

            };

            _context.OverTimeRewards.Add(Rewards);
            _context.SaveChanges();

            foreach (var i in followers)
            {

                var followesRewards = new OverTimeRewards
                {


                    Employees_Id = i.EmployeeId,
                    OverTimeRewards_Enterid = Task.Task_Employee_Open,
                    OverTimeRewards_Date = Task.Task_Done,
                    OverTimeRewards_Note = "مكافئة عمل" + " : " + Task.Task_Title,
                    OverTimeRewards_Price = num

                };

                _context.OverTimeRewards.Add(followesRewards);
                _context.SaveChanges();


            }
            return Ok(new Response
            {
                Message = "Done !",
                Data = Task,
                Error = false
            });


        }


        [HttpGet]
        public ActionResult<IEnumerable<string>> GetTaskRepalys(Guid TaskId)
        {


            var list = (from repalytask in _context.TasksRepalys.Where(x => x.IsDelete == false && x.TaskId == TaskId).AsQueryable()
                        join
                        Employ in _context.EmployessUsers.AsQueryable() on repalytask.EmployeeId equals Employ.Id
                        join
                        task in _context.Tasks.AsQueryable() on repalytask.TaskId equals task.Id
                        select new RepalyTasksDto
                        {


                            TaskName = task.Task_Title,
                            EmployeeName = Employ.Employee_Fullname,
                            Repaly_Note = repalytask.Repaly_Note,
                            Id = repalytask.Id,
                            TaskId = TaskId,
                            EmployeePhoto = _configuration["var:EmployeePhoto"] + Employ.Employee_Photo,
                            RepalyDate = repalytask.RepalyDate,
                            Attachmentlist = _context.Attachment.AsQueryable().AsNoTracking().Where(x => repalytask.Id == x.Attachment_Id)
                            .Select(s => new RepalyAttachment
                            {
                                Attachment_Id = s.Attachment_Id,
                                Attachment_Name = _configuration["var:Attachments"] + s.Attachment_Name,
                                Id = s.Id

                            }).ToList()



                        }).ToList();



            return Ok(new Response
            {
                Message = "Done !",
                Data = list,
                Error = false
            });


        }


        [HttpGet]
        public ActionResult<IEnumerable<string>> GetTowerById(Guid TowerId)
        {


            var list = _context.Towers.Where(x => x.Id == TowerId).FirstOrDefault();



            return Ok(new Response
            {
                Message = "Done !",
                Data = list,
                Error = false
            });


        }


        [HttpPut]
        public ActionResult<IEnumerable<string>> AddReplayTask([FromBody] AddRepalyTask form)

        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var _clientid = Guid.Parse(currentUserId);

            var NewRepaly = new TasksRepalys
            {
                EmployeeId = Guid.Parse("3ef34045-bbbb-49e6-880e-7e7bcb9c9a16"),
                Repaly_Note = form.Repaly_Note,
                TaskId = form.TaskId,
                RepalyDate = DateTime.Now

            };

            _context.TasksRepalys.Add(NewRepaly);

            _context.SaveChanges();
            return Ok(new Response
            {
                Message = "Done !",
                Data = NewRepaly,
                Error = false
            });
        }



        [HttpPost]
        public ActionResult<IEnumerable<string>> EditTask([FromBody] EditTask form, Guid Taskid)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);



            var Task = _context.Tasks.Where(x => x.Id == Taskid).FirstOrDefault();




            Task.Task_Note = form.Task_Note;
            Task.Task_part = form.part_Id;
            Task.Task_Price_rewards = form.Task_Price_rewards;
            Task.Task_Title = form.Task_Title;
            Task.Tower_Id = form.Tower_Id;
            Task.Task_EndDate = form.Task_EndDate;
            Task.Task_Date = form.Task_Date;
            Task.InternetUserId = form.InternetUserId;




            _context.Entry(Task).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return Ok(new Response
            {
                Message = "Done !",
                Data = Task,
                Error = false
            });


        }

        


        [HttpGet]
        public ActionResult<IEnumerable<string>> GetInOutInMap()
        {


            var list = (from inout in _context.InOut.Where(x => x.In_Out_Date.Date == DateTime.Now.Date).AsQueryable()
                        join employee in _context.EmployessUsers on inout.EmplyeeId equals employee.Id
                        select new InOutMap
                        {
                           Employee_Latitude = inout.Employee_Latitude,
                           Employee_Longitude = inout.Employee_Longitude,
                           Employee_Name = employee.Employee_Fullname,
                           InOutColor = inout.In_Out_Status == 1 ? "RED" : "GREEN" ,
                           InOut_Status = inout.In_Out_Status == 1 ? "OUT" : "IN" ,
                           Id = inout.Id,
                          

                        }).ToList();


            return Ok(new Response
            {
                Message = "Done !",
                Data = list,
                Error = false
            });


        }


        [HttpGet]
        public ActionResult<IEnumerable<string>> InternetUserById(Guid InternetUserId)
        {


            var list = _context.InternetUsers.Where(x => x.Id == InternetUserId).FirstOrDefault();



            return Ok(new Response
            {
                Message = "Done !",
                Data = list,
                Error = false
            });


        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> GetTowers()
        {


            var list = _context.Towers.Where(x => x.IsDelete == false).ToList();



            return Ok(new Response
            {
                Message = "Done !",
                Data = list,
                Error = false
            });


        }



        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetInOutAsync([FromQuery] PaginationFilter filter, string EmployeeName, DateTime? datefrom, DateTime? dateto ,  int? Inout_Status)
        {

        


            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var totalRecords = await _context.InOut.Where(x => x.IsDelete == false).CountAsync();



            var list = (from inout in _context.InOut.Where(x => x.IsDelete == false).AsNoTracking().AsQueryable()
                        join employee in _context.EmployessUsers.AsNoTracking().AsQueryable() on inout.EmplyeeId equals employee.Id
                        select new InOutDto
                        {
                            Id = inout.Id,
                            Empolyee_Name = employee.Employee_Name,
                            InOut_date = inout.In_Out_Date,
                            InOut_status = inout.In_Out_Status,
                            EmployeeInOut = "وقت الدخول" + " " + employee.Employee_In_Time + " - " + "وقت الخروج" + " " + employee.Employee_Out_Time,
                            distance = inout.distance,
                            locationurl = "https://www.google.com/maps/@"+ inout.Employee_Latitude+","+ inout.Employee_Longitude,
                            InOut_time = inout.In_Out_Time


                          }).ToList();



            if (EmployeeName != null && EmployeeName != default)
                list = list.Where(s => s.Empolyee_Name.Contains(EmployeeName)).ToList();
            totalRecords = list.Count();
            if (Inout_Status != null && Inout_Status != default)
                list = list.Where(s => s.InOut_status == Inout_Status).ToList();
            totalRecords = list.Count();
            if (datefrom != null && datefrom != default && dateto != null && dateto != default)
                list = list.Where(s => s.InOut_date.Date >= datefrom && s.InOut_date <= dateto).ToList();
            totalRecords = list.Count();
            if (datefrom != null && dateto == null)
                list = list.Where(s => s.InOut_date.Date == datefrom).ToList();
            totalRecords = list.Count();


            return Ok(new PagedResponse<List<InOutDto>>(
                list.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToList(),
                validFilter.PageNumber,
                validFilter.PageSize,
                totalRecords, 0, 0));


        }





        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetAbsenceDtoAsync([FromQuery] PaginationFilter filter, string EmployeeName, DateTime? datefrom , DateTime? dateto )
        {




            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
             var totalRecords = await _context.Absence.Where(x => x.IsDelete == false).CountAsync();



            var list = (from absence in _context.Absence.Where(x => x.IsDelete == false).AsNoTracking().AsQueryable()
                        join employee in _context.EmployessUsers.AsNoTracking().AsQueryable() on absence.EmployeeId equals employee.Id
                        select new AbsenceDto
                        {
                            Id = absence.Id,
                            Empolyee_Name = employee.Employee_Name,
                            Absence_date = absence.AbsenceDate,
                           

                        }).ToList();



            if (EmployeeName != null && EmployeeName != default)
                list = list.Where(s => s.Empolyee_Name.Contains(EmployeeName)).ToList();
            totalRecords = list.Count();
            if (datefrom != null && datefrom != default && dateto != null && dateto != default)
                list = list.Where(s => s.Absence_date.Date >= datefrom && s.Absence_date <= dateto).ToList();
            totalRecords = list.Count();
            if (datefrom != null && dateto == null)
                list = list.Where(s => s.Absence_date.Date == datefrom).ToList();
            totalRecords = list.Count();

            return Ok(new PagedResponse<List<AbsenceDto>>(
                list.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToList(),
                validFilter.PageNumber,
                validFilter.PageSize,
                totalRecords, 0, 0));


        }


        [AllowAnonymous]
        [HttpGet]
        public ActionResult<IEnumerable<string>> GetEmpolyeeTaskCount()
        {
            var list = new List<EmployeeTasksCountDto>();

            string Month = DateTime.Now.Month.ToString();
            string Year = DateTime.Now.Year.ToString();
            string DAY = DateTime.Now.Day.ToString();
            DateTime date = Convert.ToDateTime(DateTime.Now).Date;


            var employee = _context.EmployessUsers.Where(x => x.IsDisplay == false).ToList();

           foreach(var emp in employee)
            {

                var AddTaskMonth = _context.Tasks.Where(x => x.Task_Employee_Open == emp.Id && x.Task_Date.Year.ToString() == Year && x.Task_Done.Month.ToString() == Month ).Count();
                var AddTaskToday = _context.Tasks.Where(x => x.Task_Employee_Open == emp.Id && x.Task_Date == date ).Count();



                var DoneTaskMonth = _context.Tasks.Where(x=> x.Task_Employee_WorkOn == emp.Id && x.Task_Done.Year.ToString() == Year && x.Task_Done.Month.ToString() == Month && x.Task_Status == 3).Count();
                var DoneTaskToday = _context.Tasks.Where(x=> x.Task_Employee_WorkOn == emp.Id && x.Task_Done.Date == date && x.Task_Status == 3).Count();
                var FollowerTaskMonth = _context.Tasks.Where(x=> x.Task_Employee_WorkOn == emp.Id && x.Task_Open.Year.ToString() == Year && x.Task_Open.Month.ToString() == Month && x.Task_Status == 2).Count();
                var FollowerTaskToday = _context.Tasks.Where(x => x.Task_Employee_WorkOn == emp.Id && x.Task_Open.Date == date && x.Task_Status == 2).Count();

                var count = new EmployeeTasksCountDto
                {
                    Employee_FullName = emp.Employee_Fullname,
                    DoneTaskMonth = DoneTaskMonth ,
                    FollowerTaskMonth = FollowerTaskMonth,
                    DoneTaskToday = DoneTaskToday,
                    FollowerTaskToday = FollowerTaskToday,
                    AddTaskMonth = AddTaskMonth,
                    AddTaskToday = AddTaskToday




                };

                list.Add(count);

            }


            return Ok(new Response
            {
                Message = "Done !",
                Data = list,
                Error = false
            });


        }

    }
}
