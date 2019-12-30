using Microsoft.AspNetCore.Identity;
using Shop.Web.Data.Entities;
using Shop.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Web.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly UserManager<User> _userManger;
        private readonly SignInManager<User> _signInManager;

        public UserHelper(UserManager<User> userManger, SignInManager<User> signInManager)
        {
            _userManger = userManger;
           _signInManager = signInManager;
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password) => await _userManger.CreateAsync(user,password);
      

        public async Task<User> GetUserBiEmailAsync(string email) => await _userManger.FindByEmailAsync(email);

        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            return await _signInManager.PasswordSignInAsync(model.UserName,model.Password, model.RememberMe,false);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
