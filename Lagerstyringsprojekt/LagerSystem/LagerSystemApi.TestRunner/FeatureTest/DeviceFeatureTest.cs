//using Moq;
//using LagerSystemApi.Models.DTO;
//using LagerSystemApi.Controllers;
//using Microsoft.AspNetCore.Mvc;

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
//        public async void t_cangetalldevices()
//        {
//            // Arrange
//            AddSingleDeviceDTO[] mockDevices = new List<AddSingleDeviceDTO>
//            {
//                new AddSingleDeviceDTO { device_overview_id = 1, description = "This is a Monitor", location = 1, is_archived = true, qr = "Device-id=1", status = 0 },
//                new AddSingleDeviceDTO { device_overview_id = 2, description = "This is a Desktop", location = 2, is_archived = false, qr = "Device-id=2", status = 0 }
//            }.ToArray();

//            _deviceMock.Setup(service => service.AddNewDevice(mockDevices[0]));
//            _deviceMock.Setup(service => service.GetAllDevices());

//            // Act
//            for (int i = 0; i < mockDevices.Length; i++)
//            {
//                await _deviceMock.Object.AddNewDevice(mockDevices[i]);
//            }

//            IActionResult devices = await _deviceMock.Object.GetAllDevices();

//            // Assert
//            Assert.NotNull(devices);
//            Assert.Equal(mockDevices.Length, devices.Length);
//            Assert.True(devices.Any(d => d.device_overview_id == mockDevices[0].device_overview_id));
//        }


//        [Fact]
//        public async void t_cangetonedevice()
//        {
//            // Arrange
//            DeviceDTO mockDevice = new DeviceDTO { id = 1, device_overview_id = 1, description = "This is a Monitor", location = 1, is_archived = false, qr = "Device-id=1", status = 0 };

//            _deviceMock.Setup(service => service.Add(mockDevice));
//            _deviceMock.Setup(service => service.Get(1));

//            // Act
//            await _deviceMock.Object.Add(mockDevice);
//            DeviceDTO device = await _deviceMock.Object.Get(1);

//            // Assert
//            Assert.NotNull(device);
//            Assert.Equal(mockDevice.id, device.id);
//            Assert.True(device.device_overview_id == mockDevice.device_overview_id);
//        }

//        [Fact]
//        public async void t_cancreateonedevice()
//        {
//            // Arrange
//            DeviceDTO mockDevice = new DeviceDTO
//            {
//                id = 1,

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
//        public async void t_candeactivateonedevice()
//        {
//            // Arrange
//            DeviceDTO mockDevice = new DeviceDTO { id = 1, device_overview_id = 1, description = "This is a Monitor", location = 1, is_archived = false, qr = "Device-id=1", status = 0 };

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
//        public async void t_canupdateonedevice()
//        {
//            // Arrange
//            DeviceDTO mockDevice = new DeviceDTO { id = 1, device_overview_id = 1, description = "This is a Monitor", location = 1, is_archived = false, qr = "Device-id=1", status = 0 };
//            UpdateDeviceDTO updateDevice = new UpdateDeviceDTO { id = 1, device_overview_id = 1, description = "This is a Monitor", location = 6, is_archived = false, qr = "Device-id=1", status = 4 };

//            _deviceMock.Setup(service => service.Add(mockDevice));
//            _deviceMock.Setup(service => service.Update(updateDevice));
//            _deviceMock.Setup(service => service.Get(1));

//            // Act
//            await _deviceMock.Object.Add(mockDevice);
//            await _deviceMock.Object.Update(updateDevice);
//            DeviceDTO device = await _deviceMock.Object.Get(1);

//            // Assert
//            Assert.NotNull(device);
//            Assert.True(device.location == updateDevice.location);
//            Assert.True(device.status == mockDevice.status);
//        }
//    }
//}
