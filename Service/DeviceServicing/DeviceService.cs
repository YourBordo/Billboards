using Billboards.Models;
using DataBaseAccess.Repsitory;
using ModelServices.DeviceServicing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Service
{
    public class DeviceService : IDeviceService
    {
        private readonly IRepository<Device> _deviceRepository;
        private readonly IRepository<User> _userRepository;
        public DeviceService(IRepository<Device> deviceRepository, IRepository<User> userRepository)
        {
            _deviceRepository = deviceRepository;
            _userRepository = userRepository;
        }

        public void AddDevice(long userId, int memory = 32)
        {
            User user = _userRepository.GetAll().SingleOrDefault(u => u.Id == userId);
            Device device = new Device {User = user, Memory = memory};
            user?.Devices.Add(device);

            _deviceRepository.Create(device);
            _userRepository.Update(user);
        }

        public void DeleteDevice(long id)
        {
            Device device = _deviceRepository.GetAll().SingleOrDefault(d => d.Id == id);
            if (device != null)
            {
                device.IsDeleted = true;
                _deviceRepository.Delete(device);
            }
        }

        public IEnumerable<Device> GetDevices(long? userId = null)
        {
            if (userId == null)
            {
                return _deviceRepository.GetAll();
            }
            else
            {
                return _deviceRepository.GetAll().Where(d => d.User.Id == (long)userId);
            }
           
        }

        public void Save(Device device)
        {
            _deviceRepository.Update(device);
        }
        public IEnumerable<Device> GetDevicesInGroup(long deviceGroupId)
        {
            return _deviceRepository.GetAll().Where(d => d.DeviceGroup?.Id == deviceGroupId);
        }

      
        public void AddFrequency(long deviceId, int frequency)
        {
           Device device = _deviceRepository.Get(deviceId);
           device.Frequency = frequency;
           _deviceRepository.Update(device);
        }

        public void ImportFrequency(FileStream fileStream)
        {
            throw new NotImplementedException();
        }

        public void ExportFrequency(Device device)
        {
            throw new NotImplementedException();
        }

    }
}
