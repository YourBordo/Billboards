using Billboards.Models;
using System.Collections.Generic;

namespace Billboards.ModelsView
{
    public class UserDevicesModelView
    {
        public User User { get; set; }

        public IEnumerable<Device> Devices { get; set; }

        
        public UserDevicesModelView(User user, IEnumerable<Device> devices)
        {
            User = user;
            Devices = devices;
            
        }
    }
}
