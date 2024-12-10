using Microsoft.AspNetCore.Mvc;
using GiantNationalBankClient.Models;
using Newtonsoft.Json;
using GiantNationalBankClient.Utility;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GiantNationalBank.Models;
using System.Reflection;

namespace GiantNationalBankClient.Controllers
{
    public class HomeController : Controller
    {
        public IConfiguration _configuration;

        public HomeController(IConfiguration config)
        {
            _configuration = config;
        }

        public IActionResult Index()
        {
            return View();
        }

        
        public IActionResult Registration(RegistrationModel userData)
        {
            if (ModelState.IsValid)
            {
                ProcessRegistration(userData);
                return View("Views/Home/Login.cshtml");
            }
            return View(userData);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProcessLogin(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var _serviceHelper = new ServiceHelper();
                var strSerializedData = JsonConvert.SerializeObject(loginModel);
                string apiEndpoint = loginModel.UserType == "User" ? "api/User/LoginUser" : "api/Admin/LoginAdmin";

                string response = await _serviceHelper.PostRequest(strSerializedData, apiEndpoint, false, string.Empty);
                var loginResponse = JsonConvert.DeserializeObject<LoginResponseModel>(response);

                if (loginResponse != null && loginResponse.Status)
                {
                    // Store JWT in cookie for secure persistence
                    Response.Cookies.Append("AuthToken", loginResponse.Authtoken, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTime.Now.AddDays(15)
                    });

                    if (loginModel.UserType == "User")
                    {
                        return RedirectToAction("Index", "User");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Admin", loginResponse);
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Login failed. Please check your credentials.";
                    return View("Login");
                }
            }
            return View("Login");
        }

        public async Task<RegistrationResponseModel> ProcessRegistration(RegistrationModel userData)
        {
            try
            {
                var strSerializedData = JsonConvert.SerializeObject(userData);
                var objService = new ServiceHelper();
                string response = await objService.PostRequest(strSerializedData, "User/RegisterUser", false, string.Empty);
                var responseModel = JsonConvert.DeserializeObject<RegistrationResponseModel>(response);
                return responseModel;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Registration API " + ex.Message);
                return new RegistrationResponseModel { Status = false, Message = "Registration failed." };
            }
        }


        private string GetAuthToken()
        {
            return Request.Cookies["AuthToken"] ?? string.Empty;
        }
    }
}
