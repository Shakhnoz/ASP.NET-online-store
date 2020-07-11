using DAL.Entities;
using DAL.Repository;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Security;
using WAD.Models;

namespace WAD.Controllers
{
    public class HomeController : Controller
    {
        private UserRepository _userRepo = new UserRepository();
        private static NLog.Logger logger = NLog.LogManager.GetLogger("WebSite");//logger name specified


        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult Registration()
        {
            return View(new RegisterModel());
        }

        //private readonly Context _context = new Context();
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Registration(RegisterModel model)
        {
            if (model.Password != model.ConfirmPassword)
                ModelState.AddModelError("ConfirmPassword", "Doesn't match with the password");

            if (ModelState.IsValid && ValidateCaptcha())
            {
                if (_userRepo.GetAll().Any(u => u.Username == model.Username))
                {
                    ModelState.AddModelError("Username", "User with such Username already registered");
                }
                else
                {
                    var user = new User
                    {
                        Firstname = model.Firstname,
                        Lastname = model.Lastname,
                        Address = model.Address,
                        Email = model.Email,
                        Username = model.Username,
                        Password = model.Password,
                    };

                    _userRepo.Create(user);

                    logger.Info(model.Username + " got registered");


                    return RedirectToAction("Login");
                }

            }

            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View(new LoginModel());
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                if (!_userRepo.GetAll().Any(u => u.Username == model.Username && u.Password == model.Password))
                {
                    ModelState.AddModelError("Username", "Wrong credentials");
                    ModelState.AddModelError("Password", "Wrong credentials");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(model.Username, false);
                    logger.Info(model.Username + " logged in");
                    return RedirectToAction("Index");

                }

            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }


        private class CaptchaResponse
        {
            [JsonProperty("success")]

            public bool Success { get; set; }


            [JsonProperty("error-codes")]
            public List<string> ErrorCodes { get; set; }
        }

        private bool ValidateCaptcha()
        {
            var client = new WebClient();
            var reply = client.DownloadString(
                        string.Format("https://www.google.com/recaptcha/api/siteverify?secret=6LfwhNQUAAAAAMN4M0I7V9XcuTNaC2Q-xDQbsrPk&response={0}"
                        , Request["g-recaptcha-response"]));
            var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(reply);
            if (!captchaResponse.Success)
            {
                if (captchaResponse.ErrorCodes.Count <= 0) return false;
                var error = captchaResponse.ErrorCodes[0].ToLower();

                switch (error)
                {
                    case ("missing-input-secret"):
                        ModelState.AddModelError("", "The secret parameter is missing.");
                        break;
                    case ("invalid-input-secret"):
                        ModelState.AddModelError("", "The secret parameter is invalid or malformed.");
                        break;
                    case ("missing-input-response"):
                        ModelState.AddModelError("", "The response parameter is missing. Please, preceed with reCAPTCHA.");
                        break;
                    case ("invalid-input-response"):
                        ModelState.AddModelError("", "The response parameter is invalid or malformed.");
                        break;
                    default:
                        ModelState.AddModelError("", "Error occured. Please try again");
                        break;
                }
                return false;
            }
            return true;
        }





        public ActionResult About()
        {

            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}