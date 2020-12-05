using System.Collections.Generic;

namespace Billboards.Models
{
    public class Advertisement : IModel
    {
        public long Id { get; set; }
        public bool IsDeleted { get; set; }
        public long MemoryLength { get; set; }
        public long CurrentTime { get; set; }
        public long MaxTime { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public Device Device { get; set; }
        public bool IsActive { get; set; }
        public ICollection<AdvertisementStatistics> AdvertisementStatistics { get; set; } = new List<AdvertisementStatistics>();
    }
}
