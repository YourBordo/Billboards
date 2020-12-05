using Billboards.Models;
using DataBaseAccess.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataBaseAccess.Repsitories
{
    public class DeviceRepository : BaseRepository<Device>
    {
        public DeviceRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext) { }
        public override Device Get(long itemId)
        {
            return _applicationDbContext.Devices.
                Include(user => user.User).
                Include(dgr => dgr.DeviceGroup).
                    ThenInclude(u => u.User).
                    ThenInclude(ds => ds.Devices).
                SingleOrDefault(d => d.Id == itemId);
        }

        public override IEnumerable<Device> GetAll()
        {
            return _applicationDbContext.Devices.
                Include(user => user.User).
                Include(dgr => dgr.DeviceGroup).
                    ThenInclude( u => u.User).
                    ThenInclude(ds => ds.Devices).
                Include(ad=>ad.Advertisements).
                Where(d => d.IsDeleted == false).ToList();
        }


        public override void Update(Device item)
        {
            _applicationDbContext.Update(item);
            _applicationDbContext.SaveChanges();
        }

        public override void Create(Device item)
        {
            _applicationDbContext.Devices.Add(item);
            _applicationDbContext.SaveChanges();
        }

        public override void Delete(Device item)
        {
            Update(item);
        }
    }
}
