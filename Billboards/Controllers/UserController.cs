using System;
using Billboards.Models;
using Billboards.ModelsView;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModelServices.AdvertisingServicing;
using ModelServices.DeviceGroupServicing;
using ModelServices.DeviceServicing;
using ModelServices.UserServicing;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

using ModelServices.AdvertisementStatisticsServicing;
using Server;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Enums;


namespace Billboards.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;

        private readonly IAdvertisingService _advertisingService;

        private readonly IDeviceGroupService _deviceGroupService;

        private readonly IDeviceService _deviceService;

        private readonly IUserService _userService;
        private readonly IAdvertisementStatisticsService _advertisementStatisticsService;

        private IHostingEnvironment _env;

        private IServerEmulation _server;
        private string _dir;
        private static long _userId;

        private static long _deviceGroupId;

        public UserController(IAdvertisingService advertisingService,
                              IDeviceGroupService deviceGroupService,
                              IDeviceService deviceService,
                              IUserService userService,
                              IHostingEnvironment env,
                              ILogger<UserController> logger,
                              IServerEmulation server,
                              IAdvertisementStatisticsService advertisementStatisticsService)
        {
            _advertisementStatisticsService = advertisementStatisticsService;
            _advertisingService = advertisingService;
            _deviceGroupService = deviceGroupService;
            _deviceService = deviceService;
            _userService = userService;
            _logger = logger;
            _env = env;
            _dir = _env.WebRootPath;
            _server = server;
        }
        public IActionResult Devices(long userId)
        {
            if (userId != 0)
            {
                _userId = userId;
            }

            IEnumerable<Device> devices = _deviceService.GetDevices(_userId);
            User user = _userService.GetUsers().SingleOrDefault(u => u.Id == _userId);
            UserDevicesModelView userDevicesModel = new UserDevicesModelView(user, devices);

            return View(userDevicesModel);
        }

        public IActionResult DeviceGroups()
        {
            IEnumerable<DeviceGroup> deviceGroups = _deviceGroupService.GetDeviceGroups(_userId);
            return View(deviceGroups);
        }

        public IActionResult Advertising(long deviceId)
        {
            DeviceAdvertisingsView advertisementView = new DeviceAdvertisingsView();

            advertisementView.Device = _deviceService.GetDevices().SingleOrDefault(d => d.Id == deviceId);

            IEnumerable<Advertisement> advertisings = _advertisingService.GetAdvertisements(deviceId);
            advertisementView.Advertisements = advertisings;

            return View(advertisementView);
        }
        public IActionResult DeleteAdvertising(long advId, long deviceId)
        {
            _advertisingService.DeleteAdvertising(advId);
            var advrtName = _advertisingService.GetAdvertisements(deviceId).FirstOrDefault(a => a.Id == advId)
                ?.FileName;
            _logger.LogInformation($"{_userId}|User {_userService.GetUsers().SingleOrDefault(u => u.Id == _userId)} deleted video {advrtName} for device {deviceId}");
            return RedirectToAction("Advertising", new { deviceId });
        }

        public IActionResult AdvertisingStatistics(long advId)
        {
            IEnumerable<AdvertisementStatistics> Advertising =
                _advertisementStatisticsService.GetAdvertisingStatistics(advId) ?? new List<AdvertisementStatistics>();
            return View(Advertising);
        }

        public IActionResult DevicesInGroup(long deviceGroupId)
        {
            if (deviceGroupId != 0)
            {
                _deviceGroupId = deviceGroupId;
            }

            IEnumerable<Device> enableDevices = _deviceService.GetDevices(_userId).Where(d => d.DeviceGroup == null);
            DeviceGroup currentDeviceGroup = _deviceGroupService.GetDeviceGroups(_userId)
                .SingleOrDefault(dg => dg.Id == _deviceGroupId);

            IEnumerable<Device> deviceInGroup = _deviceService.GetDevicesInGroup(_deviceGroupId);

            EnableDevicesAndDevicesInGroupAndCurrentDeviceGroup viewObject =
                new EnableDevicesAndDevicesInGroupAndCurrentDeviceGroup(enableDevices, deviceInGroup, currentDeviceGroup);

            return View(viewObject);
        }

        public IActionResult AddDeviceToGroup(long deviceId, long deviceGroupId)
        {
            _deviceGroupService.AddDeviceInGroup(deviceGroupId, deviceId);
            var userName = _userService.GetUsers().SingleOrDefault(u => u.Id == _userId);
            _logger.LogInformation($"{_userId}|User {userName} added device {deviceId} to device group {deviceGroupId}");
            return RedirectToAction("DevicesInGroup", "User", new { _userId, _deviceGroupId });
        }

        public IActionResult DeleteDeviceInGroup(long deviceId, long deviceGroupId)
        {
            _deviceGroupService.DeleteDeviceInGroup(deviceGroupId, deviceId);
            var userName = _userService.GetUsers().SingleOrDefault(u => u.Id == _userId);
            _logger.LogInformation($"{_userId}|User {userName} deleted device {deviceId} from device group {deviceGroupId}");
            return RedirectToAction("DevicesInGroup", "User", new { _userId, _deviceGroupId });
        }

        public IActionResult SubmitFrequency(long deviceId, long deviceGroupId, int frequency)
        {
            var userName = _userService.GetUsers().SingleOrDefault(u => u.Id == _userId);
            if (deviceId != 0)
            {
                _deviceService.AddFrequency(deviceId, frequency);
                _logger.LogInformation($"{_userId}|User {userName} submitted frequency for device {deviceId}");
                _server.ChangeFrequencyForDevice(deviceId, deviceGroupId, frequency);
                return RedirectToAction("Devices");
            }
            else
            {
                _deviceGroupService.AddFrequency(deviceGroupId, frequency);
                _logger.LogInformation($"{_userId}|User {userName} submitted frequency for device group {deviceGroupId}");
                _server.ChangeFrequencyForDevice(deviceId, deviceGroupId, frequency);
                return RedirectToAction("DeviceGroups");
            }
        }

        [HttpPost]
        public IActionResult ImportFrequency(IFormFile uploadedFile, long deviceId, long deviceGroupId)
        {
            var userName = _userService.GetUsers().SingleOrDefault(u => u.Id == _userId);
            if (uploadedFile?.ContentType != "text/plain")
                return RedirectToAction("Devices");

            var fileName = Path.GetFileName(uploadedFile.FileName);

            if (System.IO.File.Exists(fileName))
            {
                System.IO.File.Delete(fileName);
            }

            using (var localFile = System.IO.File.OpenWrite(fileName))
            {
                using (var uplFile = uploadedFile.OpenReadStream())
                {
                    uplFile.CopyTo(localFile);
                }

            }

            using (StreamReader reader = new StreamReader(fileName))
            {

                if (int.TryParse(reader.ReadToEnd(), out var frequency))
                {
                    if (deviceId != 0)
                    {
                        _deviceService.AddFrequency(deviceId, frequency);
                        _logger.LogInformation($"{_userId}|User {userName} imported frequency for device {deviceId}");
                        _server.ChangeFrequencyForDevice(deviceId, deviceGroupId, frequency);
                        return RedirectToAction("Devices");
                    }
                    else if (deviceGroupId != 0)
                    {
                        _deviceGroupService.AddFrequency(deviceGroupId, frequency);
                        _logger.LogInformation($"{_userId}|User {userName} imported frequency for device group {deviceGroupId}");
                        _server.ChangeFrequencyForDevice(deviceId, deviceGroupId, frequency);
                        return RedirectToAction("DeviceGroups");
                    }
                }
            }
            return RedirectToAction("Devices");
        }

        [HttpPost]
        public async Task<FileResult> ExportFrequency(long deviceId)
        {
            var userName = _userService.GetUsers().SingleOrDefault(u => u.Id == _userId);
            Device device = _deviceService.GetDevices().SingleOrDefault(d => d.Id == deviceId);
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "tempFile.txt");
            using (StreamWriter fs = new StreamWriter(path))
            {
                fs.WriteLine(device.Frequency);
            }

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            _logger.LogInformation($"{_userId}|User {userName} exported frequency for device {deviceId}");

            return File(memory, "text/plain", "frequence.txt");

        }

        [HttpPost]
        public async Task<IActionResult> UploadVideo(IFormFile uploadedVideo, long deviceId)
        {
            if (uploadedVideo?.ContentType != "video/mp4")
                return RedirectToAction("Advertising", new { deviceId });

            using (var fileStream = new FileStream(Path.Combine(_dir, "file.mp4"), FileMode.Create, FileAccess.Write))
            {
                await uploadedVideo.CopyToAsync(fileStream);
            }

            Advertisement advertisement = new Advertisement();
            var filePath = _advertisingService.AddAdvertising(advertisement, deviceId, uploadedVideo.Length);

            _server.AddAdvertisement(deviceId, advertisement);
            _logger.LogInformation($"{_userId}|User {_userService.GetUsers().SingleOrDefault(u=>u.Id == _userId)} Uploaded Video for device {deviceId}");
            await ConvertVideo(filePath);
            return RedirectToAction("Advertising", new { deviceId });
        }

        public async Task<bool> ConvertVideo(string output)
        {
            try
            {
                var input = Path.Combine(_dir, "file.mp4");

                FFmpeg.ExecutablesPath = Path.Combine(_dir, "ffmpeg");

                var info = await MediaInfo.Get(input);

                var videoStream = info.VideoStreams.First()
                    .SetCodec(VideoCodec.H264)
                    .SetSize(VideoSize.Hd480);

                await Conversion.New()
                    .AddStream(videoStream)
                    .SetOutput(output)
                    .Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return true;
        }

    }
}