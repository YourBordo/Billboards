using System.Collections.Generic;

namespace Billboards.Models
{
    public class User : IModel
    {
        public long Id { get; set; }

        public bool IsDeleted { get; set; }

        public string UserName { get; set; }
        public ICollection<Device> Devices { get; set; } = new List<Device>();

        public ICollection<DeviceGroup> DeviceGroups { get; set; } = new List<DeviceGroup>();

    }
}
