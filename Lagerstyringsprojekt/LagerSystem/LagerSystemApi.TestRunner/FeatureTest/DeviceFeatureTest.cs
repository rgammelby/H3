//using Moq;
//using LagerSystemApi.Services;
//using LagerSystemApi.Models.DTO;
//using LagerSystemApi.Controllers;

//namespace LagerSystemApi.TestRunner.FeatureTest
//{
//    public class DeviceFeatureTest
//    {
//        Mock<IDeviceController> _deviceMock;
//        public DeviceFeatureTest()
//        {
//            _deviceMock = new Mock<IDeviceController>();
//        }

//        [Fact]
//        public async void CheckIf_AbleTo_GetAllDevices()
//        {
//            // Arrange
//            DeviceDTO[] mockDevices = new List<DeviceDTO>
//            {
//                new DeviceDTO { id = 1, device_overview_id = 1, description = "This is a Monitor", location_id = 1, is_archived = true, qr = "Device-id=1", status = 0 },
//                new DeviceDTO { id = 2, device_overview_id = 2, description = "This is a Desktop", location_id = 2, is_archived = false, qr = "Device-id=2", status = 0  }
//            }.ToArray();

//            _deviceMock.Setup(service => service.Add(mockDevices[0]));
//            _deviceMock.Setup(service => service.GetAll());

//            // Act
//            for(int i = 0; i < mockDevices.Length; i++)
//            {
//                await _deviceMock.Object.Add(mockDevices[i]);
//            }

//            DeviceDTO[] devices = await _deviceMock.Object.GetAll();

//            // Assert
//            Assert.NotNull(devices);
//            Assert.Equal(mockDevices.Length, devices.Length);
//            Assert.True(devices.Any(d => d.device_overview_id == mockDevices[0].device_overview_id));
//        }

        
//        //[Fact]
//        //public async void CheckIf_AbleTo_GetOnlyOneDevice()
//        //{
//        //    // Arrange
//        //    DeviceDTO mockDevice = new DeviceDTO { id = 1, device_overview_id = "Device1", description = "This is a Monitor", location_id = 1, is_archived = 1, qr = "Device-id=1", status = 0 };

//        //    _deviceMock.Setup(service => service.Add(mockDevice));
//        //    _deviceMock.Setup(service => service.Get(1));

//        //    // Act
//        //    await _deviceMock.Object.Add(mockDevice);
//        //    DeviceDTO device = await _deviceMock.Object.Get(1);

//        //    // Assert
//        //    Assert.NotNull(device);
//        //    Assert.Equal(mockDevice.id, device.id);
//        //    Assert.True(device.device_overview_id == mockDevice.device_overview_id);
//        //}

//        [Fact]
//        public async void IsAbleTo_CreateSingleDevice()
//        {
//            // Arrange
//            DeviceDTO mockDevice = new DeviceDTO { id = 1, device_overview_id = "De, description = "This is a Monitor", location_id = 1, is_archived = 1, qr = "Device-id=1", status = 0 };

//            _deviceMock.Setup(service => service.Add(mockDevice));
//            _deviceMock.Setup(service => service.Get(1));

//            // Act
//            await _deviceMock.Object.Add(mockDevice);

//            DeviceDTO device = await _deviceMock.Object.Get(mockDevice.id);

//            // Assert
//            Assert.NotNull(device);
//            Assert.Equal(mockDevice, device);
//            Assert.True(mockDevice == device);

//        }

//        [Fact]
//        public async void IsAbleTo_DeactivateADevice()
//        {
//            // Arrange
//            DeviceDTO mockDevice = new DeviceDTO { id = 1, device_overview_id = "Device1", description = "This is a Monitor", location_id = 1, is_archived = 1, qr = "Device-id=1", status = 0 };

//            _deviceMock.Setup(service => service.Add(mockDevice));
//            _deviceMock.Setup(service => service.Deactivate(1));
//            _deviceMock.Setup(service => service.Get(1));

//            // Act
//            await _deviceMock.Object.Add(mockDevice);
//            await _deviceMock.Object.Deactivate(1);

//            DeviceDTO device = await _deviceMock.Object.Get(1);

//            // Assert
//            Assert.NotNull(device);
//            Assert.True(device.status == 5);
//        }

//        [Fact]
//        public async void IsAbleTo_UpdateADevice()
//        {
//            // Arrange
//            DeviceDTO mockDevice = new DeviceDTO { id = 1, device_overview_id = "Device1", description = "This is a Monitor", location_id = 1, is_archived = 1, qr = "Device-id=1", status = 0 };
//            UpdateDeviceDTO updateDevice = new UpdateDeviceDTO { id = 1, deviceOverview_id = "Device1", description = "This is a Monitor", location_id = 6, is_archived = 1, qr = "Device-id=1", status = 4 };

//            _deviceMock.Setup(service => service.Add(mockDevice));
//            _deviceMock.Setup(service => service.Update(updateDevice));
//            _deviceMock.Setup(service => service.Get(1));

//            // Act
//            await _deviceMock.Object.Add(mockDevice);
//            await _deviceMock.Object.Update(updateDevice);
//            DeviceDTO device = await _deviceMock.Object.Get(1);

//            // Assert
//            Assert.NotNull(device);
//            Assert.True(device.location_id == updateDevice.location_id);
//            Assert.True(device.status == mockDevice.status);
//        }
//    }
//}
