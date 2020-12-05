using System;

namespace Billboards.Models
{
    public class AdvertisementStatistics : IModel
    {
        public long Id { get; set; }
        public bool IsDeleted { get; set; }
        public Advertisement Advertisement { get; set; }
        public DateTime LogTime { get; set; }
        public string ActionMessage { get; set; }
        
    }
}
