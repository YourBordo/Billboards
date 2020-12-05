using Billboards.Models;
using System.Collections.Generic;
using System.IO;

namespace ModelServices.DeviceGroupServicing
{
    public interface IDeviceGroupService
    {
        IEnumerable<DeviceGroup> GetDeviceGroups(long? userId = null);
        void AddDeviceGroup(long id);
        void DeleteDeviceGroup(long id);
        void AddFrequency(long deviceGroupId, int frequence);
        void ImportFrequency(FileStream fileStream);
        
        void AddDeviceInGroup(long deviceGroupId, long deviceId);

        void DeleteDeviceInGroup(long deivceGroupId, long deviceId);
    }
}
