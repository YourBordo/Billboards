using System.Collections.Generic;


namespace Billboards.Models
{
    public class Device : IModel
    {
        public long Id { get; set; }
        public bool IsDeleted { get; set; }
        public int Memory { get; set; }
        public int Frequency { get; set; }
        public DeviceGroup DeviceGroup { get; set; }
        public User User { get; set; }
        public ICollection<Advertisement> Advertisements { get; set; } = new List<Advertisement>();
        public bool Status { get; set; }
    }
}
