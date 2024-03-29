﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RestSharp;

namespace hr_master.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize(Roles = "Support")]
    [EnableCors("cross")]
    [ApiController]
    public class SupportController : ControllerBase
    {

        private readonly Context _context;
        private readonly IConfiguration _configuration;
        private readonly IEmployeeAddTasks _employeeAddTasks;
        public SupportController(Context context, IConfiguration configuration, IEmployeeAddTasks employeeAddTasks)
        {
            _context = context;
            _configuration = configuration;
            _employeeAddTasks = employeeAddTasks;
        }


        [AllowAnonymous]
        [HttpGet]
        public ActionResult<IEnumerable<string>> Checktime()
        {

            var time = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
            var time1 = time.AddHours(3);
            return Ok(time + "  " + time1);
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetAllTower([FromQuery] PaginationFilter filter, string TowerName)
        {

            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var totalRecords = await _context.Towers.Where(x => x.IsDelete == false).CountAsync();


            var list = (from tower in _context.Towers.Where(x => x.IsDelete == false)

                        select new Towers
                        {

                            IsDelete = tower.IsDelete,
                            Tower_Ip = tower.Tower_Ip,
                            Tower_locition = tower.Tower_locition,
                            Tower_Name = tower.Tower_Name,
                            Tower_Note = tower.Tower_Note,
                            Tower_Owner = tower.Tower_Owner,
                            Tower_Owner_Number = tower.Tower_Owner_Number,
                            Id = tower.Id,


                        }).ToList();
            if (TowerName != null && TowerName != default)
                list = list.Where(s => s.Tower_Name.Contains(TowerName)).ToList();
            totalRecords = list.Count();



            return Ok(new PagedResponse<List<Towers>>(
                list.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToList(),
                validFilter.PageNumber,
                validFilter.PageSize,
                totalRecords, 0, 0));


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

        [HttpPut]
        public ActionResult<IEnumerable<string>> AddTowerBroadcasting([FromBody] List<AddTowerBroadcasting> form)

        {

            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var _clientid = Guid.Parse(currentUserId);


            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            foreach (var i in form)
            {
                var TowerBroadcasting = new Tower_Broadcasting
                {

                    Employee_Id = _clientid,
                    Broadcasting_Ip = i.Broadcasting_Ip,
                    Broadcasting_Password = i.Broadcasting_Password,
                    Broadcasting_SerailNamber = i.Broadcasting_SerailNamber,
                    Broadcasting_SSID = i.Broadcasting_SSID,
                    Broadcasting_Time = DateTime.Now,
                    Broadcasting_Type = i.Broadcasting_Type,
                    Broadcasting_UserName = i.Broadcasting_UserName,
                    Tower_Id = i.Tower_Id,



                };

                _context.TowerBroadcasting.Add(TowerBroadcasting);
                _context.SaveChanges();

            }


            return Ok(new Response
            {
                Message = "Done !",
                Data = form,
                Error = false
            });
        }

        [HttpPut]
        public ActionResult<IEnumerable<string>> AddTowerElectrical([FromBody] List<AddTowerElectrical> form)

        {

            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var _clientid = Guid.Parse(currentUserId);


            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            foreach (var i in form)
            {
                var TowerElectrical = new Tower_Electrical
                {

                    Employee_Id = _clientid,
                    Electrical_Date = DateTime.Now,
                    Electrical_Name = i.Electrical_Name,
                    Electrical_SerailNamber = i.Electrical_SerailNamber,
                    Electrical_Type = i.Electrical_Type,
                    Tower_Id = i.Tower_Id


                };

                _context.TowerElectrical.Add(TowerElectrical);

                _context.SaveChanges();
            }
            return Ok(new Response
            {
                Message = "Done !",
                Data = form,
                Error = false
            });
        }

        [HttpPut]
        public ActionResult<IEnumerable<string>> AddTowerNote([FromBody] AddTowerNote form)

        {

            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var _clientid = Guid.Parse(currentUserId);


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var TowerNotes = new Tower_Notes
            {

                Tower_Id = form.Tower_Id,
                Notes = form.Notes,
                Notes_Date = form.Notes_Date,



            };

            _context.TowerNotes.Add(TowerNotes);

            _context.SaveChanges();

            return Ok(new Response
            {
                Message = "Done !",
                Data = TowerNotes,
                Error = false
            });
        }

        [HttpPost]
        public ActionResult<IEnumerable<string>> EditTowerBroadcasting([FromBody] AddTowerBroadcasting form, Guid BroadcastingId)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var _clientid = Guid.Parse(currentUserId);

            var towerb = _context.TowerBroadcasting.Where(x => x.Id == BroadcastingId).FirstOrDefault();


            towerb.Broadcasting_Password = form.Broadcasting_Password;
            towerb.Broadcasting_SerailNamber = form.Broadcasting_SerailNamber;
            towerb.Broadcasting_Ip = form.Broadcasting_Ip;
            towerb.Broadcasting_SSID = form.Broadcasting_SSID;
            towerb.Broadcasting_Time = form.Broadcasting_Time;
            towerb.Broadcasting_UserName = form.Broadcasting_UserName;
            towerb.Employee_Id = _clientid;
            towerb.Broadcasting_Type = form.Broadcasting_Type;

            _context.Entry(towerb).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return Ok(new Response
            {
                Message = "Done !",
                Data = towerb,
                Error = false
            });


        }

        [HttpPost]
        public ActionResult<IEnumerable<string>> EditTowerElectrical([FromBody] AddTowerElectrical form, Guid ElectricalId)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var _clientid = Guid.Parse(currentUserId);

            var towere = _context.TowerElectrical.Where(x => x.Id == ElectricalId).FirstOrDefault();


            towere.Electrical_Date = form.Electrical_Date;
            towere.Electrical_Name = form.Electrical_Name;
            towere.Electrical_SerailNamber = form.Electrical_SerailNamber;
            towere.Electrical_Type = form.Electrical_Type;
            towere.Employee_Id = _clientid;




            _context.Entry(towere).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return Ok(new Response
            {
                Message = "Done !",
                Data = towere,
                Error = false
            });


        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> GetTowerBroadcasting(Guid TowerId)
        {



            var list = (from item in _context.TowerBroadcasting.Where(x => x.Tower_Id == TowerId)
                        select new TowerBroadcastingDto
                        {

                            Broadcasting_Ip = item.Broadcasting_Ip,
                            Broadcasting_Password = item.Broadcasting_Password,
                            Broadcasting_SerailNamber = item.Broadcasting_SerailNamber,
                            Broadcasting_SSID = item.Broadcasting_SSID,
                            Broadcasting_Time = item.Broadcasting_Time.ToString("yyyy-MM-dd"),
                            Broadcasting_Type = item.Broadcasting_Type,
                            Broadcasting_UserName = item.Broadcasting_UserName,
                            Employee_Id = item.Employee_Id,
                            Tower_Id = item.Tower_Id,
                            Id = item.Id,


                        }).ToList();

            return Ok(new Response
            {
                Message = "Done !",
                Data = list,
                Error = false
            });


        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> GetTowerElectrical(Guid TowerId)
        {


            var list = (from item in _context.TowerElectrical.Where(x => x.Tower_Id == TowerId)
                        select new TowerElectricalDto
                        {

                            Electrical_SerailNamber = item.Electrical_SerailNamber,
                            Electrical_Date = item.Electrical_Date.ToString("yyyy-MM-dd"),
                            Electrical_Name = item.Electrical_Name,
                            Electrical_Type = item.Electrical_Type,
                            Employee_Id = item.Employee_Id,
                            Id = item.Id,
                            Tower_Id = item.Tower_Id,


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
        public ActionResult<IEnumerable<string>> GetTowerNote(Guid TowerId)
        {


            var list = _context.TowerNotes.Where(x => x.Tower_Id == TowerId).ToList();



            return Ok(new Response
            {
                Message = "Done !",
                Data = list,
                Error = false
            });


        }

        [HttpDelete]
        public ActionResult<IEnumerable<string>> DeleteTowerNote(Guid TowerNoteId)
        {


            var task = _context.TowerNotes.Where(x => x.Id == TowerNoteId).FirstOrDefault();
            _context.TowerNotes.Remove(task);
            _context.SaveChanges();


            return Ok(new Response
            {
                Message = "Done !",
                Data = task,
                Error = false
            });


        }

        [HttpDelete]
        public ActionResult<IEnumerable<string>> DeleteTowerElectrical(Guid ElectricalId)
        {


            var task = _context.TowerElectrical.Where(x => x.Id == ElectricalId).FirstOrDefault();
            _context.TowerElectrical.Remove(task);
            _context.SaveChanges();


            return Ok(new Response
            {
                Message = "Done !",
                Data = task,
                Error = false
            });


        }


        [HttpDelete]
        public ActionResult<IEnumerable<string>> DeleteTowerBroadcasting(Guid BroadcastingId)
        {


            var task = _context.TowerBroadcasting.Where(x => x.Id == BroadcastingId).FirstOrDefault();
            _context.TowerBroadcasting.Remove(task);
            _context.SaveChanges();


            return Ok(new Response
            {
                Message = "Done !",
                Data = task,
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
        public ActionResult<IEnumerable<string>> DeleteTower([FromBody] Delete form, Guid Towerid)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var Team = _context.Towers.Where(x => x.Id == Towerid).FirstOrDefault();




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
        public ActionResult<IEnumerable<string>> EditTower([FromBody] EditTowers form, Guid Towerid)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);



            var Tower = _context.Towers.Where(x => x.Id == Towerid).FirstOrDefault();




            Tower.Tower_Name = form.Tower_Name;
            Tower.Tower_Ip = form.Tower_Ip;
            Tower.Tower_Name = form.Tower_Name;
            Tower.Tower_Owner = form.Tower_Owner;
            Tower.Tower_Owner_Number = form.Tower_Owner_Number;
            Tower.Tower_Note = form.Tower_Note;



            _context.Entry(Tower).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return Ok(new Response
            {
                Message = "Done !",
                Data = Tower,
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



        [HttpPut]
        public ActionResult<IEnumerable<string>> AddTower([FromBody] AddTower form)

        {

            var towers = _context.Towers.Where(x => x.Tower_Name == form.Tower_Name).FirstOrDefault();
            if(towers != null)
            {

                return BadRequest(new Response
                {
                    Message = "البرج موجود مسبقا",
                    Data = towers,
                    Error = true
                });
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var NewTower = new Towers
            {
                Tower_Ip = form.Tower_Ip,
                Tower_locition = form.Tower_locition,
                Tower_Name = form.Tower_Name,
                Tower_Note = form.Tower_Note,
                Tower_Owner = form.Tower_Owner,
                Tower_Owner_Number = form.Tower_Owner_Number
            };

            _context.Towers.Add(NewTower);

            _context.SaveChanges();
            return Ok(new Response
            {
                Message = "Done !",
                Data = NewTower,
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


        [HttpGet]
        public ActionResult<IEnumerable<string>> dashbardcounts()
        {

            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var _clientid = Guid.Parse(currentUserId);

            string Month = DateTime.Now.Month.ToString();
            string Year = DateTime.Now.Year.ToString();

            var employee = _context.EmployessUsers.Where(x => x.Id == _clientid).FirstOrDefault();

            var Alltask = _context.Tasks.Where(x => x.IsDelete == false && x.Task_part == employee.Employee_Team).Count();
            var opentask = _context.Tasks.Where(x => x.IsDelete == false && x.Task_Status == 2 && x.Task_part == employee.Employee_Team).Count();
            var waittask = _context.Tasks.Where(x => x.IsDelete == false && x.Task_Status == 1 && x.Task_part == employee.Employee_Team).Count();
            var donetask = _context.Tasks.Where(x => x.IsDelete == false && x.Task_Status == 3 && x.Task_part == employee.Employee_Team).Count();
            var EmployessNameSallary = _context.EmployessUsers.Where(x => x.Id == _clientid).FirstOrDefault();

            var rawards = _context.OverTimeRewards.Where(x => x.Employees_Id == _clientid && x.OverTimeRewards_Date.Month.ToString() == Month && x.OverTimeRewards_Date.Year.ToString() == Year).ToList();
            decimal amontrawrad = 0;
            foreach (var rew in rawards)
            {

                amontrawrad += rew.OverTimeRewards_Price;

            }


            var penalties = _context.Penalties.Where(x => x.Employees_Id == _clientid && x.Penalties_Date.Month.ToString() == Month && x.Penalties_Date.Year.ToString() == Year).ToList();
            decimal amontpenalties = 0;
            foreach (var pen in penalties)
            {

                amontpenalties += pen.Penalties_Price;

            }
            decimal EmployessSallary = (EmployessNameSallary.Employee_Saller + amontrawrad) - amontpenalties;

            var Counts = new SupportDashboardCounts { DoneTask = donetask, OpenTask = opentask, Waittask = waittask, MySallery = EmployessSallary , Alltask = Alltask };

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
        public async Task<ActionResult<IEnumerable<string>>> GetPenaltiesAsync([FromQuery] PaginationFilter filter, DateTime? date)
        {
            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var _clientid = Guid.Parse(currentUserId);

            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var totalRecords = await _context.Penalties.Where(x => x.IsDelete == false && x.Employees_Id == _clientid).CountAsync();

            var list = (from Penalties in _context.Penalties.Where(x => x.IsDelete == false && x.Employees_Id == _clientid).AsQueryable()
                        join Employee in _context.EmployessUsers.AsQueryable() on Penalties.Penalties_Enterid equals Employee.Id
                        select new PenaltiesDto
                        {
                            Id = Penalties.Id,
                            Penalties_Enterid = Employee.Employee_Fullname,
                            Employees_Id = _clientid,
                            Penalties_Date = Penalties.Penalties_Date,
                            Penalties_Note = Penalties.Penalties_Note,
                            Penalties_Price = Penalties.Penalties_Price

                        }).ToList();



            if (date != null && date != default)
                list = list.Where(s => s.Penalties_Date.Date == date).ToList();
            totalRecords = list.Count();


            return Ok(new PagedResponse<List<PenaltiesDto>>(
                list.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToList(),
                validFilter.PageNumber,
                validFilter.PageSize,
                totalRecords, 0, 0));


        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetRewardsAsync([FromQuery] PaginationFilter filter, DateTime? date)
        {
            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var _clientid = Guid.Parse(currentUserId);

            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var totalRecords = await _context.Penalties.Where(x => x.IsDelete == false && x.Employees_Id == _clientid).CountAsync();

            var list = (from Rewards in _context.OverTimeRewards.Where(x => x.IsDelete == false && x.Employees_Id == _clientid).AsQueryable()
                        join Employee in _context.EmployessUsers.AsQueryable() on Rewards.OverTimeRewards_Enterid equals Employee.Id

                        select new RewardsDto
                        {
                            Id = Rewards.Id,
                            Rewards_Enterid = Employee.Employee_Fullname,
                            Employees_Id = _clientid,
                            Rewards_Date = Rewards.OverTimeRewards_Date,
                            Rewards_Note = Rewards.OverTimeRewards_Note,
                            Rewards_Price = Rewards.OverTimeRewards_Price

                        }).ToList();



            if (date != null && date != default)
                list = list.Where(s => s.Rewards_Date.Date == date).ToList();
            totalRecords = list.Count();


            return Ok(new PagedResponse<List<RewardsDto>>(
                list.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToList(),
                validFilter.PageNumber,
                validFilter.PageSize,
                totalRecords, 0, 0));


        }



        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> InternetUser([FromQuery] PaginationFilter filter, string User_Name, DateTime? date, string User_FullName, bool? IsActive)
        {


            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var totalRecords = await _context.InternetUsers.Where(x => x.IsDelete == false).CountAsync();


            var list = (from iuser in _context.InternetUsers.Where(x => x.IsDelete == false).AsNoTracking()

                        select new InternetUsers
                        {

                            Id = iuser.Id,
                            User_Price = iuser.User_Price,
                            User_Password = iuser.User_Password,
                            User_EndDate = iuser.User_EndDate,
                            User_ActiveDate = iuser.User_ActiveDate,
                            User_Card = iuser.User_Card,
                            User_FullName = iuser.User_FullName,
                            User_Name = iuser.User_Name,
                            User_Phone = iuser.User_Phone,
                            IsActive = iuser.IsActive,
                            IsDelete = iuser.IsDelete,
                            User_Adress = iuser.User_Adress,
                            User_Tower = iuser.User_Tower

                        }).ToList();

            if (User_Name != null && User_Name != default)
                list = list.Where(s => s.User_Name.Contains(User_Name)).ToList();
            totalRecords = list.Count();
            if (User_FullName != null && User_FullName != default)
                list = list.Where(s => s.User_FullName.Contains(User_FullName)).ToList();
            totalRecords = list.Count();
            if (date != null && date != default)
                list = list.Where(s => s.User_ActiveDate.Date == date).ToList();
            totalRecords = list.Count();
            if (IsActive != null && IsActive != default)
                list = list.Where(s => s.IsActive == IsActive).ToList();
            totalRecords = list.Count();

            return Ok(new PagedResponse<List<InternetUsers>>(
                list.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToList(),
                validFilter.PageNumber,
                validFilter.PageSize,
                totalRecords, 0, 0));


        }



        [HttpPost]
        public ActionResult<IEnumerable<string>> Sendnotifications(Guid InternetUserId, string notifications)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);



            //  var InternetUsers = _context.InternetUsers.Where(x => x.Id == InternetUserId).FirstOrDefault();




            var noitictioneform = new NotificationsForm
            {

                contents = notifications,
                headings = "يرجى الانتباه",
                url = "https://sys.center-wifi.com",
                ///included_segments = "All",
                include_external_user_ids = InternetUserId.ToString()


            };



            _ = SendNoiticationsforusers(noitictioneform);

            return Ok(new Response
            {
                Message = "Done !",
                Data = noitictioneform,
                Error = false
            });


        }





        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetUsersComplain([FromQuery] PaginationFilter filter, string User_FullName, DateTime? date)
        {





            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var totalRecords = await _context.User_Complaint.Where(x => x.IsDelete == false).CountAsync();


            var list = (from complain in _context.User_Complaint.Where(x => x.IsDelete == false).AsNoTracking()
                        join iuser in _context.InternetUsers.AsNoTracking() on complain.InternetUser_Id equals iuser.Id



                        select new ComplaintDto
                        {
                            Id = complain.Id,
                            InternetUser_FullName = iuser.User_FullName,
                            InternetUser_Id = iuser.Id,
                            Complain = complain.Complain,
                            Complain_Date = complain.Complain_Date,
                            Complain_Status = complain.Complain_Status,
                            Tower_Name = iuser.User_Tower,
                            User_ActiveDate = iuser.User_ActiveDate,
                            User_Adress = iuser.User_Adress,
                            User_Card = iuser.User_Card,
                            User_EndDate = iuser.User_EndDate,
                            User_Name = iuser.User_Name,
                            User_Password = iuser.User_Password,
                            User_Phone = iuser.User_Phone,
                            User_Price = iuser.User_Price,
                            IsActive = iuser.IsActive





                        }).ToList();

            if (User_FullName != null && User_FullName != default)
                list = list.Where(s => s.InternetUser_FullName.Contains(User_FullName)).ToList();
            totalRecords = list.Count();
            if (date != null && date != default)
                list = list.Where(s => s.Complain_Date.Date == date).ToList();
            totalRecords = list.Count();


            return Ok(new PagedResponse<List<ComplaintDto>>(
                list.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToList(),
                validFilter.PageNumber,
                validFilter.PageSize,
                totalRecords, 0, 0));


        }


        [HttpPost]
        public ActionResult<IEnumerable<string>> SendToInternetUserList([FromBody] List<InternetUsers> from, string notifications)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);



            //  var InternetUsers = _context.InternetUsers.Where(x => x.Id == InternetUserId).FirstOrDefault();

            foreach (var i in from)
            {

                var noitictioneform = new NotificationsForm
                {

                    contents = notifications,
                    headings = "يرجى الانتباه",
                    url = "https://sys.center-wifi.com",
                    ///included_segments = "All",
                    include_external_user_ids = i.Id.ToString()

                };



                _ = SendNoiticationsforuser(noitictioneform);

            }




            return Ok(new Response
            {
                Message = "Done !",
                Data = from,
                Error = false
            });


        }




        private IActionResult SendNoiticationsforusers(NotificationsForm form)
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
                //included_segments = new string[] { form.included_segments }
                include_external_user_ids = new string[] { form.include_external_user_ids }
            };

            request.AddJsonBody(body);

            IRestResponse response = client.Execute(request);

            return Ok(response.Content);
        }

        private IActionResult SendNoiticationsforuser(NotificationsForm form)
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
                //included_segments = new string[] { form.included_segments }
                include_external_user_ids = new string[] { form.include_external_user_ids }
            };

            request.AddJsonBody(body);

            IRestResponse response = client.Execute(request);

            return Ok(response.Content);
        }

    }
}
