using Billboards.Models;
using Billboards.ModelsView;
using Microsoft.AspNetCore.Mvc;
using ModelServices.AdvertisingServicing;
using ModelServices.DeviceGroupServicing;
using ModelServices.DeviceServicing;
using ModelServices.UserLogServicing;
using ModelServices.UserServicing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Server;

namespace Billboards.Controllers
{
    public class AdministratorController : Controller
    {
        private readonly IUserService _userService;

        private readonly IAdvertisingService _advertisingService;

        private readonly IDeviceGroupService _deviceGroupService;

        private readonly IDeviceService _deviceService;

        private readonly IUserLogService _userLogService;

        private IServerEmulation _server;
        public AdministratorController(IUserService userService,
            IAdvertisingService advertisingService,
            IDeviceGroupService deviceGroupService,
            IDeviceService deviceService,
            IUserLogService userLogService,
            IServerEmulation server)
        {
            _server = server;
            _userService = userService;
            _advertisingService = advertisingService;
            _deviceGroupService = deviceGroupService;
            _deviceService = deviceService;
            _userLogService = userLogService;

        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
            ViewBag.DeviceAmount = _deviceService.GetDevices().Count();
            ViewBag.UserAmount = _userService.GetUsers().Count();
            ViewBag.FileAmount = _advertisingService.GetAdvertisements(0).Count();
        }
        public IActionResult Users()
        {

            IEnumerable<User> users = _userService.GetUsers();
            return View(users);
        }

        public IActionResult Logs(long userId)
        {
            IEnumerable<UserLog> usersLogs = _userLogService.GetUserLogs(userId);
            if (!usersLogs.Any())
            {
                return RedirectToAction("Users");
            }
            return View(usersLogs);
        }

        [HttpPost]
        public async Task<FileResult> DownLoadLogs(long userId, DateTime? startPeriod, DateTime? finishPeriod)
        {
            string path = _userLogService.DownloadHisory(userId, startPeriod, finishPeriod);
            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, "text/plain", "logs.txt");
        }

        public IActionResult Devices()
        {
            IEnumerable<Device> devices = _deviceService.GetDevices();
            IEnumerable<User> users = _userService.GetUsers();

            UsersDevicesModelView usersDevicesModel = new UsersDevicesModelView(users, devices);
            return View(usersDevicesModel);
        }

        public IActionResult Advertising(long deviceId)
        {
            Advertisement advertisement = _server.GetNextAdv(deviceId);
            return View(advertisement);
        }
        public IActionResult DeviceGroups()
        {
            IEnumerable<DeviceGroup> deviceGroups = _deviceGroupService.GetDeviceGroups();
            IEnumerable<User> users = _userService.GetUsers();
            UsersDeviceGroupsModelView usersDeviceGroupsModel = new UsersDeviceGroupsModelView(users, deviceGroups);

            return View(usersDeviceGroupsModel);
        }

        [HttpPost]
        public IActionResult AddUser(string name)
        {
            _userService.AddUser(name);
            return RedirectToAction("Users", "Administrator");
        }

        [HttpPost]
        public IActionResult DeleteUser([FromRoute] User user)
        {
            _userService.DeleteUser(user);
            return RedirectToAction("Users", "Administrator");
        }

        public IActionResult AddDevice(long id, int memory = 32)
        {
            _deviceService.AddDevice(id, memory);
            return RedirectToAction("Devices", "Administrator");
        }

        [HttpPost]
        public IActionResult DeleteDevice(long id)
        {
            _deviceService.DeleteDevice(id);
            return RedirectToAction("Devices", "Administrator");
        }

        public IActionResult AddDeviceGroup(long id)
        {
            _deviceGroupService.AddDeviceGroup(id);
            return RedirectToAction("DeviceGroups", "Administrator");
        }
        public IActionResult DeleteDeviceGroup(long id)
        {
            _deviceGroupService.DeleteDeviceGroup(id);
            return RedirectToAction("DeviceGroups", "Administrator");
        }

        public IActionResult VideoEnding(long deviceId)
        {
            return RedirectToAction("Advertising", "Administrator", new { deviceId });
        }
    }
}