using Microsoft.AspNetCore.Mvc;
using GiantNationalBankClient.Models;
using GiantNationalBankClient.Utility;
using Newtonsoft.Json;

namespace GiantNationalBankClient.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index(LoginResponseModel userData)
        {
            // Ensure userData is not null and contains AdminData
            if (userData != null && userData.AdminData != null)
            {
                return View(userData);
            }
            else
            {
                // Handle the case where userData or AdminData is null, e.g., redirect to login
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsersView()
        {
            var userList = new List<User>();
            try
            {
                var _serviceHelper = new ServiceHelper();
                string response = await _serviceHelper.GetRequest("Admin", "GetUsers", true, HttpContext.Request.Headers["Authorization"].ToString());
                var userResponse = JsonConvert.DeserializeObject<GetUserResponseModel>(response);

                if (userResponse != null && userResponse.Status)
                {
                    userList = userResponse.User;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching users: {ex.Message}");
            }

            return View("UserListView", userList);
        }

        [HttpGet("GetUserTransactions/{userId}")]
        public async Task<IActionResult> GetUserTransactions(int userId)
        {
            var transactionsList = new List<Transactions>();
            try
            {
                var _serviceHelper = new ServiceHelper();
                string response = await _serviceHelper.GetRequest("Admin", $"GetTransactions/{userId}", true, HttpContext.Request.Headers["Authorization"].ToString());
                transactionsList = JsonConvert.DeserializeObject<List<Transactions>>(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching transactions for user {userId}: {ex.Message}");
            }

            return View("TransactionListView", transactionsList);
        }


        private string GetAuthToken()
        {
            return Request.Cookies["AuthToken"] ?? string.Empty;
        }
    }
}
