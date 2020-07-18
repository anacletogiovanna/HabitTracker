using HabitTracker.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HabitTracker.Infra
{
    public class HabitTrackerContext : DbContext
    {
        //DbSet to create classes in SQL
        public DbSet<Habit> Habits { get; set; }
        public DbSet<Work> Works { get; set; }

        public HabitTrackerContext()
        {
            Database.EnsureCreated();
        }

        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            //configure string conection(azure sql)
            optionsBuilder.UseSqlServer("Server=tcp:habittracker-db-server.database.windows.net,1433;Initial Catalog=HabitTracker-db;Persist Security Info=False;User ID=giovanna;Password=@dsInf123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"); 

        }
    }
}
