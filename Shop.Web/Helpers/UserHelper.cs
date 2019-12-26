using Microsoft.AspNetCore.Identity;
using Shop.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Web.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly UserManager<User> _userManger;

        public UserHelper(UserManager<User> userManger)
        {
            _userManger = userManger;
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password) => await _userManger.CreateAsync(user,password);
      

        public async Task<User> GetUserBiEmailAsync(string email) => await _userManger.FindByEmailAsync(email);
    }
}
