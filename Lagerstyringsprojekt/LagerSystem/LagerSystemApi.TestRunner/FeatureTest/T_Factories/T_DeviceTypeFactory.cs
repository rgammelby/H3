using LagerSystemApi.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagerSystemApi.TestRunner.FeatureTest.T_Factories
{
    public static class T_DeviceTypeFactory
    {
        public static List<DeviceTypeDTO> CreateDeviceTypes()
        {
            return new List<DeviceTypeDTO>
            {
                new DeviceTypeDTO { device_id = 1, device_type = "Keyboard" },
                new DeviceTypeDTO { device_id = 2, device_type = "Laptop" },
                new DeviceTypeDTO { device_id = 3, device_type = "Smartphone"}
            };
        }
    }
}
