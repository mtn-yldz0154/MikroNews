using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MikroNews.WebUI.Idnetity;
using MikroNews.WebUI.Models;

namespace MikroNews.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        
        public AccountController( UserManager<User> userManager, SignInManager<User> signInManager)
        {
           
            _userManager = userManager;
            _signInManager = signInManager;
           
        }
        public IActionResult Login()
        {
            return View();
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginModel model)
		{


			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user == null)
			{
				ModelState.AddModelError("", "Kullanıcı Bulunamadı");
				return View(model);
			}



			var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);

			if (result.Succeeded)
			{
				return Redirect("/admin/index");
			}
            

			return View(model);
		}
       
		
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            
            return Redirect("~/");
        }
    }
}
