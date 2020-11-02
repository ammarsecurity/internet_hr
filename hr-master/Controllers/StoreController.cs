using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Formatting;
using System.Security.Claims;
using System.Security.Cryptography;
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
    [Authorize(Roles = "Store")]
    [EnableCors("cross")]
    [ApiController]
    public class StoreController : ControllerBase
    {

        private readonly Context _context;
        private readonly IConfiguration _configuration;
        public StoreController(Context context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        [HttpPut]
        public ActionResult<IEnumerable<string>> AddStoreParts([FromBody] AddStoreParts form)

        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

           

            var StoreParts = _context.StoredParts.Where(x => x.PartName == form.PartName).FirstOrDefault();
            if (StoreParts != null)
            {
                return BadRequest(new Response
                {
                    Message = "اسم القسم موجود سابقا",
                    Data = "",
                    Error = true
                });

            }
            var Storeparts = new StoredParts
            {
               PartName = form.PartName,
               PartNote = form.PartNote
            };

            _context.StoredParts.Add(Storeparts);

            _context.SaveChanges();
            return Ok(new Response
            {
                Message = "Done !",
                Data = Storeparts,
                Error = false
            });
        }


        [HttpGet]
        public ActionResult<IEnumerable<string>> GetAllParts()
        {




            var list = _context.StoredParts.Where(x => x.IsDelete == false).ToList();

            return Ok(new Response
            {
                Message = "Done !",
                Data = list,
                Error = false
            });


        }





        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetAllItemsAsync([FromQuery] PaginationFilter filter , string itemname, DateTime? date, string employeename)
        {

            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var _clientid = Guid.Parse(currentUserId);
            var list = new List<ItemsStoreDto>();


            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var totalRecords = await _context.Stored.Where(x => x.IsDelete == false).CountAsync();


            var itemsloop = _context.Stored.Where(x => x.IsDelete == false).ToList();

            foreach (var i in itemsloop)
            {

                var WithdrawalCount = _context.StoreMovements.Where(x => x.ItemId == i.Id && x.Movement_type == 1 && x.IsDelete == false).ToList();
                int WithdrawalCounts = 0;
                foreach (var imo in WithdrawalCount)
                {

                    WithdrawalCounts += imo.Movement_Count;

                }
                var DepositCout = _context.StoreMovements.Where(x => x.ItemId == i.Id && x.Movement_type == 2 && x.IsDelete == false).ToList();
                int Depositcounts = 0;
                foreach (var Deposit in DepositCout)
                {

                    Depositcounts += Deposit.Movement_Count;

                }
                var items = (from item in itemsloop.Where(x => x.Id == i.Id)
                             join employee in _context.EmployessUsers on i.Item_EmployeeEntery equals employee.Id
                             join itempart in _context.StoredParts on i.Item_Part equals itempart.Id

                             select new ItemsStoreDto
                             {
                                 Id = item.Id,
                                 Item_Name = item.Item_Name,
                                 Item_EmployeeEntery = item.Item_EmployeeEntery,
                                 Item_EmployeeEntery_Name = employee.Employee_Fullname,
                                 Item_EnteryDate = item.Item_EnteryDate,
                                 Item_IsUsed = item.Item_IsUsed,
                                 Item_Model = item.Item_Model,
                                 Item_Part = item.Item_Part,
                                 PartName = itempart.PartName,
                                 Item_SerialNumber = item.Item_SerialNumber,
                                 Item_Count = (item.Item_Count + Depositcounts ) - WithdrawalCounts,
                             }).ToList();

                if (itemname != null && itemname != default)
                    items = items.Where(s => s.Item_Name.Contains(itemname)).ToList();
                     totalRecords = items.Count();
                if (employeename != null && employeename != default)
                    items = items.Where(s => s.Item_EmployeeEntery_Name.Contains(employeename)).ToList();
                    totalRecords = items.Count();
                if (date != null && date != default)
                    items = items.Where(s => s.Item_EnteryDate.Date <= date).ToList();
                    totalRecords = items.Count();


                list.AddRange(items);
        }

          


            return Ok(new PagedResponse<List<ItemsStoreDto>>(
               list.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToList(),
               validFilter.PageNumber,
               validFilter.PageSize,
               totalRecords, 0, 0));



        }





        [HttpPost]
        public ActionResult<IEnumerable<string>> Deletepart([FromBody] Delete form, Guid PartId)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var part = _context.StoredParts.Where(x => x.Id == PartId).FirstOrDefault();





            part.IsDelete = form.IsDelete;

            _context.Entry(part).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return Ok(new Response
            {
                Message = "Done !",
                Data = part,
                Error = false
            });


        }

        [HttpPost]
        public ActionResult<IEnumerable<string>> EditPartStore([FromBody] EditStorePart form, Guid PartId)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var part = _context.StoredParts.Where(x => x.Id == PartId).FirstOrDefault();




            part.PartName = form.PartName;
            part.PartNote = form.PartNote;

           



            _context.Entry(part).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return Ok(new Response
            {
                Message = "Done !",
                Data = part,
                Error = false
            });


        }


        [HttpPut]
        public ActionResult<IEnumerable<string>> AddItemToStore([FromBody] AddItemToStore form)

        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var _clientid = Guid.Parse(currentUserId);

            var AddItem = new Stored
            {
                Item_SerialNumber = form.Item_SerialNumber,
                Item_Count = form.Item_Count,
                Item_EmployeeEntery = _clientid,
                Item_EnteryDate = form.Item_EnteryDate,
                Item_IsUsed = form.Item_IsUsed,
                Item_Model = form.Item_Model,
                Item_Name = form.Item_Name,
                Item_Part = form.Item_Part


            };

            _context.Stored.Add(AddItem);

            _context.SaveChanges();
            return Ok(new Response
            {
                Message = "Done !",
                Data = AddItem,
                Error = false
            });
        }

       


        [HttpPost]
        public ActionResult<IEnumerable<string>> EditStore([FromBody] StoreDto form, Guid ItemId)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var storeitem = _context.Stored.Where(x => x.Id == ItemId).FirstOrDefault();




            storeitem.Item_Name = form.Item_Name;
            storeitem.Item_Part = form.Item_Part;
            storeitem.Item_SerialNumber = form.Item_SerialNumber;
            storeitem.Item_Model = form.Item_Model;
            storeitem.Item_IsUsed = form.Item_IsUsed;
            storeitem.Item_Count = form.Item_Count;



            _context.Entry(storeitem).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return Ok(new Response
            {
                Message = "Done !",
                Data = storeitem,
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
        public ActionResult<IEnumerable<string>> DeleteItem([FromBody] Delete form, Guid ItemId)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var Item = _context.Stored.Where(x => x.Id == ItemId).FirstOrDefault();





            Item.IsDelete = form.IsDelete;

            _context.Entry(Item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return Ok(new Response
            {
                Message = "Done !",
                Data = Item,
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






        [HttpPut]
        public ActionResult<IEnumerable<string>> AddMovement([FromBody] AddMovements form)

        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var _clientid = Guid.Parse(currentUserId);

            var AddMovement = new StoreMovements
            {
               Movement_Count = form.Movement_Count,
               Movement_date = DateTime.Now,
               Movement_Note = form.Movement_Note,
               Movement_Received =form.Movement_Received,
               Movement_type = form.Movement_type,
               Movment_Employee = _clientid ,
               ItemId = form.ItemId,



            };

            _context.StoreMovements.Add(AddMovement);

            _context.SaveChanges();
            return Ok(new Response
            {
                Message = "Done !",
                Data = AddMovement,
                Error = false
            });
        }



        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetAllMovementsAsync([FromQuery] PaginationFilter filter , string itemname, DateTime? date, string employeename , int? Movement_type)
        {

            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var _clientid = Guid.Parse(currentUserId);

            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var totalRecords = await _context.StoreMovements.Where(x => x.IsDelete == false).CountAsync();

            var items = (from move in _context.StoreMovements.Where(x => x.IsDelete == false).AsQueryable()
                         join employee in _context.EmployessUsers on move.Movment_Employee equals employee.Id
                         join item in _context.Stored on move.ItemId equals item.Id
                         select new MovmentsDto
                         {
                            
                             Movement_Count = move.Movement_Count,
                             Movment_Employee = employee.Employee_Fullname,
                             Movement_date = move.Movement_date,
                             Movement_Note = move.Movement_Note,
                             Movement_Received = move.Movement_Received,
                             Movement_type = move.Movement_type,
                             Item_Name = item.Item_Name,
                             Id = move.Id
                             
                             


                         }).ToList();
            if (itemname != null && itemname != default)
                items = items.Where(s => s.Item_Name.Contains(itemname)).ToList();
            totalRecords = items.Count();
            if (employeename != null && employeename != default)
                items = items.Where(s => s.Movment_Employee.Contains(employeename)).ToList();
            totalRecords = items.Count();
            if (date != null && date != default)
                items = items.Where(s => s.Movement_date.Date <= date).ToList();
            totalRecords = items.Count();
            if (Movement_type != null && Movement_type != default)
                items = items.Where(s => s.Movement_type == Movement_type).ToList();
            totalRecords = items.Count();

            return Ok(new PagedResponse<List<MovmentsDto>>(
               items.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToList(),
               validFilter.PageNumber,
               validFilter.PageSize,
               totalRecords, 0, 0));



        }



        [HttpPost]
        public ActionResult<IEnumerable<string>> DeleteMovemnet ([FromBody] Delete form, Guid MovemnetId)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var Item = _context.StoreMovements.Where(x => x.Id == MovemnetId).FirstOrDefault();





            Item.IsDelete = form.IsDelete;

            _context.Entry(Item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return Ok(new Response
            {
                Message = "Done !",
                Data = Item,
                Error = false
            });


        }





        [HttpGet]
        public ActionResult<IEnumerable<string>> dashbardcounts()
        {


            string Month = DateTime.Now.Month.ToString();
            string Year = DateTime.Now.Year.ToString();
           

            string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var _clientid = Guid.Parse(currentUserId);


            var rawards = _context.OverTimeRewards.Where(x => x.Employees_Id == _clientid && x.OverTimeRewards_Date.Month.ToString() == Month && x.OverTimeRewards_Date.Year.ToString() == Year ).ToList();
            decimal amontrawrad = 0;
            foreach(var rew in rawards)
            {

                amontrawrad += rew.OverTimeRewards_Price;

            }


            var penalties = _context.Penalties.Where(x => x.Employees_Id == _clientid && x.Penalties_Date.Month.ToString() == Month && x.Penalties_Date.Year.ToString() == Year).ToList();
            decimal amontpenalties = 0;
            foreach (var pen in penalties)
            {

                amontpenalties += pen.Penalties_Price;

            }



            var store = _context.Stored.Where(x => x.IsDelete == false).Count();
            var storemovment = _context.StoreMovements.Where(x => x.IsDelete == false).Count();
            var EmployessNameSallary = _context.EmployessUsers.Where(x => x.Id == _clientid).FirstOrDefault();

            decimal EmployessSallary = (EmployessNameSallary.Employee_Saller + amontrawrad) - amontpenalties;



            var Counts = new StoreDashboardCount { AllItems = store, AllMovments = storemovment, MySallery = EmployessSallary };

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
