using LagerstyringClassLibrary;
using LagerstyringClassLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace LagerstyringClassLibrary
{
    public static class Seeder
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            // Seed data for StatusType
            modelBuilder.Entity<StatusType>().HasData(
                new StatusType { id = 1, status_type = "Available" },
                new StatusType { id = 2, status_type = "Overdue" },
                new StatusType { id = 3, status_type = "Borrowed" },
                new StatusType { id = 4, status_type = "Unavailable" }
            );

            modelBuilder.Entity<StatusType>()
                .Property(st => st.id)
                .ValueGeneratedOnAdd();

            // Seed data for ActivityType
            modelBuilder.Entity<ActivityType>().HasData(
                new ActivityType { id = 1, activity_type = "Borrow" },
                new ActivityType { id = 2, activity_type = "Return" },
                new ActivityType { id = 3, activity_type = "Extend" },
                new ActivityType { id = 4, activity_type = "Late" }
            );

            modelBuilder.Entity<ActivityType>()
                .Property(st => st.id)
                .ValueGeneratedOnAdd();

            // Seed data for DeviceType
            modelBuilder.Entity<DeviceType>().HasData(
                new DeviceType { id = 1, type_name = "Laptop" },
                new DeviceType { id = 2, type_name = "Desktop" },
                new DeviceType { id = 3, type_name = "Keyboard" },
                new DeviceType { id = 4, type_name = "Monitor" },
                new DeviceType { id = 5, type_name = "Mouse" },
                new DeviceType { id = 6, type_name = "Server" },
                new DeviceType { id = 7, type_name = "Router" },
                new DeviceType { id = 8, type_name = "Switch" },
                new DeviceType { id = 9, type_name = "Headset" },
                new DeviceType { id = 10, type_name = "Microphone Set" },
                new DeviceType { id = 11, type_name = "WebCam" }
            );

            // Seed data for LocationRoom (3 rooms)
            modelBuilder.Entity<LocationRoom>().HasData(
                new LocationRoom { id = 1, designation = "D.15" },
                new LocationRoom { id = 2, designation = "D.16" },
                new LocationRoom { id = 3, designation = "D.17" }
            );

            // Seed data for LocationCupboard (6 cupboards per room)
            modelBuilder.Entity<LocationCupboard>().HasData(
                // Cupboards for Room D.15 (id=1)
                new LocationCupboard { id = 1, designation = "Cupboard 1", room_id = 1 },
                new LocationCupboard { id = 2, designation = "Cupboard 2", room_id = 1 },
                new LocationCupboard { id = 3, designation = "Cupboard 3", room_id = 1 },
                new LocationCupboard { id = 4, designation = "Cupboard 4", room_id = 1 },
                new LocationCupboard { id = 5, designation = "Cupboard 5", room_id = 1 },
                new LocationCupboard { id = 6, designation = "Cupboard 6", room_id = 1 },

                // Cupboards for Room D.16 (id=2)
                new LocationCupboard { id = 7, designation = "Cupboard 7", room_id = 2 },
                new LocationCupboard { id = 8, designation = "Cupboard 8", room_id = 2 },
                new LocationCupboard { id = 9, designation = "Cupboard 9", room_id = 2 },
                new LocationCupboard { id = 10, designation = "Cupboard 10", room_id = 2 },
                new LocationCupboard { id = 11, designation = "Cupboard 11", room_id = 2 },
                new LocationCupboard { id = 12, designation = "Cupboard 12", room_id = 2 },

                // Cupboards for Room D.17 (id=3)
                new LocationCupboard { id = 13, designation = "Cupboard 13", room_id = 3 },
                new LocationCupboard { id = 14, designation = "Cupboard 14", room_id = 3 },
                new LocationCupboard { id = 15, designation = "Cupboard 15", room_id = 3 },
                new LocationCupboard { id = 16, designation = "Cupboard 16", room_id = 3 },
                new LocationCupboard { id = 17, designation = "Cupboard 17", room_id = 3 },
                new LocationCupboard { id = 18, designation = "Cupboard 18", room_id = 3 }
            );
        }
    }
}
