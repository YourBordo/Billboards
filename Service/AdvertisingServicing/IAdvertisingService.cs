using Billboards.Models;
using System.Collections.Generic;

namespace ModelServices.AdvertisingServicing
{
    public interface IAdvertisingService 
   {

       IEnumerable<Advertisement> GetAdvertisements(long deviceId);
       
       void DeleteAdvertising(long advId);

       string AddAdvertising(Advertisement advertisement, long deviceId, long memoryLength);

       void Save(Advertisement advert);
   }
}
