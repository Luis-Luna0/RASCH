using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RASCH.API.Data;
using RASCH.API.Data.Entities;
using RASCH.API.Models;
using RASCH.Common.Enums;

namespace RASCH.API.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DataContext _context;
        private readonly SignInManager<User> _signInManager;

        public UserHelper(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, DataContext context, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }
        //Registro
        public async Task<User> AddUserAsync(AddUserViewModel model, Guid imageId, UserType userType)
        {
            User user = new User
            {
                Address = model.Address,
                Email = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,
                ImageId = imageId,
                PhoneNumber = model.PhoneNumber,
                Business = await _context.Business.FindAsync(model.BusinessId),
                UserName = model.Username,
                UserType = userType
            };

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            if (result != IdentityResult.Success)
            {
                return null;
            }

            User newUser = await GetUserAsync(model.Username);
            await AddUserToRoleAsync(newUser, user.UserType.ToString());
            return newUser;
        }

        public async Task AddUserToRoleAsync(User user, string roleName)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }


        public async Task CheckRoleAsync(string roleName)
        {
            bool roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = roleName });
            }
        }

        public async Task<IdentityResult> DeleteUserAsync(User user)
        {
            return await _userManager.DeleteAsync(user);
        }

        public async Task<User> GetUserAsync(string email)
        {
            return await _context.Users.Include(x => x.Business)
                .FirstOrDefaultAsync(x => x.Email == email);
        }
        /*Aqui despues de el segundo include la sintaxis es ThenInclude*/
        public async Task<User> GetUserAsync(Guid id)
        {
            return await _context.Users
                .Include(x => x.Business)
                .FirstOrDefaultAsync(x => x.Id == id.ToString());
        }

        public async Task<bool> IsUserInRoleAsync(User user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }
        // metodo para iniciar sesion
        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            return await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);
        }
        // metodo para salir de la sesion
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        //Update de la edición del usuario desde el admin
        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            User currentUser = await GetUserAsync(user.Email);
            currentUser.LastName = user.LastName;
            currentUser.FirstName = user.FirstName;
            currentUser.Business = user.Business;
            currentUser.Address = user.Address;
            currentUser.ImageId = user.ImageId;
            currentUser.PhoneNumber = user.PhoneNumber;
            return await _userManager.UpdateAsync(currentUser);
        }
    }
}
