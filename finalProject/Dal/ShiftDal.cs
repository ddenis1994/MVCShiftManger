using finalProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace finalProject.Dal
{
    public class ShiftDal : DbContext

    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<shifts>().ToTable("Shifts");
        }

        public DbSet<shifts> shifts { get; set; }
    }
}