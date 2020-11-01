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

namespace hr_master.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize(Roles = "Technical")]
    [EnableCors("cross")]
    [ApiController]
    public class TechnicalController : ControllerBase
    {

        private readonly Context _context;
        private readonly IConfiguration _configuration;
        public TechnicalController(Context context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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
        public async Task<ActionResult<IEnumerable<string>>> GetAllTower([FromQuery] PaginationFilter filter)
        {

            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var totalRecords = await _context.Towers.Where(x => x.IsDelete == false).CountAsync();
            var list = _context.Towers.Where(x => x.IsDelete == false).ToList();



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


        //[HttpPut]
        //public ActionResult<IEnumerable<string>> AddTasks([FromBody] AddTaskes form)

        //{

        //    string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    var _clientid = Guid.Parse(currentUserId);




        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    var NewTask = new Tasks
        //    {
        //        Task_Title = form.Task_Title,
        //        Task_Date = form.Task_Date,
        //        Task_Employee_Open = _clientid,
        //        Task_part = form.Task_part,
        //        Task_Price = form.Task_Price,
        //        Tower_Id = form.Tower_Id,
        //        Task_Note = form.Task_Note,
        //        Task_EndDate = form.Task_EndDate,
        //        Task_Status = 1 ,





        //    };

        //    _context.Tasks.Add(NewTask);

        //    _context.SaveChanges();
        //    return Ok(new Response
        //    {
        //        Message = "Done !",
        //        Data = NewTask,
        //        Error = false
        //    });
        //}

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
        public ActionResult<IEnumerable<string>> AddfollowerTask([FromBody] AddTaskFollowers form)

        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var followers = _context.TaskFollowers.Where(x => x.EmployeeId == form.EmployeeId).FirstOrDefault();
            if(followers != null)
            {

                return Ok(new Response
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

            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var _clientid = Guid.Parse(currentUserId);
            var time = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
            var time1 = time.AddHours(3);
            var Task = _context.Tasks.Where(x => x.Id == TaskId).FirstOrDefault();




            Task.Task_Employee_WorkOn  = _clientid ;
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
            var userpart = _context.EmployessUsers.Where(x => x.Id == _clientid).FirstOrDefault();

            var Alltask = _context.Tasks.Where(x => x.IsDelete == false && x.Task_part == userpart.Employee_Team).Count();
            var opentask = _context.Tasks.Where(x => x.IsDelete == false && x.Task_Status == 2 && x.Task_part == userpart.Employee_Team).Count();
            var waittask = _context.Tasks.Where(x => x.IsDelete == false && x.Task_Status == 1 && x.Task_part == userpart.Employee_Team).Count();
            var donetask = _context.Tasks.Where(x => x.IsDelete == false && x.Task_Status == 3 && x.Task_part == userpart.Employee_Team).Count();
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

    }
}
