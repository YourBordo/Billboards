using Billboards.Models;
using DataBaseAccess.Repsitory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ModelServices.AdvertisementStatisticsServicing
{
    public class AdvertisementStatisticsService : IAdvertisementStatisticsService
    {
        private readonly IRepository<AdvertisementStatistics> _advertisementStatisticsRepository;

        private readonly IRepository<Advertisement> _advertisementRepository;

        public AdvertisementStatisticsService(IRepository<AdvertisementStatistics> advertisementStatisticsRepository, IRepository<Advertisement> advertisementRepository)
        {
            _advertisementStatisticsRepository = advertisementStatisticsRepository;
            _advertisementRepository = advertisementRepository;
        }
        public IEnumerable<AdvertisementStatistics> GetAdvertisingStatistics(long advId)
        {
            return _advertisementStatisticsRepository.GetAll()?.Where(ast => ast.Advertisement.Id == advId)?.ToList();
        }

        public void AddAdvertisingStatistics(long advId, string message)
        {
            var advertisement = _advertisementRepository.Get(advId);
            AdvertisementStatistics advertisementStatistics = new AdvertisementStatistics()
            {
                ActionMessage = message,
                LogTime = DateTime.Now,
                Advertisement = advertisement
            };
            
            _advertisementStatisticsRepository.Create(advertisementStatistics);
            advertisement.AdvertisementStatistics.Add(advertisementStatistics);
            _advertisementRepository.Update(advertisement);
        }
    }
}
