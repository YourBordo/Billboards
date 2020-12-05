using Billboards.Models;
using DataBaseAccess.Repsitory;
using ModelServices.DeviceGroupServicing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ModelServices
{
    public class DeviceGroupService : IDeviceGroupService
    {
        private readonly IRepository<Device> _deviceRepository;
        private readonly IRepository<DeviceGroup> _deviceGroupRepository;
        private readonly IRepository<User> _userRepository;
        public DeviceGroupService(IRepository<User> userRepository, IRepository<DeviceGroup> deviceGroupRepository, IRepository<Device> deviceRepository)
        {
            _userRepository = userRepository;
            _deviceGroupRepository = deviceGroupRepository;
            _deviceRepository = deviceRepository;
        }

        public IEnumerable<DeviceGroup> GetDeviceGroups(long? userId = null)
        {
            if (userId == null)
            {
                return _deviceGroupRepository.GetAll();
            }
            else
            {
                var a = _deviceGroupRepository.GetAll();
                return a.Where(d => d.User.Id == (long)userId);
            }
        }

        public void AddDeviceGroup(long id)
        {
            User user = _userRepository.GetAll().SingleOrDefault(u => u.Id == id);
            DeviceGroup deviceGroup = new DeviceGroup { User = user };

            if (user != null)
            {
                user.DeviceGroups.Add(deviceGroup);

                _deviceGroupRepository.Create(deviceGroup);
                _userRepository.Update(user);
            }
        }

        public void DeleteDeviceGroup(long id)
        {
            DeviceGroup deviceGroup = _deviceGroupRepository.GetAll().SingleOrDefault(dg => dg.Id == id);
            deviceGroup.IsDeleted = true;
            _deviceGroupRepository.Delete(deviceGroup);
        }

        public void ImportFrequency(FileStream fileStream)
        {
            throw new NotImplementedException();
        }

        public void AddDeviceInGroup(long deviceGroupId, long deviceId)
        {
            DeviceGroup deviceGroup = _deviceGroupRepository.Get(deviceGroupId);
            Device device = _deviceRepository.Get(deviceId);
            deviceGroup.Devices.Add(device);
            device.DeviceGroup = deviceGroup;
            
            _deviceGroupRepository.Update(deviceGroup);
            _deviceRepository.Update(device);
        }


        public void AddFrequency(long deviceGroupId, int frequence)
        {
            DeviceGroup deviceGroup = _deviceGroupRepository.Get(deviceGroupId);
            foreach (var device in deviceGroup.Devices)
            {
                device.Frequency = frequence;
                _deviceRepository.Update(device);
            }
            _deviceGroupRepository.Update(deviceGroup);
        }



        public void DeleteDeviceInGroup(long deviceGroupId, long deviceId)
        {
            DeviceGroup deviceGroup = _deviceGroupRepository.Get(deviceGroupId);
            Device device = _deviceRepository.Get(deviceId);
            deviceGroup.Devices.Remove(device);
            device.DeviceGroup = null;

            _deviceGroupRepository.Update(deviceGroup);
            _deviceRepository.Update(device);
        }
    }
}
