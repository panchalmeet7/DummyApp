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
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace DummyApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly DummyAppContext _dummyAppContext;
        private readonly EmailRepository _emailRepository;
        private readonly AccountRepository _accountRepository;
        private string connectionString = "Data Source=PCT38\\SQL2019;Initial Catalog=DummyApp;User Id=sa;Password=Tatva@123;TrustServerCertificate=True;Integrated Security=True";
        public AccountController(ILogger<AccountController> logger, IEmailRepository emailRepository, IAccountRepository accountRepository, DummyAppContext dummyAppContext)
        {
            _logger = logger;
            _emailRepository = (EmailRepository?)emailRepository;
            _accountRepository = (AccountRepository?)accountRepository;
            _dummyAppContext = dummyAppContext;

        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("");
            }
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel LoginViewModel, User user)
        {
            try
            {
                //bool status = Validation_Input_UserEmail_Twice(model);

                using (SqlConnection con = new SqlConnection(connectionString))  // establishing connection with database 
                {
                    //<!-------- Login Stored procedure -------->

                    //  CREATE or ALTER PROCEDURE sp_login_user
                    //  (
                    //  @Email VARCHAR(150),
                    //  @Password varchar(250)
                    //  )
                    //  as
                    //  declare @status int
                    //  if exists(select * from dbo.[User] where Email = @Email and Password = @Password)
                    //        set @status = 1
                    //  else
                    //        set @status = 0
                    //  select @status

                    using (SqlCommand cmd = new SqlCommand("sp_login_user", con))
                    {
                        con.Open();  // opening a connection
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = LoginViewModel.Email; // Adding Values into params
                        cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = LoginViewModel.Password; // Adding Values into params
                        int status;
                        status = Convert.ToInt16(cmd.ExecuteScalar()); // ExecuteScalar method is used to retrieve a single value from DB
                        if (status == 1)
                        {
                            return RedirectToAction("Index", "CRUD");
                        }
                        else
                        {
                            TempData["Error"] = "Invalid User Credentials!!";
                            return RedirectToAction("Login", "Account");
                        }

                        cmd.ExecuteNonQuery(); // executing the query with given params
                        con.Close();
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }

            //Login without sp -->

            //var users = _dummyAppContext.Users.Where(u => u.Email == loginmodel.Email).FirstOrDefault();
            //if (users.Email == loginmodel.Email && users.Password == loginmodel.Password)
            //{
            //   // Response.Redirect("Index", "CRUD");
            //    //return RedirectToAction("Index", "CRUD");
            //}
            //TempData["Error"] = "Invalid User Credentials!!";
            //return RedirectToAction("Login", "Account");
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

            if (users.Email == model.Email)
            {
                TempData["Error"] = "Oops Email already exists, Please try another Email!";
            }
            else
            {
                _accountRepository.AddUser(model);
                return RedirectToAction("Login");
            }
            return View();
        }
        //public IActionResult Register(RegistrationViewModel model)
        //{
        //    try
        //    {
        //        if (model != null)
        //        {
        //            bool status = _accountRepository.Validation_Input_UserEmail_Twice(model);
        //            if (status)
        //            {
        //                ViewBag.Message = "Opss User Name already exsist, Please try another User Name !!!";
        //                return View("Registration");
        //            }
        //            else
        //            {
        //                ViewBag.Message = "Register New User successfully..";
        //                return View("Registration");
        //            }
        //        }
        //        return View("Registration");
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
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
