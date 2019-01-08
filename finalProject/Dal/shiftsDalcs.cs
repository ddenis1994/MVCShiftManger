using finalProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace finalProject.Dal
{
    public class shiftsDalcs : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<shifts.shift>().ToTable("Shifts1");
        }

        public DbSet<shifts.shift> Shifts { get; set; }
    }
}