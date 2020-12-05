using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Billboards.Models;

namespace Billboards.ModelsView
{
    public class DeviceAdvertisingsView
    {
        public Device Device { get; set; }
        public IEnumerable<Advertisement> Advertisements { get; set; }
    }
}
