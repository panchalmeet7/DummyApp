using DummyApp.Entities.Data;
using DummyApp.Entities.Models;
using DummyApp.Entities.ViewModels;
using DummyApp.Repository.Interface;
using DummyApp.Repository.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DummyApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly DummyAppContext _dummyAppContext;
        private readonly EmailRepository _emailRepository;
        private readonly AccountRepository _accountRepository;
        public AccountController(ILogger<AccountController> logger, IEmailRepository emailRepository, IAccountRepository accountRepository, DummyAppContext dummyAppContext)
        {
            _logger = logger;
            emailRepository = _emailRepository;
            accountRepository = _accountRepository;
            _dummyAppContext = dummyAppContext;

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
        public IActionResult Registration(RegistrationViewModel model, User user)
        {
            var users = _dummyAppContext.Users.Where(u => u.Email == model.Email).FirstOrDefault();
            if (model != null)
            {
                if (users.Email == model.Email)
                {
                    ViewBag.Message = "Opss Email already exsist, Please try another Email!!!";
                    return View();
                }
                else
                {
                    _accountRepository.AddUser(model);
                    return RedirectToAction("Login");
                }
            }
            else
            {
                return View();
            }
        }
        public IActionResult Register(RegistrationViewModel model)
        {
            try
            {
                if (model != null)
                {
                    bool status = _accountRepository.Validation_Input_UserEmail_Twice(model);
                    if (status)
                    {
                        ViewBag.Message = "Opss User Name already exsist, Please try another User Name !!!";
                        return View("Registration");
                    }
                    else
                    {
                        ViewBag.Message = "Register New User successfully..";
                        return View("Registration");
                    }
                }
                return View("Registration");
            }
            catch (Exception)
            {
                throw;
            }
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
