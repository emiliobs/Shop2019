using Microsoft.AspNetCore.Identity;
using Shop.Web.Data.Entities;
using Shop.Web.Models;
using System;
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

        public async Task<IdentityResult> AddUserAsync(User user, string password) => await _userManger.CreateAsync(user, password);

        public async Task<User> GetUserBiEmailAsync(string email) => await _userManger.FindByEmailAsync(email);

        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            return await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword)
        {
            return await _userManger.ChangePasswordAsync(user, oldPassword, newPassword);
        }

        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            return await _userManger.UpdateAsync(user);
        }

        public async Task<SignInResult> ValidatePasswordAsync(User user, string password)
        {
            return await _signInManager.CheckPasswordSignInAsync(user, password, false);
        }
    }
}
