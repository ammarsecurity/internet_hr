using hr_master.Filter;
using hr_master.Models;
using hr_master.Models.Dto;
using hr_master.Models.Form;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Interface
{
  
    public interface IEmployeeAddTasks : IDisposable
    {
        void InsertTask(Tasks Taskform);
        //void Update(AddTaskes specification);
        // void Update(Invoice invoice);
        void Save();

       
    }
}
