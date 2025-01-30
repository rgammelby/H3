using LagerSystemApi.Services;
using LagerSystemApi.Controllers;
using LagerSystemApi.Models.DTO;
using Moq;

namespace LagerSystemApi.TestRunner.FeatureTest
{
    public class ActivityFeatureTest
    {
        Mock<IActivityController> _activity;
        public ActivityFeatureTest()
        {
            _activity = new Mock<IActivityController>();
        }

        [Fact]
        public async void CanGet_All_ActivityHistory()
        {
            // Arrange
            ActivityDTO[] mockActivity = new List<ActivityDTO>
            {
                new ActivityDTO { id = 1, device_id = 1, activity_type = 1, notes = "No-One", booking_id = 1, lifecycle_id = 1 ,created_at = new DateTime(2025, 3, 25), start_date = new DateTime(2025, 3, 25), end_date = new DateTime(2025, 5, 1) },
                new ActivityDTO { id = 2, device_id = 2, activity_type = 1, notes = "No-Two", booking_id = 1, lifecycle_id = 2 ,created_at = new DateTime(2025, 5, 15), start_date = new DateTime(2025, 5, 15), end_date = new DateTime(2025, 8, 29) }
            }.ToArray();

            _activity.Setup(service => service.Add(mockActivity[0]));
            _activity.Setup(service => service.GetAll());

            // Act
            for (int i = 0; i < mockActivity.Length; i++)
            {
                _activity.Object.Add(mockActivity[i]);
            }
            ActivityDTO[] activity = await _activity.Object.GetAll();
            
            // Assert
            Assert.NotNull(activity);
            Assert.Equal(mockActivity.Length, activity.Length);
            Assert.True(activity[0].id == 1 && activity[1].id == 2);
        }

        [Fact]
        public async void CanYou_GetActivity_ByDevice()
        {
            // Arrange
            ActivityDTO[] mockActivity = new List<ActivityDTO> {
                new ActivityDTO { id = 1, device_id = 1, activity_type = 1, notes = "No-Two", booking_id = 1, lifecycle_id = 1, created_at = new DateTime(2025, 5, 15), start_date = new DateTime(2025, 5, 15), end_date = new DateTime(2025, 8, 29) },
                new ActivityDTO { id = 2, device_id = 2, activity_type = 1, notes = "No-Two", booking_id = 1, lifecycle_id = 2, created_at = new DateTime(2025, 5, 15), start_date = new DateTime(2025, 5, 15), end_date = new DateTime(2025, 8, 29) }
            }.ToArray();

            _activity.Setup(service => service.Add(mockActivity[0]));
            _activity.Setup(service => service.GetByDeviceId(1));

            // Act
            for(int i = 0; i < mockActivity.Length; i++)
            {
                _activity.Object.Add(mockActivity[i]);
            }

            ActivityDTO[] activities = await _activity.Object.GetByDeviceId(1);

            // Assert
            Assert.NotNull(activities);
            Assert.Equal(mockActivity.Length, activities.Length);
            Assert.True(activities[0].id == 1 && activities[1].id == 2);
        }

        [Fact]
        public async void CanGet_One_ActivityHistory()
        {
            // Arrange
            ActivityDTO mockActivity = new ActivityDTO { id = 2, device_id = 2, activity_type = 1, notes = "No-Two", booking_id = 1, lifecycle_id = 2, created_at = new DateTime(2025, 5, 15), start_date = new DateTime(2025, 5, 15), end_date = new DateTime(2025, 8, 29) };

            _activity.Setup(service => service.Add(mockActivity));
            _activity.Setup(service => service.Get(1));

            // Act
            _activity.Object.Add(mockActivity);
            ActivityDTO activity = await _activity.Object.Get(1);

            // Assert
            Assert.NotNull(activity);
            Assert.True(mockActivity.id == activity.id);
        }

        [Fact]
        public async void CheckIf_AbleTo_Create_ActivityHistory()
        {
            // Arrange
            ActivityDTO mockActivity = new ActivityDTO { id = 1, device_id = 1, activity_type = 1, notes = "No-Two", booking_id = 1, lifecycle_id = 1, created_at = new DateTime(2025, 5, 15), start_date = new DateTime(2025, 5, 15), end_date = new DateTime(2025, 8, 29) };

            _activity.Setup(service => service.Add(mockActivity));
            _activity.Setup(service => service.Get(1));

            // Act
            _activity.Object.Add(mockActivity);
            ActivityDTO activiity = await _activity.Object.Get(1);

            // Assert
            Assert.NotNull(activiity);
            Assert.True(mockActivity.id == activiity.id);
        }

        [Fact]
        public async void Can_Update_ActivityHistory()
        {
            // Arrange
            ActivityDTO mockActivity = new ActivityDTO { id = 1, device_id = 1, activity_type = 1, notes = "No-Two", booking_id = 1, lifecycle_id = 1, created_at = new DateTime(2025, 5, 15), start_date = new DateTime(2025, 5, 15), end_date = new DateTime(2025, 8, 29) };
            UpdateActivityDTO updateActivity = new UpdateActivityDTO { id = 1, device_id = 1, activity_type = 1, notes = "is updated", booking_id = 1, lifecycle_id = 1, created_at = new DateTime(2025, 5, 15), start_date = new DateTime(2025, 5, 15), end_date = new DateTime(2025, 10, 1) };

            _activity.Setup(service => service.Add(mockActivity));
            _activity.Setup(service => service.Update(updateActivity));
            _activity.Setup(service => service.Get(1));

            // Act
            _activity.Object.Add(mockActivity);
            _activity.Object.Update(updateActivity);
            ActivityDTO activity = await _activity.Object.Get(1);

            // Assert
            Assert.NotNull(activity);
            Assert.Equal(activity.notes, updateActivity.notes);
            Assert.True(activity.notes == updateActivity.notes && activity.end_date == updateActivity.end_date && activity.activity_type == updateActivity.activity_type);
        }

        /*
         * All of these below is basically to see if you can create a activity with a specific type, therefor the same as a test ono AddActivity
        [Fact]
        public void CheckIf_AbleTo_CreateBorrowActivity()
        {

        }

        [Fact]
        public void CheckIf_AbleTo_ExtendActivity()
        {

        }

        [Fact]
        public void CheckIf_AbleTo_ReturnActivity()
        {

        }
        */
    }
}
