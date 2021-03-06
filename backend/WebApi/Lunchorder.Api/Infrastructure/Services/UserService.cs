using System;
using System.Linq;
using System.Threading.Tasks;
using Lunchorder.Common.Interfaces;
using Lunchorder.Domain.Entities.Authentication;
using Microsoft.AspNet.Identity;

namespace Lunchorder.Api.Infrastructure.Services
{
    public class ApplicationUserService : IUserService
    {
        private readonly Func<UserManager<ApplicationUser>> _userManager;
        private readonly Func<RoleManager<ApplicationRole, string>> _roleManager;

        public ApplicationUserService(Func<UserManager<ApplicationUser>> userManager, Func<RoleManager<ApplicationRole, string>> roleManager)
        {
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        /// <summary>
        /// Creates a new user without a password
        /// </summary>
        /// <param name="username">New username</param>
        /// <param name="email">Email of the user</param>
        /// <param name="firstName">First name of the user</param>
        /// <param name="lastName">Last name of the user</param>
        /// <returns></returns>
        public async Task<ApplicationUser> Create(string username, string email, string firstName, string lastName)
        {
            var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = username, Email = email, FirstName = firstName, LastName = lastName };
            IdentityResult result = await _userManager().CreateAsync(user);

            HandleError(result);

            return user;
        }

        /// <summary>
        /// Creates a new user with a password
        /// </summary>
        /// <param name="username">New username</param>
        /// <param name="email">Email of the user</param>
        /// <param name="firstName">First name of the user</param>
        /// <param name="lastName">Last name of the user</param>
        /// <param name="password">Password of the user</param>
        /// <returns></returns>
        public async Task<ApplicationUser> Create(string username, string email, string firstName, string lastName, string password)
        {
            var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), Balance = new Random().Next(100), UserName = username, Email = email, FirstName = firstName, LastName = lastName };
            IdentityResult result = await _userManager().CreateAsync(user, password);

            HandleError(result);

            return user;
        }

        public async Task UpdateUserPicture(string userId, string url)
        {
            var userManager = _userManager();
            var user = await userManager.FindByIdAsync(userId);
            user.Picture = url;
            await userManager.UpdateAsync(user);
        }

        /// <summary>
        /// Adds a user to a specific role
        /// </summary>
        /// <param name="userId">the id of the user</param>
        /// <param name="roleName">the name of the role</param>
        /// <returns></returns>
        public async Task AddUserToRole(string userId, string roleName)
        {
            await _userManager().AddToRoleAsync(userId, roleName);
        }

        public async Task AddRole(string roleName)
        {
            await _roleManager().CreateAsync(new ApplicationRole { Name = roleName });

        }

        private void HandleError(IdentityResult result)
        {
            // todo better error handling
            if (result == null)
            {
                throw new Exception("Could not create user, response was null");
            }
            if (result.Errors != null && result.Errors.Any())
            {
                throw new Exception(string.Join(";", result.Errors));
            }
        }
    }
}