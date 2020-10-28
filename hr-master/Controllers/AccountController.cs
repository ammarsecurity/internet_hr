﻿using System;
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
    [Authorize(Roles = "Account")]
    [EnableCors("cross")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly Context _context;
        private readonly IConfiguration _configuration;
        public AccountController(Context context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }



        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetPenaltiesAsync([FromQuery] PaginationFilter filter, DateTime? date)
        {
         

            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var totalRecords = await _context.Penalties.Where(x => x.IsDelete == false ).CountAsync();

            var list = (from Penalties in _context.Penalties.Where(x => x.IsDelete == false).AsQueryable()
                        join Employee in _context.EmployessUsers.AsQueryable() on Penalties.Penalties_Enterid equals Employee.Id
                        join Employee1 in _context.EmployessUsers.AsQueryable() on Penalties.Employees_Id equals Employee1.Id
                        select new PenaltiesDto
                        {
                            Id = Penalties.Id,
                            Penalties_Enterid = Employee.Employee_Fullname,
                            Employees_Name = Employee1.Employee_Fullname,
                            Penalties_Date = Penalties.Penalties_Date,
                            Penalties_Note = Penalties.Penalties_Note,
                            Penalties_Price = Penalties.Penalties_Price,
                            

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




        [HttpPost]
        public ActionResult<IEnumerable<string>> DeletePenalties([FromBody] Delete form, Guid PenaltiesId)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var Penalties = _context.Penalties.Where(x => x.Id == PenaltiesId).FirstOrDefault();




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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetRewardsAsync([FromQuery] PaginationFilter filter, DateTime? date)
        {
         
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var totalRecords = await _context.Penalties.Where(x => x.IsDelete == false ).CountAsync();

            var list = (from Rewards in _context.OverTimeRewards.Where(x => x.IsDelete == false ).AsQueryable()
                        join Employee in _context.EmployessUsers.AsQueryable() on Rewards.OverTimeRewards_Enterid equals Employee.Id
                        join Employee1 in _context.EmployessUsers.AsQueryable() on Rewards.Employees_Id equals Employee1.Id
                        select new RewardsDto
                        {
                            Id = Rewards.Id,
                            Rewards_Enterid = Employee.Employee_Fullname,
                            Employees_Name = Employee1.Employee_Fullname,
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

        [HttpPost]
        public ActionResult<IEnumerable<string>> DeleteRewards([FromBody] Delete form, Guid RewardsId)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var OverTimeRewards = _context.OverTimeRewards.Where(x => x.Id == RewardsId).FirstOrDefault();




            OverTimeRewards.IsDelete = form.IsDelete;

            _context.Entry(OverTimeRewards).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return Ok(new Response
            {
                Message = "Done !",
                Data = OverTimeRewards,
                Error = false
            });


        }

        [HttpPut]
        public ActionResult<IEnumerable<string>> AddPenalties([FromBody] AddPenalties form)

        {



            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var _clientid = Guid.Parse(currentUserId);


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var NewPenalties = new Penalties
            {
             

                Penalties_Date = form.Penalties_Date,
                Penalties_Enterid = _clientid ,
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



        [HttpPut]
        public ActionResult<IEnumerable<string>> AddRewards([FromBody] OverTimeRewards form)

        {



            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var _clientid = Guid.Parse(currentUserId);


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var NewOverTimeRewards = new OverTimeRewards
            {


               OverTimeRewards_Price = form.OverTimeRewards_Price,
               Employees_Id = form.Employees_Id,
               OverTimeRewards_Date = form.OverTimeRewards_Date,
               OverTimeRewards_Enterid = _clientid,
               OverTimeRewards_Note = form.OverTimeRewards_Note,






            };

            _context.OverTimeRewards.Add(NewOverTimeRewards);

            _context.SaveChanges();
            return Ok(new Response
            {
                Message = "Done !",
                Data = NewOverTimeRewards,
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


        

        [AllowAnonymous]
         [HttpGet]
        public ActionResult<IEnumerable<string>> Fetchingsalaries(DateTime date)
        {


            var list = _context.EmployessUsers.Where(x => x.IsDelete == false).ToList();
            var flist = new List<SalariesDto>();


            foreach (var em in list)
            {
                var overtimereward = _context.OverTimeRewards.Where(x => x.Employees_Id == em.Id && x.OverTimeRewards_Date.Year == date.Year && x.OverTimeRewards_Date.Month == date.Month).ToList();
                decimal rewardsprice = 0;
                foreach (var emrew in overtimereward)
                {

                    rewardsprice += emrew.OverTimeRewards_Price;
                }


                var penalties = _context.Penalties.Where(x => x.Employees_Id == em.Id && x.Penalties_Date.Year == date.Year && x.Penalties_Date.Month == date.Month).ToList();
                decimal penaltiesprice = 0;
                foreach (var empen in penalties)
                {

                    penaltiesprice += empen.Penalties_Price;
                }


                var employeesalayer = new SalariesDto
                {

                    Employee_Name = em.Employee_Fullname,
                    Salary_Date = date,
                    Salary_Name = em.Employee_Saller,
                    Totel_Penalties = decimal.ToInt32(penaltiesprice),
                    Totel_Rewareds = decimal.ToInt32(rewardsprice),
                    Totel_Salary = Convert.ToDecimal((em.Employee_Saller + rewardsprice) - penaltiesprice),



                };


                flist.Add(employeesalayer);
            }




            return Ok(new Response
            {
                Message = "Done !",
                Data = flist,
                Error = false
            });


        }



        [HttpPut]
        public ActionResult<IEnumerable<string>> AddAccountersDoor([FromBody] AddAccountersDoor form)

        {



            


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var NewAccountersDoor = new AccountersDoor
            {


                AccounDoor_Info = form.AccounDoor_Info,
                AccounDoor_Name = form.AccounDoor_Name,
                AccounDoor_Status = form.AccounDoor_Status

            };

            _context.AccountersDoor.Add(NewAccountersDoor);

            _context.SaveChanges();
            return Ok(new Response
            {
                Message = "Done !",
                Data = NewAccountersDoor,
                Error = false
            });
        }



        [HttpPost]
        public ActionResult<IEnumerable<string>> EditAccountersDoor([FromBody] AddAccountersDoor form, Guid PartId)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);



            var AccountersDoor = _context.AccountersDoor.Where(x => x.Id == PartId).FirstOrDefault();




            AccountersDoor.AccounDoor_Info = form.AccounDoor_Info;
            AccountersDoor.AccounDoor_Name = form.AccounDoor_Name;
            AccountersDoor.AccounDoor_Status = form.AccounDoor_Status;


            _context.Entry(AccountersDoor).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return Ok(new Response
            {
                Message = "Done !",
                Data = AccountersDoor,
                Error = false
            });


        }



        [HttpPut]
        public ActionResult<IEnumerable<string>> AddInputAndOutput([FromBody] AddInputAndOutput form)

        {


            var inoutdoor = _context.AccountersDoor.Where(x => x.Id == form.InputAndOutput_Door).FirstOrDefault();



            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var NewInputAndOutput = new AccounterInputAndOutput
            {
                InputAndOutput_Date = form.InputAndOutput_Date ,
                InputAndOutput_Door = form.InputAndOutput_Door,
                InputAndOutput_Note = form.InputAndOutput_Note,
                InputAndOutput_Price = form.InputAndOutput_Price,
                InputAndOutput_Status = inoutdoor.AccounDoor_Status,
            };

            _context.AccounterInputAndOutput.Add(NewInputAndOutput);

            _context.SaveChanges();
            return Ok(new Response
            {
                Message = "Done !",
                Data = NewInputAndOutput,
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
        public ActionResult<IEnumerable<string>> GetAllAccountersDoor()
        {


            var list = _context.AccountersDoor.Where(x => x.IsDelete == false).ToList();



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
            var totalRecords = await _context.Tasks.Where(x => x.IsDelete == false && x.Task_Employee_Open == _clientid).CountAsync();


            var list = (from task in _context.Tasks.OrderByDescending(x => x.Task_Date).Where(x => x.IsDelete == false && x.Task_Employee_Open == _clientid).AsNoTracking()
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
        public async Task<ActionResult<IEnumerable<string>>> GetInputAndOutput([FromQuery] PaginationFilter filter, DateTime? date , int? InputAndOutputStatus)
        {


            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var totalRecords = await _context.AccounterInputAndOutput.Where(x => x.IsDelete == false).CountAsync();

            var list = (from InputAndOutput in _context.AccounterInputAndOutput.OrderByDescending(x => x.InputAndOutput_Date).Where(x => x.IsDelete == false).AsQueryable()
                        join Door in _context.AccountersDoor.AsQueryable() on InputAndOutput.InputAndOutput_Door equals Door.Id
                        select new InputAndOutputDto
                        {
                            Id = InputAndOutput.Id,
                            InputAndOutput_Date = InputAndOutput.InputAndOutput_Date,
                            InputAndOutput_Door = Door.AccounDoor_Name,
                            InputAndOutput_Note = InputAndOutput.InputAndOutput_Note,
                            InputAndOutput_Price = InputAndOutput.InputAndOutput_Price,
                            InputAndOutput_Status = InputAndOutput.InputAndOutput_Status,


                        }).ToList();




            if (InputAndOutputStatus != null && InputAndOutputStatus != default)
                list = list.Where(s => s.InputAndOutput_Status == InputAndOutputStatus).ToList();
            totalRecords = list.Count();
            if (date != null && date != default)
                list = list.Where(s => s.InputAndOutput_Date.Date == date).ToList();
            totalRecords = list.Count();
         

            return Ok(new PagedResponse<List<InputAndOutputDto>>(
                list.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToList(),
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
        public ActionResult<IEnumerable<string>> AddTasks([FromBody] AddTaskes form)

        {
            var time = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
            var time1 = time.AddHours(3);
            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var _clientid = Guid.Parse(currentUserId);




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
                Task_Status = 1,
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





        [HttpPut]
        public ActionResult<IEnumerable<string>> AddInternetUser([FromBody] AddInternetUser form)

        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var internetusers = _context.InternetUsers.Where(x => x.User_Name == form.User_Name && x.IsDelete == false).FirstOrDefault();

            if(internetusers != null)
            {
                return BadRequest(new Response
                {
                    Message = "الحساب موجود مسبقا",
                    Data = internetusers,
                    Error = true
                });
            }



            var internetuser = new InternetUsers
            {
                User_FullName = form.User_FullName,
                User_Name = form.User_Name,
                User_ActiveDate = form.User_ActiveDate,
                User_Card = form.User_Card,
                User_EndDate = form.User_EndDate,
                User_Password = form.User_Password,
                User_Price = form.User_Price,
                User_Phone = form.User_Phone,
                IsActive = true,
                User_Adress = form.User_Adress,
                User_Tower = form.User_Tower

            };

            _context.InternetUsers.Add(internetuser);

            _context.SaveChanges();
            return Ok(new Response
            {
                Message = "Done !",
                Data = internetuser,
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
                            User_Tower  = iuser.User_Tower

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
        public ActionResult<IEnumerable<string>> EditInternetUser([FromBody] AddInternetUser form, Guid InternetUserId)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);



            var InternetUsers = _context.InternetUsers.Where(x => x.Id == InternetUserId).FirstOrDefault();




            InternetUsers.User_ActiveDate = form.User_ActiveDate;
            InternetUsers.User_Card = form.User_Card;
            InternetUsers.User_FullName = form.User_FullName;
            InternetUsers.User_Name = form.User_Name;
            InternetUsers.User_Password = form.User_Password;
            InternetUsers.User_Phone = form.User_Phone;
            InternetUsers.User_Price = form.User_Price;
            InternetUsers.User_EndDate = form.User_EndDate;
            InternetUsers.User_Tower = form.User_Tower;
            InternetUsers.User_Adress = form.User_Adress;



            _context.Entry(InternetUsers).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return Ok(new Response
            {
                Message = "Done !",
                Data = InternetUsers,
                Error = false
            });


        }




        [HttpPost]
        public ActionResult<IEnumerable<string>> Sendnotifications( Guid InternetUserId , string notifications)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);



          //  var InternetUsers = _context.InternetUsers.Where(x => x.Id == InternetUserId).FirstOrDefault();



            
            var noitictioneform = new NotificationsForm
            {

                contents = notifications ,
                headings = "يرجى الانتباه",
                url = "https://sys.center-wifi.com",
                ///included_segments = "All",
                include_external_user_ids = InternetUserId.ToString()


            };



            _ = SendNoiticationsforuser(noitictioneform);

            return Ok(new Response
            {
                Message = "Done !",
                Data = noitictioneform,
                Error = false
            });


        }



        [HttpPost]
        public ActionResult<IEnumerable<string>> ActiveInternetUser([FromBody] ActiveInternetUser form, Guid InternetUserId)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);



            var InternetUsers = _context.InternetUsers.Where(x => x.Id == InternetUserId).FirstOrDefault();




            InternetUsers.IsActive= true;
            InternetUsers.User_ActiveDate = form.User_ActiveDate;
            InternetUsers.User_EndDate = form.User_EndDate;
            InternetUsers.User_Card = form.User_Card;
            InternetUsers.User_Price = form.User_Price;



            _context.Entry(InternetUsers).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return Ok(new Response
            {
                Message = "Done !",
                Data = InternetUsers,
                Error = false
            });


        }


        [HttpPost]
        public ActionResult<IEnumerable<string>> DeleteInternetUser([FromBody] Delete form, Guid InternetUserId)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var InternetUser = _context.InternetUsers.Where(x => x.Id == InternetUserId).FirstOrDefault();




            InternetUser.IsDelete = form.IsDelete;

            _context.Entry(InternetUser).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return Ok(new Response
            {
                Message = "Done !",
                Data = InternetUser,
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



        [HttpPost]
        public ActionResult<IEnumerable<string>> DeletePart([FromBody] Delete form, Guid PartId)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var AccountersDoor = _context.AccountersDoor.Where(x => x.Id == PartId).FirstOrDefault();




            AccountersDoor.IsDelete = form.IsDelete;

            _context.Entry(AccountersDoor).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return Ok(new Response
            {
                Message = "Done !",
                Data = AccountersDoor,
                Error = false
            });


        }




        [HttpPost]
        public ActionResult<IEnumerable<string>> DeleteInputAndOutput([FromBody] Delete form, Guid InputAndOutput)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var InputAndOutputlist = _context.AccounterInputAndOutput.Where(x => x.Id == InputAndOutput).FirstOrDefault();




            InputAndOutputlist.IsDelete = form.IsDelete;

            _context.Entry(InputAndOutputlist).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return Ok(new Response
            {
                Message = "Done !",
                Data = InputAndOutputlist,
                Error = false
            });


        }




        [HttpGet]
        public ActionResult<IEnumerable<string>> dashbardcounts()
        {
            string Month = DateTime.Now.Month.ToString();
            string Year = DateTime.Now.Year.ToString();
            string DAY = DateTime.Now.Day.ToString();
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




            var ExpensesTodayList = _context.AccounterInputAndOutput.Where(x => x.IsDelete == false && x.InputAndOutput_Date.Month.ToString() == Month && x.InputAndOutput_Date.Year.ToString() == Year && x.InputAndOutput_Date.Day.ToString() == DAY  && x.InputAndOutput_Status == 0).ToList();
            decimal ExpensesToday = 0;
            foreach (var i in ExpensesTodayList)
            {

                ExpensesToday += i.InputAndOutput_Price;
            }


            var RevenuesTodayList = _context.AccounterInputAndOutput.Where(x => x.IsDelete == false && x.InputAndOutput_Date.Month.ToString() == Month && x.InputAndOutput_Date.Year.ToString() == Year && x.InputAndOutput_Date.Day.ToString() == DAY && x.InputAndOutput_Status == 1).ToList();
            decimal RevenuesToday = 0;
            foreach (var i in RevenuesTodayList)
            {

                RevenuesToday += i.InputAndOutput_Price;
            }



            var EmployessSallary = _context.EmployessUsers.Where(x => x.IsDelete == false).ToList();
            decimal Sallary = 0;
            foreach (var i in EmployessSallary)
            {

                Sallary += i.Employee_Saller;
            }

            var Employees = _context.EmployessUsers.Where(x => x.IsDelete == false && x.IsDisplay == false).Count();

            var Counts = new AccountDashboradCount
            {

                Employeescount = Employees ,
                SallerySum = Sallary, 
                thismonthOverTimeReawradSum = OverTimeReawradSum,
                thismonthpenaltiesSun = penaltiesSum,
                thismonthExpenses = Expenses,
                thismonthRevenues = Revenues ,
                ExpensesToday = ExpensesToday ,
                RevenuesToday = RevenuesToday ,
            };

            return Ok(new Response
            {
                Message = "ok",
                Data = Counts,
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



                _ = SendNoiticationsforusers(noitictioneform);

            }




            return Ok(new Response
            {
                Message = "Done !",
                Data = from,
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
