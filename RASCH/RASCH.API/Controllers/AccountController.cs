using Microsoft.AspNetCore.Mvc;
using RASCH.API.Data;
using RASCH.API.Data.Entities;
using RASCH.API.Helpers;
using RASCH.API.Models;
using RASCH.Common.Enums;

namespace RASCH.API.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IBlobHelper _blobHelper;
        //private readonly IMailHelper _mailHelper;

        public AccountController(IUserHelper userHelper, DataContext context, ICombosHelper combosHelper, IBlobHelper blobHelper)//, IMailHelper mailHelper)
        {
            _userHelper = userHelper;
            _context = context;
            _combosHelper = combosHelper;
            _blobHelper = blobHelper;
            // this._mailHelper = mailHelper;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(Index), "Home");
            }

            return View(new LoginViewModel());
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _userHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(Request.Query["ReturnUrl"].First());
                    }

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos.");
            }

            return View(model);
        }

        //metodo para desloguearce
        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }
        /*metodo para accesos no autorizados*/
        public IActionResult NotAuthorized()
        {
            return View();
        }

        public IActionResult Register()
        {
            AddUserViewModel model = new AddUserViewModel
            {
                Business = _combosHelper.GetComboBusiness()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AddUserViewModel model)
        {

            Guid imageId = Guid.Empty;

            if (model.ImageFile != null)
            {
                imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "user");
            }

            User user = await _userHelper.AddUserAsync(model, imageId, UserType.User);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Este correo ya está siendo usado por otro usuario.");
                model.Business = _combosHelper.GetComboBusiness();
                return View(model);
            }

            LoginViewModel loginViewModel = new LoginViewModel
            {
                Password = model.Password,
                RememberMe = false,
                Username = model.Username
            };

            var result2 = await _userHelper.LoginAsync(loginViewModel);
            if (result2.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }




            model.Business = _combosHelper.GetComboBusiness();
            return View(model);
        }
    }
}