using RASCH.API.Data;
using RASCH.API.Data.Entities;
using RASCH.API.Models;

namespace RASCH.API.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;

        public ConverterHelper(DataContext context, ICombosHelper combosHelper)
        {
            _context = context;
            _combosHelper = combosHelper;
        }

        public async Task<User> ToUserAsync(UserViewModel model, Guid imageId, bool isNew)
        {
            return new User
            {
                Address = model.Address,
                Business = await _context.Business.FindAsync(model.BusinessId),
                Email = model.Email,
                FirstName = model.FirstName,
                Id = isNew ? Guid.NewGuid().ToString() : model.Id,
                ImageId = imageId,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                UserName = model.Email,
                UserType = model.UserType,
            };
        }

        /*  public Task<User> ToUserAsync(UserViewModel model, Guid imageId, bool isNew)
          {
              throw new NotImplementedException();
          }*/

        public UserViewModel ToUserViewModel(User user)
        {
            return new UserViewModel
            {
                Address = user.Address,
                BusinessId = user.Business.Id,
                Business = _combosHelper.GetComboBusiness(),
                Email = user.Email,
                FirstName = user.FirstName,
                Id = user.Id,
                ImageId = user.ImageId,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                UserType = user.UserType,
            };
        }
        /*
        public UserViewModel ToUserViewModel(User user)
        {
            throw new NotImplementedException();
        }
        */
        
    }
}