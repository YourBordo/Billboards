using Billboards.Models;
using System.Collections.Generic;

namespace ModelServices.AdvertisementStatisticsServicing
{
    public interface IAdvertisementStatisticsService
    {
        IEnumerable<AdvertisementStatistics> GetAdvertisingStatistics(long advId);
        void AddAdvertisingStatistics(long advId, string message);
    }
}
