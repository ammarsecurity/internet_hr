using System;
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
    [Authorize(Roles = "Account,Admin,Support,Technical,Store")]
    [EnableCors("cross")]
    [ApiController]
    public class TasksController : ControllerBase
    {

        private readonly Context _context;
        private readonly IConfiguration _configuration;
        private readonly IEmployeeAddTasks _employeeAddTasks;
        public TasksController(Context context, IConfiguration configuration, IEmployeeAddTasks employeeAddTasks )
        {
            _context = context;
            _configuration = configuration;
            _employeeAddTasks = employeeAddTasks;
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




            Task.Task_Employee_Close = _clientid;
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


        [HttpGet]
        public ActionResult<IEnumerable<string>> GetEmployeeBySelectTeam(Guid TeamId)
        {


            var Team = _context.Teams.Where(x => x.Id == TeamId && x.IsDelete == false).FirstOrDefault();
            var employee = _context.EmployessUsers.Where(x => x.Employee_Team == Team.Id && x.IsDelete == false).ToList();




            return Ok(new Response
            {
                Message = "Done !",
                Data = employee,
                Error = false
            });


        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetMyTeamTask([FromQuery] PaginationFilter filter, string Task_Employee_WorkOn, DateTime? date, string Task_Employee_Open, int? Task_Status , Guid? Tower_Id, Guid? InternetUserId , string Task_Title)
        {


            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var _clientid = Guid.Parse(currentUserId);
            var _user =await _context.EmployessUsers.Where(x => x.Id == _clientid).FirstOrDefaultAsync();
            var _team = await _context.Teams.Where(x => x.Id == _user.Employee_Team).FirstOrDefaultAsync();

            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var totalRecords = await _context.Tasks.Where(x => x.IsDelete && x.Task_Open_Part == _team.Id).CountAsync();


            var list = (from task in _context.Tasks.OrderByDescending(x => x.Task_Date).Where(x => x.IsDelete == false && x.Task_Open_Part == _team.Id && x.Task_Status != 3).AsNoTracking()
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
                            InternetUserName = internetuser.User_Name ?? "لايوجد"





                        }).ToList();

            if (Task_Employee_WorkOn != null && Task_Employee_WorkOn != default)
                list = list.Where(s => s.Task_Employee_WorkOn.Contains(Task_Employee_WorkOn)).ToList();
            totalRecords = list.Count();  
            
            if (Task_Title != null && Task_Title != default)
                list = list.Where(s => s.Task_Title.Contains(Task_Title)).ToList();
            totalRecords = list.Count();

            if (Tower_Id != null && Tower_Id != default )
                list = list.Where(s => s.Tower_Id == Tower_Id).ToList();
            totalRecords = list.Count();

            if (InternetUserId != null && InternetUserId != default)
                list = list.Where(s => s.InternetUserId == InternetUserId).ToList();
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
        public async Task<ActionResult<IEnumerable<string>>> GetMyAllTask([FromQuery] PaginationFilter filter, string Task_Employee_WorkOn, DateTime? date, string Task_Employee_Open, int? Task_Status, Guid? Tower_Id, Guid? InternetUserId, string Task_Title)
        {


            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var _clientid = Guid.Parse(currentUserId);


            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var totalRecords = await _context.Tasks.Where(x => x.IsDelete == false && x.Task_Employee_Open == _clientid ).CountAsync();


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
                            InternetUserName = internetuser.User_Name ?? "لايوجد"





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
            if (Tower_Id != null && Tower_Id != default)
                list = list.Where(s => s.Tower_Id == Tower_Id).ToList();
            totalRecords = list.Count();
            if (Task_Title != null && Task_Title != default)
                list = list.Where(s => s.Task_Title.Contains(Task_Title)).ToList();
            totalRecords = list.Count();
            if (InternetUserId != null && InternetUserId != default)
                list = list.Where(s => s.InternetUserId == InternetUserId).ToList();
            totalRecords = list.Count();

            return Ok(new PagedResponse<List<TasksDto>>(
                list.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToList(),
                validFilter.PageNumber,
                validFilter.PageSize,
                totalRecords, 0, 0));


        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetAllTask([FromQuery] PaginationFilter filter, string Task_Employee_WorkOn, DateTime? date, string Task_Employee_Open, int? Task_Status, Guid? Tower_Id, Guid? InternetUserId, string Task_Title)
        {


            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var _clientid = Guid.Parse(currentUserId);
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var nlist = new List<TasksDto>();
            var totalRecords = 0;

            if (_clientid != Guid.Parse("3ef34045-bbbb-49e6-880e-7e7bcb9c9a16")) {

            var Employeeinfo = await _context.EmployessUsers.Where(x => x.Id == _clientid).FirstOrDefaultAsync();


            var teams = await _context.Teams.Where(x => x.Id == Employeeinfo.Employee_Team).FirstOrDefaultAsync();
                if (teams == null)
                {
                    return BadRequest(new Response
                    {
                        Message = "يجب تعديل فريق الموظف",
                        Data = "",
                        Error = false
                    });

                }

                totalRecords = await _context.Tasks.Where(x => x.IsDelete == false && x.Task_part == teams.Id && x.Task_Status != 3).CountAsync();


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
                            Tower_Name = tower.Tower_Name ?? "لايوجد",
                            Tower_Id = tower.Id,
                            part_Id = part.Id,
                            Task_Price = reward.RewardsPrice,
                            InternetUserId = internetuser.Id,
                            Task_Employee_WorkOn_id = task.Task_Employee_WorkOn,
                             InternetUserName = internetuser.User_Name ?? "لايوجد"




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
                if (Tower_Id != null && Tower_Id != default)
                    list = list.Where(s => s.Tower_Id == Tower_Id).ToList();
                totalRecords = list.Count();

                if (InternetUserId != null && InternetUserId != default)
                    list = list.Where(s => s.InternetUserId == InternetUserId).ToList();
                totalRecords = list.Count();
                if (Task_Title != null && Task_Title != default)
                    list = list.Where(s => s.Task_Title.Contains(Task_Title)).ToList();
                totalRecords = list.Count();
                nlist.AddRange(list);
            }
            else
            {


             

                totalRecords = await _context.Tasks.Where(x => x.IsDelete == false && x.Task_Status != 3).CountAsync();


                var list = (from task in _context.Tasks.OrderByDescending(x => x.Task_Date).Where(x => x.IsDelete == false && x.Task_Status != 3).AsNoTracking()
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
                                 InternetUserName = internetuser.User_Name ?? "لايوجد"




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
                if (Tower_Id != null && Tower_Id != default)
                    list = list.Where(s => s.Tower_Id == Tower_Id).ToList();
                totalRecords = list.Count();

                if (InternetUserId != null && InternetUserId != default)
                    list = list.Where(s => s.InternetUserId == InternetUserId).ToList();
                totalRecords = list.Count();
                if (Task_Title != null && Task_Title != default)
                    list = list.Where(s => s.Task_Title.Contains(Task_Title)).ToList();
                totalRecords = list.Count();
                nlist.AddRange(list);
            }


            return Ok(new PagedResponse<List<TasksDto>>(
                nlist.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToList(),
                validFilter.PageNumber,
                validFilter.PageSize,
                totalRecords, 0, 0));


        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetAllDoneTask([FromQuery] PaginationFilter filter, string Task_Employee_WorkOn, DateTime? date, string Task_Employee_Open, int? Task_Status, Guid? Tower_Id, Guid? InternetUserId, string Task_Title)
        {


            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var _clientid = Guid.Parse(currentUserId);
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var nlist = new List<TasksDto>();
            var totalRecords = 0;

            if (_clientid != Guid.Parse("3ef34045-bbbb-49e6-880e-7e7bcb9c9a16"))
            {

                var Employeeinfo = await _context.EmployessUsers.Where(x => x.Id == _clientid).FirstOrDefaultAsync();


                var teams = await _context.Teams.Where(x => x.Id == Employeeinfo.Employee_Team).FirstOrDefaultAsync();

                if(teams == null)
                {
                    return BadRequest(new Response
                    {
                        Message = "يجب تعديل فريق الموظف",
                        Data = "",
                        Error = false
                    });

                }

                totalRecords = await _context.Tasks.Where(x => x.IsDelete == false && x.Task_part == teams.Id && x.Task_Status == 3).CountAsync();


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
                                Tower_Name = tower.Tower_Name ?? "لايوجد",
                                Tower_Id = tower.Id,
                                part_Id = part.Id,
                                Task_Price = reward.RewardsPrice,
                                InternetUserId = internetuser.Id,
                                Task_Employee_WorkOn_id = task.Task_Employee_WorkOn,
                                 InternetUserName = internetuser.User_Name ?? "لايوجد"




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
                if (Tower_Id != null && Tower_Id != default)
                    list = list.Where(s => s.Tower_Id == Tower_Id).ToList();
                totalRecords = list.Count();

                if (InternetUserId != null && InternetUserId != default)
                    list = list.Where(s => s.InternetUserId == InternetUserId).ToList();
                totalRecords = list.Count();
                if (Task_Title != null && Task_Title != default)
                    list = list.Where(s => s.Task_Title.Contains(Task_Title)).ToList();
                totalRecords = list.Count();

                nlist.AddRange(list);
            }
            else
            {

              


                totalRecords = await _context.Tasks.Where(x => x.IsDelete == false  && x.Task_Status == 3).CountAsync();


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
                if (Tower_Id != null && Tower_Id != default)
                    list = list.Where(s => s.Tower_Id == Tower_Id).ToList();
                totalRecords = list.Count();

                if (InternetUserId != null && InternetUserId != default)
                    list = list.Where(s => s.InternetUserId == InternetUserId).ToList();
                totalRecords = list.Count();

                if (Task_Title != null && Task_Title != default)
                    list = list.Where(s => s.Task_Title.Contains(Task_Title)).ToList();
                totalRecords = list.Count();


                nlist.AddRange(list);



            }

            return Ok(new PagedResponse<List<TasksDto>>(
                nlist.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToList(),
                validFilter.PageNumber,
                validFilter.PageSize,
                totalRecords, 0, 0));


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
        [HttpPut]
        public async Task<ActionResult<IEnumerable<string>>> AddTasksAsync([FromBody] AddTaskes form)

        {





            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var _clientid = Guid.Parse(currentUserId);

            var time = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
            var time1 = time.AddHours(3);


            var _user = await _context.EmployessUsers.Where(x => x.Id == _clientid).FirstOrDefaultAsync();
            var _team = await _context.Teams.Where(x => x.Id == _user.Employee_Team).FirstOrDefaultAsync();

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
                Task_Employee_Open = _clientid,
                Task_part = form.Task_part,
                Task_Price_rewards = form.Task_Price_rewards,
                Tower_Id = form.Tower_Id,
                Task_Note = form.Task_Note,
                Task_EndDate = form.Task_EndDate,
                Task_Status = Task_Status,
                InternetUserId = form.InternetUserId,
                Task_Employee_WorkOn = form.Task_Employee_WorkOn,
                Task_Open = Task_Open,
                Task_Open_Part = _team.Id
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
       
        

    }
}
