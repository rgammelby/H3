global using System.ComponentModel.DataAnnotations;
using LagerstyringClassLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LagerstyringClassLibrary
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        public DbSet<ActivityType> ActivityTypes { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<DeviceOverview> DeviceOverview { get; set; }
        public DbSet<DeviceType> DeviceTypes { get; set; }
        //public DbSet<Location> Locations { get; set; }
        public DbSet<LocationCupboard> Cupboards { get; set; }
        public DbSet<LocationRoom> Rooms { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<SingleDevice> SingleDevices { get; set; }
        public DbSet<StatusType> StatusTypes { get; set; }
        public DbSet<User> Users { get; set; }

        // OnModelCreating for instituting foreign keys
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Activity - ActivityType (Many-to-One)
            modelBuilder.Entity<Activity>()
                .HasOne(a => a.ActivityType)
                .WithMany(at => at.Activities)
                .HasForeignKey(a => a.activity_type);

            // Activity - SingleDevice (Many-to-One)
            modelBuilder.Entity<Activity>()
                .HasOne(a => a.SingleDevice)
                .WithMany(d => d.Activities)
                .HasForeignKey(a => a.device_id);

            // Activity - User (Many-to-One)
            modelBuilder.Entity<Activity>()
                .HasOne(a => a.User)
                .WithMany(u => u.Activities)
                .HasForeignKey(a => a.user_id);

            // DeviceOverview - DeviceType (Many-to-One)
            modelBuilder.Entity<DeviceOverview>()
                .HasOne(d => d.DeviceType)
                .WithMany(dt => dt.Overviews)
                .HasForeignKey(d => d.device_type);

            // SingleDevice - StatusType (Many-to-One)
            modelBuilder.Entity<SingleDevice>()
                .HasOne(d => d.Statuses)
                .WithMany(st => st.Devices)
                .HasForeignKey(d => d.status);

            // SingleDevice - DeviceOverview (Many-to-One)
            modelBuilder.Entity<SingleDevice>()
                .HasOne(sd => sd.DeviceOverview)
                .WithMany(d => d.Devices)
                .HasForeignKey(s => s.device_overview_id);

            // UNIQUE and INDEX constraints for User and User column email
            modelBuilder.Entity<User>()
                .HasIndex(u => u.email)
                .IsUnique();

            // SingleDevice - LocationCupboard (Many-to-One)
            modelBuilder.Entity<SingleDevice>()
                .HasOne(sd => sd.Location)
                .WithMany(c => c.Devices)
                .HasForeignKey(sd => sd.location);

            // LocationCupboard - LocationRoom (Many-to-One)
            modelBuilder.Entity<LocationCupboard>()
                .HasOne(c => c.Room)
                .WithMany(r => r.Cupboards)
                .HasForeignKey(c => c.room_id);

            // UNIQUE and INDEX constraints for User and User column email
            modelBuilder.Entity<User>()
                .HasIndex(u => u.email)
                .IsUnique();

            // UNIQUE for Activity column lifecycle_id
            modelBuilder.Entity<Activity>()
                .HasIndex(a => a.lifecycle_id);

            // UNIQUE for DeviceOverview column name (model) 
            modelBuilder.Entity<DeviceOverview>()
                .HasIndex(d => d.device_type);

            // necessary data insert for tables StatusType and ActivityType
            modelBuilder.Entity<StatusType>().HasData(
                 new StatusType { id = 1, status_type = "Available" },
                 new StatusType { id = 2, status_type = "Overdue" },
                 new StatusType { id = 3, status_type = "Borrowed" },
                 new StatusType { id = 4, status_type = "Unavailable" }
             );

            modelBuilder.Entity<StatusType>()
                .Property(st => st.id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<ActivityType>().HasData(
                new ActivityType { id = 1, activity_type = "Borrow" },
                new ActivityType { id = 2, activity_type = "Return" },
                new ActivityType { id = 3, activity_type = "Extend" },
                new ActivityType { id = 4, activity_type = "Late" }
            );

            modelBuilder.Entity<ActivityType>()
                .Property(st => st.id)
                .ValueGeneratedOnAdd();
        }

        public string conn = "Server=SUS-EL-TWAN1;Database=Lagerstyring;Trusted_Connection=True;TrustServerCertificate=True";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(conn);
            }
        }
    }
}
