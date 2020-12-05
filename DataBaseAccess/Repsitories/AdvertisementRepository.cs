using Billboards.Models;
using DataBaseAccess.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataBaseAccess.Repsitories
{
    public class AdvertisementRepository : BaseRepository<Advertisement>
    {
        public AdvertisementRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext) { }
        public override void Create(Advertisement item)
        {
            _applicationDbContext.Advertisements.Add(item);
            _applicationDbContext.SaveChanges();
        }

        public override void Delete(Advertisement item)
        {
            Update(item);
        }

        public override Advertisement Get(long itemId)
        {
            return _applicationDbContext.Advertisements
                .Include(d => d.Device)
                .ThenInclude(u => u.User)
                .Include(ads => ads.AdvertisementStatistics)
                .SingleOrDefault(ad => ad.Id == itemId);
        }

        public override IEnumerable<Advertisement> GetAll()
        {
            return _applicationDbContext.Advertisements
                .Include(d => d.Device)
                    .ThenInclude(u=>u.User)
                .Include(ads => ads.AdvertisementStatistics)
                .Where(ad=>ad.IsDeleted == false).ToList();
        }


        public override void Update(Advertisement item)
        {
            _applicationDbContext.Update(item);
            _applicationDbContext.SaveChanges();
        }
    }
}
