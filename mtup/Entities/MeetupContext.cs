using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mtup.Entities
{
    public class MeetupContext : DbContext
    {
        private readonly string _connectionString = "Server=PLG1503X2\\SQLEXPRESS;Database=MeetupDb;Trusted_Connection=True;";
        
        //"Server=PLG1503X2\\SQLEXPRESS;Database=MeetupDb;Trusted_Connection=True;"
        //"Server=(localdb)\\mssqllocaldb;Database=MeetupDb;Trusted_Connection=True;"
        public DbSet<Meetup> Meetups { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Lecture> Lectures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Meetup>()
                .HasOne(m => m.Location)
                .WithOne(l => l.Meetup)
                .HasForeignKey<Location>(k => k.MeetupId);

            modelBuilder.Entity<Meetup>()
                .HasMany(m => m.Lectures)
                .WithOne(l => l.Meetup);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
