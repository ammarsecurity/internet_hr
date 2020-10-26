using hr_master.Db;
using hr_master.Models;
using hr_master.Models.Form;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Class
{
    public class AbsenceClass
    {
        private readonly Context _context;


        public AbsenceClass(Context context)
        {
            _context = context;

        }


        public AbsenceClass()
        {
        }


        public void checkAbsenceClass1()
        {


            var employees = _context.EmployessUsers.Where(x => x.IsDelete == false && x.IsDisplay == false).ToList();
            var admininfo = _context.AdminUser.FirstOrDefault();
            DateTime date = Convert.ToDateTime(DateTime.Now).Date;



            foreach (var i in employees)
            {

                var vacation = _context.EmployeeVacations.Where(x => x.EmployeeId == i.Id && x.StartDate.Date <= date && x.EndDate >= date).FirstOrDefault();


                if (vacation == null)
                {
                    var inemployee = _context.InOut.Where(x => x.In_Out_Date.Date == date && x.EmplyeeId == i.Id && x.In_Out_Status == 0).FirstOrDefault();


                    if (inemployee != null)
                    {

                        var inout = _context.InOut.Where(x => x.In_Out_Date.Date == date && x.EmplyeeId == i.Id && x.In_Out_Status == 1).FirstOrDefault();

                        if (inout == null)
                        {

                            var addabsence = new Absence
                            {

                                AbsenceDate = DateTime.Now,
                                EmployeeId = i.Id,

                            };

                            _context.Absence.Add(addabsence);


                            var AddPenalties = new Penalties
                            {

                                Penalties_Date = DateTime.Now,
                                Penalties_Note = "غياب - عدم وجود بصمة الخروج ",
                                Penalties_Price = admininfo.Delay_penalty,
                                Employees_Id = i.Id,
                                Penalties_Enterid = Guid.Parse("3ef34045-bbbb-49e6-880e-7e7bcb9c9a16"),

                            };

                            _context.Penalties.Add(AddPenalties);




                            _context.SaveChanges();

                        }


                    }

                }
            }




        }

        public void checkAbsenceClass()
        {


            var employees = _context.EmployessUsers.Where(x => x.IsDelete == false && x.IsDisplay == false).ToList();
            var admininfo = _context.AdminUser.FirstOrDefault();
            DateTime date = Convert.ToDateTime(DateTime.Now).Date;
          


            foreach(var i in employees)
            {

                var vacation = _context.EmployeeVacations.Where(x => x.EmployeeId == i.Id && x.StartDate.Date <= date && x.EndDate >= date).FirstOrDefault();


                if(vacation == null)
                {

                    var inout = _context.InOut.Where(x => x.In_Out_Date.Date == date && x.EmplyeeId == i.Id && x.In_Out_Status == 0).FirstOrDefault();

                    if (inout == null)
                    {

                        var addabsence = new Absence
                        {

                            AbsenceDate = DateTime.Now,
                            EmployeeId = i.Id,

                        };

                        _context.Absence.Add(addabsence);


                        var AddPenalties = new Penalties
                        {

                            Penalties_Date = DateTime.Now,
                            Penalties_Note = "غياب",
                            Penalties_Price = admininfo.Delay_penalty,
                            Employees_Id = i.Id,
                            Penalties_Enterid = Guid.Parse("3ef34045-bbbb-49e6-880e-7e7bcb9c9a16"),

                        };

                        _context.Penalties.Add(AddPenalties);




                        _context.SaveChanges();

                    }



                }


               


            }




        }

    }
}
