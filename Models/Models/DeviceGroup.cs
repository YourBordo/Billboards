using System.Collections.Generic;

namespace Billboards.Models
{
    public class DeviceGroup : IModel
    {
        public long Id { get; set; }

        public bool IsDeleted { get; set; }

        public User User { get; set; }

        public ICollection<Device> Devices { get; set; } = new List<Device>();

    }
}
