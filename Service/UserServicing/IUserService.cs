using Billboards.Models;
using System.Collections.Generic;

namespace ModelServices.UserServicing
{
    public interface IUserService 
    {
        void DeleteUser(User user);

        void AddUser(string userName);

        IEnumerable<User> GetUsers();
    }
}
