using E_Commerce.Models;
using Microsoft.AspNetCore.Identity;
using System.Text;
using E_Commerce.Areas.Identity.Data;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using E_Commerce.Repository;

namespace E_Commerce.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration configuration;

        public AccountRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
        }

       /* public async Task<IdentityResult> LockUserByEmailAsync(string UserName)
        {
            var user = await userManager.FindByEmailAsync(UserName);

            var lockoutEnd = DateTimeOffset.UtcNow.AddYears(100); // Lockout user for 100 years

            return await userManager.SetLockoutEndDateAsync(user, lockoutEnd);
        }

        public async Task<IdentityResult> UnlockUserByEmailAsync(string UserName)
        {
            var user = await userManager.FindByEmailAsync(UserName);

            return await userManager.SetLockoutEndDateAsync(user, null);
        }
*/
        public async Task<List<ApplicationUser>> ShowUserAsync()
        {
            var user = await userManager.Users.ToListAsync();
            return user;
        }

        public async Task<ApplicationUser> GetUesrByEmailAsync(string email)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(x => x.Email == email);
            return user;
        }

        /*
         public async Task<IdentityResult> ConfirmAccountAsync(string email)
         {
             var user = await userManager.FindByEmailAsync(email);
             user.EmailConfirmed = true;
             return await userManager.UpdateAsync(user);
         }

         public async Task<IdentityResult> RestPassAsync(string email, string newPass)
         {
             var user = await userManager.FindByEmailAsync(email);
             var token = await userManager.GeneratePasswordResetTokenAsync(user);
             return await userManager.ResetPasswordAsync(user, token, newPass);
         }


         public async Task<IdentityResult> ChangePass(string email, string NewPass, string CurrentPass)
         {
             string plainPassword = NewPass;

             var user = await userManager.FindByEmailAsync(email);

             return await userManager.ChangePasswordAsync(user, CurrentPass, plainPassword);

         }*/
    }
}
