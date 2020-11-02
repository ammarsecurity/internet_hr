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

            var Counts = new SupportDashboardCounts { DoneTask = donetask, OpenTask = opentask, Waittask = waittask, MySallery = EmployessSallary, Alltask = Alltask };

            return Ok(new Response
            {
                Message = "ok",
                Data = Counts,
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
