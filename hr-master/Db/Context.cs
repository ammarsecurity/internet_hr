using hr_master.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr_master.Db
{
    public class Context : DbContext
    {

        public Context(DbContextOptions<Context> options)
           : base(options)
        {
        }


        public virtual DbSet<AdminUsers> AdminUser { get; set; }
        public virtual DbSet<EmployessUsers> EmployessUsers { get; set; }
        public virtual DbSet<Teams> Teams { get; set; }
        public virtual DbSet<InOut> InOut { get; set; }
        public virtual DbSet<Stored> Stored { get; set; }
        public virtual DbSet<StoredParts> StoredParts { get; set; }
        public virtual DbSet<Attachment> Attachment { get; set; }
        public virtual DbSet<Tasks> Tasks { get; set; }
        public virtual DbSet<Towers> Towers { get; set; }
        public virtual DbSet<Penalties> Penalties { get; set; }
        public virtual DbSet<RewardsTable> RewardsTable { get; set; }
        public virtual DbSet<ScheduleDelayPenalties> ScheduleDelayPenalties { get; set; }

        public virtual DbSet<OverTime> OverTime { get; set; }
        public virtual DbSet<OverTimeRewards> OverTimeRewards { get; set; }

        public virtual DbSet<Absence> Absence { get; set; }
        public virtual DbSet<TasksRepalys> TasksRepalys { get; set; }
        public virtual DbSet<AccountersDoor> AccountersDoor { get; set; }
        public virtual DbSet<AccounterInputAndOutput> AccounterInputAndOutput { get; set; }
        public virtual DbSet<InternetUsers> InternetUsers { get; set; }

        public virtual DbSet<User_Complaint> User_Complaint { get; set; }
        public virtual DbSet<StoreMovements> StoreMovements { get; set; }
        public virtual DbSet<Tower_Broadcasting> TowerBroadcasting { get; set; }
        public virtual DbSet<Tower_Electrical> TowerElectrical { get; set; }
        public virtual DbSet<Tower_Notes> TowerNotes { get; set; }
        public virtual DbSet<EmployeeVacations> EmployeeVacations { get; set; }
        public virtual DbSet<TaskFollowers> TaskFollowers { get; set; }
    }
}
