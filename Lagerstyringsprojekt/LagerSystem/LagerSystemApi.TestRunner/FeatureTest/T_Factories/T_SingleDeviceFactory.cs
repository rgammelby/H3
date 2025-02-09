using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LagerSystemApi.Models.DTO;

namespace LagerSystemApi.TestRunner.FeatureTest.T_Factories
{
    public static class T_SingleDeviceFactory
    {
        public static List<DeviceDTO> CreateSingleDevices()
        {
            return new List<DeviceDTO>
        {
            new DeviceDTO { id = 101, device_overview_id = 1, is_archived = false, description = "iPhone 12 - Blue", status = 1, location = 10, qr = "QR001" },
            new DeviceDTO { id = 102, device_overview_id = 1, is_archived = false, description = "iPhone 12 - Black", status = 1, location = 12, qr = "QR002" },
            new DeviceDTO { id = 103, device_overview_id = 2, is_archived = false, description = "Samsung S21 - Silver", status = 1, location = 15, qr = "QR003" }
        };
        }
    }
}
