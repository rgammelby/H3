using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LagerSystemApi.Models.DTO;

namespace LagerSystemApi.TestRunner.FeatureTest.T_Factories
{
    public static class T_DeviceOverviewFactory
    {
        public static List<DeviceOverviewDTO> CreateDeviceOverviews()
        {
            return new List<DeviceOverviewDTO>
            {
                new DeviceOverviewDTO { id = 1, model = "iPhone 12", device_type = 1, image = "iphone12.png", qty = 10, available_qty = 5, last_ordered = DateTime.Now },
                new DeviceOverviewDTO { id = 2, model = "Samsung Galaxy S21", device_type = 2, image = "s21.png", qty = 8, available_qty = 3, last_ordered = DateTime.Now },
                new DeviceOverviewDTO { id = 3, model = "Google Pixel 6", device_type = 3, image = "pixel6.png", qty = 15, available_qty = 10, last_ordered = DateTime.Now }
            };
        }
    }
}
