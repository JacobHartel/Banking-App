using Microsoft.AspNetCore.Mvc;
using GiantNationalBankClient.Models;
using Newtonsoft.Json;
using GiantNationalBankClient.Utility;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;
using GiantNationalBank.Models;
using GiantNationalBankAPI.Models;

namespace GiantNationalBankClient.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly ServiceHelper _serviceHelper;

        public UserController()
        {
            _serviceHelper = new ServiceHelper();
        }

        // User Dashboard - Shows profile information
        public async Task<IActionResult> Index()
        {
            try
            {
                string userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                var response = await _serviceHelper.GetRequest("User", $"GetUserByID?userID={userId}", true, GetAuthToken());
                var userResponse = JsonConvert.DeserializeObject<GetUserResponseModel>(response);

                if (userResponse != null && userResponse.Status)
                {
                    // Passes userResponse.user to the Index view
                    return View("Index", userResponse.user);
                }

                TempData["ErrorMessage"] = "Failed to retrieve user data.";
                return View("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Get User Data: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while retrieving user data.";
                return View("Index");
            }
        }

        // Save/Update Profile Information
        [HttpPost]
        public async Task<IActionResult> SaveProfile(User updatedUser)
        {
            try
            {
                var strSerializedData = JsonConvert.SerializeObject(updatedUser);
                string response = await _serviceHelper.PostRequest(strSerializedData, "User/SaveUserRecord", true, GetAuthToken());
                var responseModel = JsonConvert.DeserializeObject<SaveUserResponse>(response);

                if (responseModel != null && responseModel.Status)
                {
                    TempData["SuccessMessage"] = "Profile updated successfully.";
                    return RedirectToAction("Index");
                }

                TempData["ErrorMessage"] = "Failed to update profile.";
                return View("Index", updatedUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Update User API: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while updating your profile.";
                return View("Index", updatedUser);
            }
        }

        // View User Accounts
        public async Task<IActionResult> Accounts()
        {
            try
            {
                string userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                var response = await _serviceHelper.GetRequest("User", $"GetAccountByUser?userID={userId}", true, GetAuthToken());
                var accountsResponse = JsonConvert.DeserializeObject<GetAccountsResponseModel>(response);

                if (accountsResponse != null && accountsResponse.Status && accountsResponse.accountList != null)
                {
                    return View("Accounts", accountsResponse.accountList);
                }

                TempData["ErrorMessage"] = "Failed to retrieve accounts.";
                return View("Accounts", new List<Account>());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"View Accounts Error: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while retrieving your accounts.";
                return View("Accounts", new List<Account>());
            }
        }

        // Create New Account
        public IActionResult CreateAccount()
        {
            return View("CreateAccount");
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount(RegistrationModel model)
        {
            try
            {
                var strSerializedData = JsonConvert.SerializeObject(model);
                string response = await _serviceHelper.PostRequest(strSerializedData, "User/CreateNewAccount", true, GetAuthToken());
                var responseModel = JsonConvert.DeserializeObject<CreateAccountResponseModel>(response);

                if (responseModel != null && responseModel.Status)
                {
                    TempData["SuccessMessage"] = "Account created successfully.";
                    return RedirectToAction("Accounts");
                }

                TempData["ErrorMessage"] = "Failed to create account.";
                return View("CreateAccount", model);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Create Account Error: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while creating the account.";
                return View("CreateAccount", model);
            }
        }

        public async Task<IActionResult> Transactions(int accountId)
        {
            try
            {
                var response = await _serviceHelper.GetRequest("Account", $"GetTransactionsByAccount/{accountId}", true, GetAuthToken());
                var transactionsResponse = JsonConvert.DeserializeObject<GetTransactionsResponseModel>(response);

                if (transactionsResponse != null && transactionsResponse.Status && transactionsResponse.TransactionList != null)
                {
                    ViewBag.AccountId = accountId;
                    return View("Transactions", transactionsResponse.TransactionList);
                }

                TempData["ErrorMessage"] = "Failed to retrieve transactions.";
                return RedirectToAction("Accounts");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"View Transactions Error: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while retrieving transactions.";
                return RedirectToAction("Accounts");
            }
        }

        private string GetAuthToken()
        {
            return Request.Cookies["AuthToken"] ?? string.Empty;
        }
    }
}
