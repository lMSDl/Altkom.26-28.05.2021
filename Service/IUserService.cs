using Service.Models;
using System;
using System.Collections.Generic;

namespace Service
{
    public interface IUserService
    {
        int Create(User user);
        User Read(int id);
        IEnumerable<User> Read();
        void Update(int id, User user);
        void Delete(int id);
    }
}
