using System;
using System.Collections.Generic;
using LagerSystemApi.Models.DTO;

namespace LagerSystemApi.TestRunner.FeatureTest.T_Factories
{
    public static class T_ActivityFactory
    {
        public static List<ActivityDTO> CreateActivities()
        {
            return new List<ActivityDTO>
            {
                new ActivityDTO { id = 1, device_id = 1, activity_type = 1, notes = "Activity One", lifecycle_id = Guid.NewGuid(), created_at = new DateTime(2025, 3, 25), start_date = new DateTime(2025, 3, 25), end_date = new DateTime(2025, 5, 1) },
                new ActivityDTO { id = 2, device_id = 2, activity_type = 2, notes = "Activity Two", lifecycle_id = Guid.NewGuid(), created_at = new DateTime(2025, 5, 15), start_date = new DateTime(2025, 5, 15), end_date = new DateTime(2025, 8, 29) }
            };
        }
    }
}
