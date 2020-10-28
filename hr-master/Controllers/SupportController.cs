using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
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
    [Authorize(Roles = "Support")]
    [EnableCors("cross")]
    [ApiController]
    public class SupportController : ControllerBase
    {

        private readonly Context _context;
        private readonly IConfiguration _configuration;
        public SupportController(Context context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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
        public async Task<ActionResult<IEnumerable<string>>> GetAllTower([FromQuery] PaginationFilter filter , string TowerName)
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
        public ActionResult<IEnumerable<string>> AddTowerElectrical([FromBody]  List<AddTowerElectrical> form)

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
        public ActionResult<IEnumerable<string>> UnfolloweTask(Guid TaskId)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var time = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
            var time1 = time.AddHours(3);
            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var _clientid = Guid.Parse(currentUserId);

            var Task = _context.Tasks.Where(x => x.Id == TaskId).FirstOrDefault();




            Task.Task_Employee_WorkOn = default;
            Task.Task_Open = default;
            Task.Task_Status = 1;


            _context.Entry(Task).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return Ok(new Response
            {
                Message = "Done !",
                Data = Task,
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


            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var _clientid = Guid.Parse(currentUserId);





            var Employeeinfo = await _context.EmployessUsers.Where(x => x.Id == _clientid).FirstOrDefaultAsync();


            var teams = await _context.Teams.Where(x => x.Id == Employeeinfo.Employee_Team).FirstOrDefaultAsync();

            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var totalRecords = await _context.Tasks.Where(x => x.IsDelete == false && x.Task_part == teams.Id && x.Task_Status != 3).CountAsync();


            var list = (from task in _context.Tasks.OrderByDescending(x => x.Task_Date).Where(x => x.IsDelete == false && x.Task_part == teams.Id && x.Task_Status != 3).AsNoTracking()
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


            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var _clientid = Guid.Parse(currentUserId);





            var Employeeinfo = await _context.EmployessUsers.Where(x => x.Id == _clientid).FirstOrDefaultAsync();


            var teams = await _context.Teams.Where(x => x.Id == Employeeinfo.Employee_Team).FirstOrDefaultAsync();

            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var totalRecords = await _context.Tasks.Where(x => x.IsDelete == false && x.Task_part == teams.Id && x.Task_Status == 3).CountAsync();


            var list = (from task in _context.Tasks.OrderByDescending(x => x.Task_Date).Where(x => x.IsDelete == false && x.Task_part == teams.Id && x.Task_Status == 3).AsNoTracking()
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


        [HttpPut]
        public ActionResult<IEnumerable<string>> AddfollowerTask([FromBody] AddTaskFollowers form)

        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var followers = _context.TaskFollowers.Where(x => x.EmployeeId == form.EmployeeId).FirstOrDefault();
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




        [HttpPost]
        public ActionResult<IEnumerable<string>> followeTask(Guid TaskId)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var time = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
            var time1 = time.AddHours(3);

            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var _clientid = Guid.Parse(currentUserId);

            var Task = _context.Tasks.Where(x => x.Id == TaskId).FirstOrDefault();




            Task.Task_Employee_WorkOn = _clientid;
            Task.Task_Open = time1;
            Task.Task_Status = 2;


            _context.Entry(Task).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return Ok(new Response
            {
                Message = "Done !",
                Data = Task,
                Error = false
            });


        }




        [HttpPut]
        public ActionResult<IEnumerable<string>> AddTasks([FromBody] AddTaskes form)

        {

            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var _clientid = Guid.Parse(currentUserId);


            var time = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
            var time1 = time.AddHours(3);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var NewTask = new Tasks
            {
                Task_Title = form.Task_Title,
                Task_Date = time1,
                Task_Employee_Open = _clientid,
                Task_part = form.Task_part,
                Task_Price_rewards = form.Task_Price_rewards,
                Tower_Id = form.Tower_Id,
                Task_Note = form.Task_Note,
                Task_EndDate = form.Task_EndDate,
                Task_Status = 1 ,
                InternetUserId = form.InternetUserId,
                




            };

            _context.Tasks.Add(NewTask);

            _context.SaveChanges();

            var parts = _context.Teams.Where(x => x.Id == NewTask.Task_part).FirstOrDefault();
            var noitictioneform = new NotificationsForm
            {

                contents = "تم اضافة تاسك جديد",
                headings = "يرجى الانتباه",
                url = "http://sys.center-wifi.com",
                included_segments = parts.Team_Roles,


            };

            _ = SendNoitications(noitictioneform);
       
            return Ok(new Response
            {
                Message = "Done !",
                Data = NewTask,
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




            Task.Task_Employee_Close  = _clientid ;
            Task.Task_Done = time1;
            Task.Task_Status = 3;
            Task.Task_closed_Note = from.Task_closed_Note;

            _context.Entry(Task).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
      

            var taskrewards = _context.RewardsTable.Where(x => x.Id == Task.Task_Price_rewards).FirstOrDefault();
            var followers = _context.TaskFollowers.Where(x => x.TaskId == TaskId).ToList();

            decimal num = taskrewards.RewardsPrice / (followers.Count + 1);

            DateTime date = DateTime.Now;
            var Rewards = new OverTimeRewards
            {


                Employees_Id = Task.Task_Employee_WorkOn,
                OverTimeRewards_Enterid = Task.Task_Employee_Open,
                OverTimeRewards_Date = TimeZoneInfo.ConvertTimeToUtc(Task.Task_Done) ,
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

      

        [HttpPut]
        public ActionResult<IEnumerable<string>> AddTower([FromBody] AddTower form)

        {

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





        [HttpPut]
        public ActionResult<IEnumerable<string>> AddReplayTask([FromBody] AddRepalyTask form)

        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var _clientid = Guid.Parse(currentUserId);

            var NewRepaly = new TasksRepalys
            {
                EmployeeId = _clientid,
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

            var Alltask = _context.Tasks.Where(x => x.IsDelete == false &&  x.Task_Employee_Open == _clientid).Count();
            var opentask = _context.Tasks.Where(x => x.IsDelete == false && x.Task_Status == 2 && x.Task_Employee_Open == _clientid).Count();
            var waittask = _context.Tasks.Where(x => x.IsDelete == false && x.Task_Status == 1 && x.Task_Employee_Open == _clientid).Count();
            var donetask = _context.Tasks.Where(x => x.IsDelete == false && x.Task_Status == 3 && x.Task_Employee_Open == _clientid).Count();
            var EmployessSallary = _context.EmployessUsers.Where(x => x.Id == _clientid).FirstOrDefault();
    



            var Counts = new SupportDashboardCounts { DoneTask = donetask, OpenTask  = opentask , Waittask = waittask , MySallery = EmployessSallary.Employee_Saller , Alltask = Alltask };

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
        public ActionResult<IEnumerable<string>> SendToInternetUserList([FromBody] List<InternetUsers> from , string notifications)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);



            //  var InternetUsers = _context.InternetUsers.Where(x => x.Id == InternetUserId).FirstOrDefault();

            foreach(var i in from)
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
        private IActionResult SendNoitications(NotificationsForm form)
        {

            var client = new RestClient(_configuration["onesginel:Url"]);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", _configuration["onesginel:Authorization"]);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Cookie", "__cfduid=d8a2aa2f8395ad68b8fd27b63127834571600976869");

            //request.AddParameter("application/json", "{\r\n\"app_id\" : \"b7d2542a-824a-4afa-9389-08880920baa8\",\r\n\"contents\" : {\"en\" : \"تم اضافة تاسك جديد\"},\r\n\"headings\" : {\"en\" : \"يرجى الانتباة\"},\r\n\"url\" : \"http://wifihr.tatwer.tech\",\r\n\"included_segments\" : [\"All\"]\r\n}", ParameterType.RequestBody);



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
