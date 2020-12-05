using Billboards.Models;
using DataBaseAccess.DbContext;
using System;
using System.Collections.Generic;

namespace DataBaseAccess.Repsitories
{
    public class UserLogRepository : BaseRepository<UserLog>
    {
        public UserLogRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext) { }
        public override UserLog Get(long item)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<UserLog> GetAll()
        {
            throw new NotImplementedException();
        }

        public override void Update(UserLog item)
        {
            throw new NotImplementedException();
        }

        public override void Create(UserLog item)
        {
            throw new NotImplementedException();
        }

        public override void Delete(UserLog item)
        {
            throw new NotImplementedException();
        }
    }
}
