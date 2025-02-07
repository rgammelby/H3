using LagerSystemApi.Controllers;
using LagerSystemApi.Models.DTO;
using Moq;

namespace LagerSystemApi.TestRunner.FeatureTest
{
    public class LogFeatureTest
    {
        Mock<ILogController> _logService;
        public LogFeatureTest()
        {
            _logService = new Mock<ILogController>();
        }

        [Fact]
        public async void t_canreadlogs()
        {
            // Arrange
            LogDTO[] mockLogs = new LogDTO[] {
                new LogDTO { id = 1, log_message = "Hello", log_type = "default" },
                new LogDTO { id = 2, log_message = "World!", log_type = "default" }
            };

            _logService.Setup(service => service.GetAll());

            // Act
            LogDTO[] logs = await _logService.Object.GetAll();

            // Assert
            Assert.NotNull(logs);
            Assert.Equal(mockLogs.Length, logs.Length);
            Assert.True(mockLogs[0] == logs[0] && mockLogs[1] == logs[1]);
        }

        //[Fact]
        //public async void CanYou_Search_Logs()
        //{
        //    // Arrange
        //    LogDTO[] mockLogs = new LogDTO[]
        //    {
        //        new LogDTO { id = 1, log_message = "Test1", log_type = "test" },
        //        new LogDTO { id = 2, log_message = "Test2", log_type = "test" },
        //        new LogDTO { id = 3, log_message = "Test3", log_type = "test" }
        //    };

        //    _logService.Setup(service => service.Search("Test3"));

        //    // Act
        //    LogDTO[] logs = await _logService.Object.Search("Test3");

        //    // Assert
        //    Assert.NotNull(logs);
        //    Assert.Contains(logs, log => log.log_message == "Test3");
        //}
    }
}
