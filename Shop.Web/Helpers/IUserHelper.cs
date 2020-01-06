using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.Web.Data.Entities;
using Shop.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Shop.Web.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserBiEmailAsync(string email);
        Task<IdentityResult> AddUserAsync(User user, string password);

        Task<SignInResult> LoginAsync(LoginViewModel model);

        Task LogoutAsync();

        Task<IdentityResult> UpdateUserAsync(User  user);

        Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);
        Task<SignInResult> ValidatePasswordAsync(User user, string password);
    }
}
