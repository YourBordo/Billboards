using Billboards.Models;
using System.Collections.Generic;
using System.IO;

namespace ModelServices.DeviceServicing
{
    public interface IDeviceService
    {
        void AddDevice(long id, int memory = 32);
        void DeleteDevice(long id);
        IEnumerable<Device> GetDevices(long? userId = null);
        IEnumerable<Device> GetDevicesInGroup(long diveGroupId);
        void AddFrequency(long deviceId, int frequence);
        void ImportFrequency(FileStream fileStream);
        void ExportFrequency(Device device);
        void Save(Device device);

    }
}
