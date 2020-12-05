using Billboards.Models;
using DataBaseAccess.Repsitory;
using ModelServices.UserServicing;
using System.Collections.Generic;
using System.Linq;

namespace ModelServices
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _repository;

        public UserService(IRepository<User> repository)
        {
            _repository = repository;
        }

        public void DeleteUser(User user)
        {
            _repository.Delete(user);
        }

        public void AddUser(string userName)
        {
            User user = new User()
            {
                UserName = userName
            };
            _repository.Create(user);
        }

        public IEnumerable<User> GetUsers()
        {
            return _repository.GetAll().ToList();
        }

    }
}
