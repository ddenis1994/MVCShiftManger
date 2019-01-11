using finalProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace finalProject.Dal
{
    public class MainDal : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Candidate>().ToTable("CandidateTBL");
            modelBuilder.Entity<Job>().ToTable("jobsTBL");
            modelBuilder.Entity<roles>().ToTable("RolesTBL");
            modelBuilder.Entity<shifts.shift>().ToTable("Shifts1TBL");
            modelBuilder.Entity<User>().ToTable("UserTBL");
            modelBuilder.Entity<shifts>().ToTable("WeelShiftsTBL");
            

        }
        



        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<shifts> WeekShifts { get; set; }
        public DbSet<shifts.shift> Shifts1 { get; set; }
        public DbSet<roles> roles { get; set; }
        public DbSet<Job> jobs { get; set; }
    }
}