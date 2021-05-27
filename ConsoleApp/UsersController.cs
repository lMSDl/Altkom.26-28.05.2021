using Service;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class UsersController
    {
        private IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }

        public IEnumerable<User> Get()
        {
            var result = _service.Read();

            return result;
        }

        public User Get(int id)
        {
            return _service.Read(id);
        }


        public int Post(User user)
        {
            return _service.Create(user);
        }

        public bool Put(int id, User user)
        {
            if (_service.Read(id) == null)
                return false;

            _service.Update(id, user);
            return true;
        }


        public bool Delete(int id)
        {
            if (_service.Read(id) == null)
                return false;

            _service.Delete(id);
            return true;
        }
    }
}