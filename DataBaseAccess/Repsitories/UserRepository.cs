using Billboards.Models;
using DataBaseAccess.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataBaseAccess.Repsitories
{
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext) { }
        public override User Get(long item)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<User> GetAll()
        {
            
            return _applicationDbContext.Users.Where(user => user.IsDeleted == false);
        }

        public override void Update(User item)
        {
            _applicationDbContext.Users.Update(item);
            _applicationDbContext.SaveChanges();
        }

        public override void Create(User user)
        {
            _applicationDbContext.Users.Add(user);
            _applicationDbContext.SaveChanges();
        }

        public override void Delete(User user)
        {
            user.IsDeleted = true;
            _applicationDbContext.Update(user);
            _applicationDbContext.SaveChanges();
        }
    }
}
