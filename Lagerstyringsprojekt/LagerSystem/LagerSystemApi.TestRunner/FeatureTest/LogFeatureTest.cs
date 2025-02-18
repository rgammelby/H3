using LagerSystemApi.Controllers;
using LagerSystemApi.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LagerSystemApi.TestRunner.FeatureTest
{
    public class LogFeatureTest
    {
        Mock<LogController> _logService;
        public LogFeatureTest()
        {
            _logService = new Mock<LogController>();
        }
        //[Fact]
        //public async void T_CanCreateLog()
        //{
        //    // Arrange
        //    LogDTO mockLog = new LogDTO { id = 1, log_message = "deviceId: 1, has been inserted by userId: 3", log_type = "Insert"};

        //    _logService.Setup(service => service.Get(1));

        //    // Act
        //    LogDTO log = await _logService.Object.Get(1);

        //    // Assert
        //    Assert.NotNull(log);
        //    Assert.True(log.id == mockLog.id);
        //}

        [Fact]
        public async void T_CanReadAllLogs()
        {
            // Arrange
            LogDTO[] mockLogs = new LogDTO[] {
                new LogDTO { id = 1, log_message = "Hello", log_type = "default" },
                new LogDTO { id = 2, log_message = "World!", log_type = "default" }
            };

            _logService.Setup(service => service.GetAllLogs());

            // Act
            var logs = await _logService.Object.GetAllLogs();

            // Assert
            Assert.NotNull(logs);
            Assert.Equal(mockLogs.Length, logs.Length);
            Assert.True(mockLogs[0] == logs[0] && mockLogs[1] == logs[1]);
        }

    //    [Fact]
    //    public async void T_CanReadOneLog()
    //    {
    //        // Arrange
    //        LogDTO mockLog = new LogDTO { id = 1, log_message = "Test", log_type = "test" };

    //        _logService.Setup(service => service.Get(1));

    //        // Act
    //        LogDTO log = await _logService.Object.Get(1);

    //        // Assert
    //        Assert.NotNull(log);
    //        Assert.True(mockLog == log);
    //    }

    //    [Fact]
    //    public async void T_CanSearchLogs()
    //    {
    //        // Arrange
    //        LogDTO[] mockLogs = new LogDTO[]
    //        {
    //            new LogDTO { id = 1, log_message = "Test1", log_type = "test" },
    //            new LogDTO { id = 2, log_message = "Test2", log_type = "test" },
    //            new LogDTO { id = 3, log_message = "Test3", log_type = "test" }
    //        };

    //        _logService.Setup(service => service.Search("Test3"));

    //        // Act
    //        LogDTO[] logs = await _logService.Object.Search("Test3");

    //        // Assert
    //        Assert.NotNull(logs);
    //        Assert.Contains(logs, log => log.log_message == "Test3");
    //    }
    }
}
