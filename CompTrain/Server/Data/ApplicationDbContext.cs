using CompTrain.Shared.Models.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompTrain.Server.Data
{
    public class ApplicationDbContext : IdentityDbContext<Athlete>
    {
        public DbSet<Benchmark> Benchmarks { get; set; }
        public DbSet<Benchmarkresult> Benchmarkresults { get; set; }
        public DbSet<Plane> Planes { get; set; }
        
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Wod> Wods { get; set; }
        public DbSet<Workout> Workouts { get; set; }
        public DbSet<Workoutresult> Workoutresults { get; set; }
        public DbSet<Workouttype> Workouttypes { get; set; }

        public DbSet<Resulttype> Resulttypes { get; set; }

        public DbSet<RestDay> RestDays { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);          
            
            builder.Entity<Wod>()
                .HasIndex(u => u.Date)
                .IsUnique();
            
        }
    }
}
