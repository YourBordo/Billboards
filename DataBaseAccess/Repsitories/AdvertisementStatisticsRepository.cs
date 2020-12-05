using Billboards.Models;
using DataBaseAccess.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DataBaseAccess.Repsitories
{
    public class AdvertisementStatisticsRepository : BaseRepository<AdvertisementStatistics>
    {
        public AdvertisementStatisticsRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext) { }
        public override AdvertisementStatistics Get(long itemId)
        {
            return _applicationDbContext.AdvertisementStatistics
                .Include(a => a.Advertisement)
                    .ThenInclude(ast => ast.AdvertisementStatistics)
                .SingleOrDefault(d => d.Id == itemId);
        }

        public override IEnumerable<AdvertisementStatistics> GetAll()
        {
            return _applicationDbContext.AdvertisementStatistics
                .Include(a => a.Advertisement)
                    .ThenInclude(ast => ast.AdvertisementStatistics)
                .Where(d => d.IsDeleted == false).ToList();
        }


        public override void Update(AdvertisementStatistics item)
        {
            _applicationDbContext.Update(item);
            _applicationDbContext.SaveChanges();
        }

        public override void Create(AdvertisementStatistics item)
        {
            _applicationDbContext.AdvertisementStatistics.Add(item);
            _applicationDbContext.SaveChanges();
        }

        public override void Delete(AdvertisementStatistics item)
        {
            Update(item);
        }
    }
}
