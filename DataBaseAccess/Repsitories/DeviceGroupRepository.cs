using Billboards.Models;
using DataBaseAccess.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataBaseAccess.Repsitories
{
    public class DeviceGroupRepository : BaseRepository<DeviceGroup>
    {
        public DeviceGroupRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext) { }
        public override DeviceGroup Get(long itemId)
        {
            return _applicationDbContext.DeviceGroups.Include(u => u.User)
                .Include(d => d.Devices)
                .SingleOrDefault(dg => dg.Id == itemId);
        }

        public override IEnumerable<DeviceGroup> GetAll()
        {
            return _applicationDbContext.DeviceGroups.
                Include( u => u.User).
                Include(d => d.Devices).
                Where(d => d.IsDeleted == false);
        }



        public override void Update(DeviceGroup item)
        {
            _applicationDbContext.DeviceGroups.Update(item);
            _applicationDbContext.SaveChanges();
        }

        public override void Create(DeviceGroup item)
        {
            _applicationDbContext.DeviceGroups.Add(item);
            _applicationDbContext.SaveChanges();
        }

        public override void Delete(DeviceGroup item)
        {
            Update(item);
        }
    }
}
