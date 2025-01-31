using Microsoft.EntityFrameworkCore;
using LagerSystemApi.Models.Domain;

namespace LagerSystemApi.Data
{
    public class LagerSystemDbContext: DbContext
    {
        public LagerSystemDbContext(DbContextOptions<LagerSystemDbContext> options): base(options) { }

        public DbSet<DeviceType> DeviceTypes {  get; set; }
        public DbSet<DeviceOverview> DeviceOverviews { get; set; }
        public DbSet<SingleDevice> Devices { get; set; }
        public DbSet<ActivityType> ActivityTypes { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<LocationCupboard> LocationCupboards { get; set; }
        public DbSet<LocationRoom> LocationRooms { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<StatusType> StatusTypes { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Devicetype table
            modelBuilder.Entity<DeviceType>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.name).HasMaxLength(124).IsRequired();
                entity.Property(e => e.type).IsRequired();
                entity.Property(e => e.qty);
                entity.Property(e => e.img).IsRequired();
            });

            // SingleDevice table
            modelBuilder.Entity<SingleDevice>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.deviceOverview_id).HasMaxLength(124).IsRequired();
                entity.Property(e => e.is_archived).IsRequired();
                entity.Property(e => e.description).IsRequired();
                entity.Property(e => e.status).IsRequired();
                entity.Property(e => e.location_id).IsRequired();
                entity.Property(e => e.qr).IsRequired();
            });

            // DeviceOverview table
            modelBuilder.Entity<DeviceOverview>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.device_type).IsRequired();
                entity.Property(e => e.model).IsRequired();
                entity.Property(e => e.available_qty).IsRequired();
                entity.Property(e => e.qty).IsRequired();
                entity.Property(e => e.image);
                entity.Property(e => e.last_ordered).IsRequired();
            });

            // StatusType table
            modelBuilder.Entity<StatusType>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.status_type).IsRequired();
            });

            // ActivityHistory table
            modelBuilder.Entity<Activity>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.user_id).IsRequired();
                entity.Property(e => e.device_id).IsRequired();
                entity.Property(e => e.activity_type).IsRequired();
                entity.Property(e => e.start_date).IsRequired();
                entity.Property(e => e.end_date).IsRequired();
                entity.Property(e => e.created_at).IsRequired();
                entity.Property(e => e.notes).IsRequired();
                entity.Property(e => e.lifecycle_id).IsRequired();
            });

            // ActivityType table
            modelBuilder.Entity<ActivityType>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.activty_type);
            });

            // User table
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.first_name).IsRequired();
                entity.Property(e => e.last_name).IsRequired();
                entity.Property(e => e.email).IsRequired();
                entity.Property(e => e.telephone).IsRequired();
                entity.Property(e => e.is_active).IsRequired();
                entity.Property(e => e.type).IsRequired();
            });

            // LocationRoom table
            modelBuilder.Entity<LocationRoom>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.designation).IsRequired();
                entity.Property(e => e.location_cupboard);
            });

            // LocationCupboard table
            modelBuilder.Entity<LocationCupboard>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.designation).IsRequired();
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.room_id).IsRequired();
            });

            // Log table
            modelBuilder.Entity<Log>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.log_type).IsRequired();
                entity.Property(e => e.log_message).IsRequired();
            });
        }
    }
}
