using DummyApp.Entities.Data;
using DummyApp.Entities.Models;
using DummyApp.Entities.ViewModels;
using DummyApp.Repository.Interface;
using DummyApp.Repository.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DummyApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly DummyAppContext _dummyAppContext;
        private readonly EmailRepository _emailRepository;
        private readonly AccountRepository _accountRepository;
        public AccountController(ILogger<AccountController> logger, IEmailRepository emailRepository, IAccountRepository accountRepository)
        {
            logger = _logger;
            emailRepository = _emailRepository;
            accountRepository = _accountRepository;

        }
        public IActionResult Index()
        {
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
        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Registration(RegistrationViewModel model)
        {
            if (model != null)
            {
                _accountRepository.AddUser(model);
                return RedirectToAction("Login");
            }
            else
            {
                return View();
            }
            //var newUser = new User()
            //{
            //    FirstName = model.FirstName,
            //    LastName = model.LastName,
            //    Email = model.Email,
            //    PhoneNumber = model.PhoneNumber,
            //    Gender = model.Gender,
            //    Password = model.Password,
            //};
            //_dummyAppContext.Users.Add(newUser);
            //_dummyAppContext.SaveChanges();
           
            
        }
        public IActionResult ResetPassword()
        {
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }
    }
}
