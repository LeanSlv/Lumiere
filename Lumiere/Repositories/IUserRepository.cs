﻿using Lumiere.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Lumiere.Repositories
{
    public interface IUserRepository
    {
        Task<IdentityResult> CreateWithoutPasswordAsync(User user);
        Task<IdentityResult> CreateWithPasswordAsync(User user, string password);
        Task<IdentityResult> UpdateAsync(User user);
        Task<IdentityResult> DeleteAsync(User user);
        IEnumerable<User> GetAll();
        Task<User> GetByIdAsync(string userId);
        Task<User> GetByNameAsync(string name);
        Task<User> GetCurrentUser(ClaimsPrincipal currentUserClaims);
        Task<string> GenerateEmailConfirmationTokenAsync(User user);
        Task<IdentityResult> ConfirmEmailAsync(User user, string token);
        Task<bool> IsEmailConfirmedAsync(User user);
        Task<string> GeneratePasswordResetTokenAsync(User user);
        Task<IdentityResult> ResetPasswordAsync(User user, string token, string newPassword);
    }
}