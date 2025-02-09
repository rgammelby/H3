using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LagerSystemApi.Models.DTO;

namespace LagerSystemApi.TestRunner.FeatureTest.T_Factories
{
    public class FakeDeviceRepository
    {
        private readonly List<DeviceOverviewDTO> _deviceOverviews;
        private readonly List<DeviceDTO> _singleDevices;
        private readonly List<DeviceTypeDTO> _deviceTypes;

        public IReadOnlyList<DeviceOverviewDTO> DeviceOverviews => _deviceOverviews;
        public IReadOnlyList<DeviceDTO> SingleDevices => _singleDevices;
        public IReadOnlyList<DeviceTypeDTO> DeviceTypes => _deviceTypes;


        public FakeDeviceRepository()
        {
            _deviceOverviews = T_DeviceOverviewFactory.CreateDeviceOverviews();
            _singleDevices = T_SingleDeviceFactory.CreateSingleDevices();
            _deviceTypes = T_DeviceTypeFactory.CreateDeviceTypes();
        }

        public async Task<List<DeviceDTO>> GetSingleDevicesByModel(string model)
        {
            var deviceOverviewIds = _deviceOverviews
                .Where(d => d.model.Contains(model, StringComparison.OrdinalIgnoreCase))
                .Select(d => d.id)
                .ToList();

            var result = _singleDevices
                .Where(s => deviceOverviewIds.Contains(s.device_overview_id))
                .ToList();

            return await Task.FromResult(result);
        }

        public async Task<List<DeviceDTO>> GetSingleDevicesByType(string type)
        {
            var deviceOverviewIds = _deviceOverviews
                .Where(d => _deviceTypes.Any(t => t.device_id == d.device_type &&
                                                   t.device_type.Equals(type, StringComparison.OrdinalIgnoreCase)))
                .Select(d => d.id)
                .ToList();

            var result = _singleDevices
                .Where(s => deviceOverviewIds.Contains(s.device_overview_id))
                .ToList();

            return await Task.FromResult(result);
        }

    }
}
