using Billboards.Models;
using System.Collections.Generic;

namespace Billboards.ModelsView
{
    public class UsersDevicesModelView
    {
        public IEnumerable<User> Users { get; set; }

        public IEnumerable<Device> Devices { get; set; }

        public UsersDevicesModelView(IEnumerable<User> users, IEnumerable<Device> devices)
        {
            Users = users;
            Devices = devices;
        }

    }
}
