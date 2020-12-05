using Billboards.Models;
using System.Collections.Generic;

namespace Billboards.ModelsView
{
    public class EnableDevicesAndDevicesInGroupAndCurrentDeviceGroup
    {
        public IEnumerable<Device> EnableDevices { get; set; }

        public IEnumerable<Device> DevicesInGroup { get; set; }

        public DeviceGroup DeviceGroup { get; set; }

        public EnableDevicesAndDevicesInGroupAndCurrentDeviceGroup(IEnumerable<Device> enableDevices, IEnumerable<Device> devicesInGroup, DeviceGroup deviceGroup)
        {
            EnableDevices = enableDevices;
            DevicesInGroup = devicesInGroup;
            DeviceGroup = deviceGroup;
        }
    }
}
