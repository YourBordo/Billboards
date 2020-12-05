using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Billboards.Models;
using ModelServices.AdvertisementStatisticsServicing;
using ModelServices.AdvertisingServicing;
using ModelServices.DeviceGroupServicing;
using ModelServices.DeviceServicing;

namespace Server
{
    public class ServerEmulation : IServerEmulation
    {
        private readonly IAdvertisingService _advertisingService;

        private readonly IDeviceService _deviceService;

        private readonly IAdvertisementStatisticsService _advertisementStatisticsService;
        private readonly IDeviceGroupService _deviceGroupService;

        private static bool _devicesInit = false;

        private static Dictionary<long, Task> _tasksDelay = new Dictionary<long, Task>();

        private static Dictionary<long, Queue<Advertisement>> _advertisements = new Dictionary<long, Queue<Advertisement>>();
        public ServerEmulation(IAdvertisingService advertisingService, IDeviceService deviceService, IAdvertisementStatisticsService advertisementStatisticsService, IDeviceGroupService deviceGroupService)
        {
            _advertisementStatisticsService = advertisementStatisticsService;
            _advertisingService = advertisingService;
            _deviceService = deviceService;
            _deviceGroupService = deviceGroupService;
            if (_devicesInit == false)
            {
                InitDevices();
                _devicesInit = true;
            }

        }

        private void InitDevices()
        {
            IEnumerable<Device> devices = _deviceService.GetDevices();
            foreach (var device in devices)
            {
                var deviceId = device.Id;
                Queue<Advertisement> queueAdvert = new Queue<Advertisement>();
                for (int i = 0; i < device.Frequency; i++)
                {
                    foreach (var advertisement in device.Advertisements)
                    {
                        queueAdvert.Enqueue(advertisement);
                    }
                }

                
                _advertisements.Add(deviceId, queueAdvert);
            }
        }

        public Advertisement GetNextAdv(long deviceId)
        {
            Advertisement adv = new Advertisement()
            {
                Device = _deviceService.GetDevices().SingleOrDefault(d => d.Id == deviceId),
                FileName = "PosterVideo.mp4"
            };

            if (!_tasksDelay.ContainsKey(deviceId))
            {
                Task task = new Task(() =>
                {
                    Thread.Sleep(1000);
                });
                _tasksDelay.Add(deviceId, task);
                _tasksDelay[deviceId].Start();
                return adv;

            }
            else if (!_tasksDelay[deviceId].IsCompleted)
            {
                return adv;
            }
            else if (!_advertisements.ContainsKey(deviceId))
            {
                return adv;
            }
            else { 
            _tasksDelay.Remove(deviceId);
                Queue<Advertisement> certainAdvQueue = _advertisements[deviceId];
                if (certainAdvQueue.Count() != 0)
                {
                    var certainAdv = certainAdvQueue?.Dequeue();
                    _advertisementStatisticsService.AddAdvertisingStatistics(certainAdv.Id, $"the video is loading on website");
                    return certainAdv;
                }
                else
                {
                    return adv;
                }

            }
        }

        public void AddAdvertisement(long deviceId, Advertisement advertisement)
        {
            if (!_advertisements.ContainsKey(deviceId))
            {
                Queue<Advertisement> queue = new Queue<Advertisement>();
                queue.Enqueue(advertisement);
                _advertisements.Add(deviceId, queue);
            }
            else
            {
                _advertisements[deviceId].Enqueue(advertisement);
            }
            _advertisementStatisticsService.AddAdvertisingStatistics(advertisement.Id, $"the video was loaded into server");
        }

        public void DeleteAdvertisement(long deviceId, long advId)
        {
            if (_advertisements.ContainsKey(deviceId))
            {
                var queue = _advertisements[deviceId];
                var adv = _advertisingService.GetAdvertisements(deviceId).SingleOrDefault(a => a.Id == advId);
                if (queue.Contains(adv))
                {
                    Queue<Advertisement> newQueue = new Queue<Advertisement>();
                    foreach (var advertisement in queue)
                    {
                        if (advertisement.Id != adv.Id)
                        {
                            newQueue.Enqueue(advertisement);
                        }
                    }
                    _advertisements[deviceId] = newQueue;
                }
                _advertisementStatisticsService.AddAdvertisingStatistics(advId, $"the video  was deleted");
            }
        }

        public void ChangeFrequencyForDevice(long deviceId, long deviceGroupId, int frequency)
        {
            if (deviceId != 0)
            {
                Device device = _deviceService.GetDevices().SingleOrDefault(d => d.Id == deviceId);

                Queue<Advertisement> queueAdvert = new Queue<Advertisement>();
                for (int i = 0; i < frequency; i++)
                {
                    foreach (var advertisement in device.Advertisements)
                    {
                        queueAdvert.Enqueue(advertisement);
                        _advertisementStatisticsService.AddAdvertisingStatistics(advertisement.Id,
                            $"the video was added in queue for device {deviceId}");
                    }
                }

                _advertisements.Remove(deviceId);
                _advertisements.Add(deviceId, queueAdvert);
            }
            else
            {
                DeviceGroup dg = _deviceGroupService.GetDeviceGroups().SingleOrDefault(d => d.Id == deviceGroupId);
                foreach (var device in dg.Devices)
                {

                    Queue<Advertisement> queueAdvert = new Queue<Advertisement>();
                    for (int i = 0; i < frequency; i++)
                    {
                        foreach (var advertisement in device.Advertisements)
                        {
                            queueAdvert.Enqueue(advertisement);
                            _advertisementStatisticsService.AddAdvertisingStatistics(advertisement.Id,
                                $"the video was added in queue for device {deviceId}");
                        }
                    }

                    _advertisements.Remove(deviceId);
                    _advertisements.Add(deviceId, queueAdvert);
                }
            }
        }

        public void ChangeFrequencyForDeviceGroup(long deviceGroupId, int frequency)
        {
            IEnumerable<Device> devices = _deviceService.GetDevicesInGroup(deviceGroupId);
            foreach (var device in devices)
            {
                var deviceId = device.Id;
                Queue<Advertisement> queueAdvert = new Queue<Advertisement>();
                for (int i = 0; i < frequency; i++)
                {
                    foreach (var advertisement in device.Advertisements)
                    {
                        queueAdvert.Enqueue(advertisement);
                    }
                }
                _advertisements.Add(deviceId, queueAdvert);
            }
        }

    }
}
