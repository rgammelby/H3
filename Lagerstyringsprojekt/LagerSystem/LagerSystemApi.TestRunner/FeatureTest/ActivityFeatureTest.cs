using LagerSystemApi.Controllers;
using LagerSystemApi.Models.DTO;
using Moq;
using Microsoft.AspNetCore.Mvc;
using LagerSystemApi.TestRunner.FeatureTest.T_Factories;

namespace LagerSystemApi.TestRunner.FeatureTest
{
    public class ActivityFeatureTest
    {
        private readonly Mock<IActivityController> _activity;
        private readonly List<ActivityDTO> _mockActivities;

        public ActivityFeatureTest()
        {
            _activity = new Mock<IActivityController>();
            _mockActivities = T_ActivityFactory.CreateActivities();
        }

        [Fact]
        public async Task T_CanGetAllActivities()
        {
            _activity.Setup(service => service.GetAll()).ReturnsAsync(new OkObjectResult(_mockActivities));

            var result = await _activity.Object.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<ActivityDTO>>(okResult.Value);

            Assert.NotNull(returnValue);
            Assert.Equal(_mockActivities.Count, returnValue.Count);
        }

        [Fact]
        public async Task T_CanGetAllActivitiesByDeviceId()
        {
            int deviceId = 1;
            var expectedActivities = _mockActivities.Where(a => a.device_id == deviceId).ToList();

            _activity.Setup(service => service.GetByDeviceId(deviceId)).ReturnsAsync(new OkObjectResult(expectedActivities));

            var result = await _activity.Object.GetByDeviceId(deviceId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<ActivityDTO>>(okResult.Value);

            Assert.NotNull(returnValue);
            Assert.Equal(expectedActivities.Count, returnValue.Count);
        }

        [Fact]
        public async Task T_CanGetOneActivity()
        {
            int activityId = 1;
            var expectedActivity = _mockActivities.First(a => a.id == activityId);

            _activity.Setup(service => service.Get(activityId)).ReturnsAsync(new OkObjectResult(expectedActivity));

            var result = await _activity.Object.Get(activityId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ActivityDTO>(okResult.Value);

            Assert.NotNull(returnValue);
            Assert.Equal(expectedActivity.notes, returnValue.notes);
        }

        [Fact]
        public async Task T_CanCreateActivity()
        {
            var newActivity = new ActivityDTO { id = 3, device_id = 3, activity_type = 2, notes = "New Activity", lifecycle_id = Guid.NewGuid(), created_at = DateTime.Now, start_date = DateTime.Now, end_date = DateTime.Now.AddMonths(1) };

            _activity.Setup(service => service.Add(newActivity));
            _activity.Setup(service => service.Get(newActivity.id)).ReturnsAsync(new OkObjectResult(newActivity));

            await _activity.Object.Add(newActivity);
            var result = await _activity.Object.Get(newActivity.id);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ActivityDTO>(okResult.Value);

            Assert.NotNull(returnValue);
            Assert.Equal(newActivity.notes, returnValue.notes);
        }

        [Fact]
        public async Task T_CanUpdateActivity()
        {
            int activityId = 1;
            var existingActivity = _mockActivities.First(a => a.id == activityId);
            var updatedActivity = new UpdateActivityDTO { id = existingActivity.id, device_id = existingActivity.device_id, activity_type = 2, notes = "Updated Activity", lifecycle_id = existingActivity.lifecycle_id, created_at = existingActivity.created_at, start_date = existingActivity.start_date, end_date = existingActivity.end_date.AddMonths(1) };

            _activity.Setup(service => service.Update(activityId, updatedActivity));
            _activity.Setup(service => service.Get(activityId)).ReturnsAsync(new OkObjectResult(updatedActivity));

            await _activity.Object.Update(activityId, updatedActivity);
            var result = await _activity.Object.Get(activityId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<UpdateActivityDTO>(okResult.Value);

            Assert.NotNull(returnValue);
            Assert.Equal(updatedActivity.notes, returnValue.notes);
        }
    }
}
