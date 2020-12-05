using System;
using System.Collections.Generic;
using System.Text;
using Billboards.Models;

namespace Server
{
    public interface IServerEmulation
    {
        Advertisement GetNextAdv(long deviceId);
        void AddAdvertisement(long deviceId, Advertisement advertisement);
        void DeleteAdvertisement(long deviceId, long advId);
        void ChangeFrequencyForDevice(long deviceId, long deviceGroupId, int frequency);
    }
}
