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
using Microsoft.AspNetCore.Authorization;
using System.Drawing;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Imaging;
using System.Text;


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
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        #region-- > Login Using SP and Cokkie Authentication
        public async Task<ActionResult> Login(LoginViewModel LoginViewModel, User user)
        {
            try
            {
                //bool status = Validation_Input_UserEmail_Twice(model);
                using SqlConnection con = new(connectionString);  // establishing connection with database 

                using (SqlCommand cmd = new SqlCommand("sp_login_user", con))
                {
                    await con.OpenAsync();  // opening a connection
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = LoginViewModel.Email; // Adding Values into params
                    cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = LoginViewModel.Password; // Adding Values into params
                    int status;
                    status = Convert.ToInt16(cmd.ExecuteScalar()); // ExecuteScalar method is used to retrieve a single value from DB
                    if (status == 1)
                    {
                        var userFirstName = _dummyAppContext.Users.Where(data => data.Email == LoginViewModel.Email).Select(user => user.FirstName).FirstOrDefault();
                        var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, userFirstName) }, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                        HttpContext.Session.SetString("Email", LoginViewModel.Email);
                        ViewBag.EmailID = HttpContext.Session.GetString("Email");
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
            catch (Exception)
            {
                throw;
            }

            // <------ Previous Approch ------>

            //var users = _dummyAppContext.Users.Where(u => u.Email == loginmodel.Email).FirstOrDefault();
            //if (users.Email == loginmodel.Email && users.Password == loginmodel.Password)
            //{
            //   // Response.Redirect("Index", "CRUD");
            //    //return RedirectToAction("Index", "CRUD");
            //}
            //TempData["Error"] = "Invalid User Credentials!!";
            //return RedirectToAction("Login", "Account");

        }
        #endregion

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        /// <summary>
        /// redirect to login if email is not already exists
        /// </summary>
        /// <param name="model"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Registration(RegistrationViewModel model, User user)
        {
            var users = _dummyAppContext.Users.Where(u => u.Email == model.Email).FirstOrDefault();

            if (users?.Email == model.Email)
            {
                TempData["Error"] = "Oops Email already exists, Please try with another Email!";
            }
            else
            {
                _accountRepository.AddUser(model);
                return RedirectToAction("Login");
            }
            return View();
        }

        public IActionResult ResetPassword()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordViewModel forgotPasswordViewModel)
        {
            string emailadd = forgotPasswordViewModel.Email;
            if (!string.IsNullOrEmpty(emailadd))
            {
                byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
                byte[] key = Guid.NewGuid().ToByteArray();
                string token = Convert.ToBase64String(time.Concat(key).ToArray());
                var linkHref = "https://localhost:7260/Account/ResetPassword?token=" + token + "&email=" + emailadd;
                string subject = "Please Reset your password";
                string body = "Link to Reset your password :" + " " + linkHref;
                _emailRepository.SendEmail(emailadd, subject, body);
            }
            else
            {
                TempData["Error"] = "Sorry for the inconvenience";
                return View();
            }

            return View();
        }
    
        [HttpGet]
        public JsonResult GetImageData()
        {
            var images = _dummyAppContext.ImageComps.FromSqlRaw("sp_getimage").ToList();

            return Json(images);
        }
    
        [HttpGet]
        public IActionResult ImageUploadThumb()
        {

            return View();
        }

        [HttpPost]
        public IActionResult ImageUploadThumb(ImageUploadViewModel imageUploadViewModel)
        {
            if (imageUploadViewModel != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    imageUploadViewModel.ImagePath.CopyTo(memoryStream);
                    var imageBytes = memoryStream.ToArray();
                    var imageBlob = Convert.ToBase64String(imageBytes);
                    var newimg = "data:image/png;base64," + imageBlob;
                    //using (var image = Image.FromStream(memoryStream))
                    //{
                    //    var height = image.Height;
                    //    var width = image.Width;
                    //}

                    ImageComp img = new ImageComp()
                    {
                        ImagePath = newimg
                    };
                    _dummyAppContext.ImageComps.Add(img);
                    _dummyAppContext.SaveChanges();

                }
            }
            return View();
        }

        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var storedCookie = Request.Cookies.Keys;
            foreach (var cookie in storedCookie)
            {
                Response.Cookies.Delete(cookie);
                HttpContext.Session.Clear();
            }
            return RedirectToAction("Login", "Account");
        }
    }
}

//var getImages = _dummyAppContext.ImageComps.FromSqlRaw("sp_getimage").ToList();
//foreach (var image in getImages)
//{
//    var imageData = Convert.FromBase64String(image.ImagePath);

//    using (var stream = new MemoryStream(imageData))
//    {
//        using (var originalImage = Image.FromStream(stream))
//        {
//            using (var compressedImage = new Bitmap(originalImage.Width, originalImage.Height))
//            {
//                using (var graphics = Graphics.FromImage(compressedImage))
//                {
//                    var compressionQuality = 50;

//                    var compressionFormat = ImageFormat.Jpeg;

//                    var encoderParameters = new EncoderParameters(1);
//                    encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, compressionQuality);

//                    graphics.DrawImage(originalImage, new Rectangle(0, 0, originalImage.Width, originalImage.Height));
//                    graphics.Dispose();

//                    using (var outputStream = new MemoryStream())
//                    {
//                        compressedImage.Save(outputStream, GetEncoderInfo(compressionFormat), encoderParameters);

//                        var compressedImageData = outputStream.ToArray();
//                    }
//                }
//            }
//        }
//    }

//}