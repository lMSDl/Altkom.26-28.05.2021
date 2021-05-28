using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class UserService : IUserService
    {
        private ICollection<User> _users = new List<User>();

        public int Create(User user)
        {
            var id = _users.Count != 0 ? _users.Max(x => x.Id) + 1 : 1;
            user = (User)user.Clone();
            //user.Id = id;
            _users.Add(user);

            return id;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public User Read(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> Read()
        {
            throw new NotImplementedException();
        }

        public void Update(int id, User user)
        {
            throw new NotImplementedException();
        }
    }
}
