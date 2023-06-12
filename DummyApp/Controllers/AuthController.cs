using DummyApp.Entities.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DummyApp.Controllers
{
    public class AuthController : Controller
    {
        #region Properties
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        #endregion

        #region Constructor
        public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        #endregion


        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Registration")]
        public IActionResult Registration()
        {
            return View();
        }

        /// <summary>
        /// This method regiter new user into our database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.FirstName, Email = model.Email };
                //userManager.AddIdentity(user);
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false); //Flag indicating whether the sign-in cookie should persist after the browser is closed.
                    return RedirectToAction("CRUD", "Index");
                }

                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View();
        }
         
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            return View();
        }

    }
}
