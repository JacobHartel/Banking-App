using GiantNationalBankAPI.Data;
using GiantNationalBankAPI.IRepository;
using GiantNationalBankAPI.Models;
using GiantNationalBankAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GiantNationalBankAPI.Controllers
{
    public class AccountController : ControllerBase
    {
        #region Constructor
        // private readonly IUser repository;
        private readonly IAccount repository;

        //context for the database connection
        private readonly GiantNationalBankContext context;

        //variable for holding the configuration data for login authentication
        private IConfiguration config;

        public AccountController(IConfiguration Config)
        {
            config = Config;
            context = new GiantNationalBankContext(config);
            repository = new AccountDAL(context, config);


        }
        #endregion

        #region GetAllAccountData
        [HttpGet("GetAllAccountData", Name = "GetAllAccountData")]
        [AllowAnonymous]
        public async Task<GetAccountsResponseModel> GetAllAccountData()
        {
            GetAccountsResponseModel response = new GetAccountsResponseModel();

            //set up a list to hold the accounts
            List<AccountModel> accountList = new List<AccountModel>();

            try
            {
                accountList = await repository.GetAllAccounts().ConfigureAwait(true);

                //check the list isn't empty
                if (accountList.Count != 0)
                {
                    response.Status = true;
                    response.StatusCode = 200;
                    response.accountList = accountList;
                }
                else
                {
                    //there has been an error
                    response.Status = false;
                    response.Message = "Get Failed";
                    response.StatusCode = 500;
                }

            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "Get Failed";
                response.StatusCode = 500;
                //there has been an error
                Console.WriteLine(ex.Message);
            }
            return response;
        }
        #endregion

        #region GetAccountByID
        [HttpGet("GetAccountByID", Name = "GetAccountByID")]
        [AllowAnonymous]
        public async Task<GetAccountResponseModel> GetAccountByID(int accountID)
        {
            GetAccountResponseModel response = new GetAccountResponseModel();

            //holds the account information
            AccountModel accountData = null;

            try
            {
                accountData = await repository.GetAccountByID(accountID).ConfigureAwait(true);

                //check the object isn't null
                if (accountData != null)
                {
                    response.Status = true;
                    response.StatusCode = 200;
                    response.account = accountData;
                }
                else
                {
                    //there has been an error
                    response.Status = false;
                    response.Message = "Get Failed";
                    response.StatusCode = 500;
                }

            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "Get Failed";
                response.StatusCode = 500;
                //there has been an error
                Console.WriteLine(ex.Message);
            }
            return response;
        }
        #endregion

        #region DebitTransaction
        [HttpPost("DebitTransaction", Name = "DebitTransaction")]
        [AllowAnonymous]
        public async Task<TransactionResponseModel> DebitTransaction(int accountID, decimal amount, string name)
        {
            TransactionResponseModel response = new TransactionResponseModel();

            //holds the account information
            AccountModel accountData = null;

            try
            {
                accountData = await repository.DebitAnAccount(accountID, amount, name).ConfigureAwait(true);

                //check the object isn't null
                if (accountData != null)
                {
                    response.Status = true;
                    response.StatusCode = 200;
                    response.account = accountData;
                }
                else
                {
                    //there has been an error
                    response.Status = false;
                    response.Message = "Transaction Failed";
                    response.StatusCode = 500;
                }

            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "Transaction Failed";
                response.StatusCode = 500;
                //there has been an error
                Console.WriteLine(ex.Message);
            }
            return response;
        }
        #endregion

        #region CreditTransaction
        [HttpPost("CreditTransaction", Name = "CreditTransaction")]
        [AllowAnonymous]
        public async Task<TransactionResponseModel> CreditTransaction(int accountID, decimal amount, string name)
        {
            TransactionResponseModel response = new TransactionResponseModel();

            //holds the account information
            AccountModel accountData = null;

            try
            {
                accountData = await repository.CreditAnAccount(accountID, amount, name).ConfigureAwait(true);

                //check the object isn't null
                if (accountData != null)
                {
                    response.Status = true;
                    response.StatusCode = 200;
                    response.account = accountData;
                }
                else
                {
                    //there has been an error
                    response.Status = false;
                    response.Message = "Transaction Failed";
                    response.StatusCode = 0;
                }

            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "Transaction Failed";
                response.StatusCode = 0;
                //there has been an error
                Console.WriteLine(ex.Message);
            }
            return response;
        }
        #endregion

        #region CreateNewAccount
        [HttpPost("CreateNewAccount", Name = "CreateNewAccount")]
        [AllowAnonymous]
        public async Task<CreateAccountResponseModel> CreateNewAccount(int userID)
        {
            CreateAccountResponseModel response = new CreateAccountResponseModel();

            //holds the account information
            Account accountData = null;

            try
            {
                accountData = await repository.CreateNewAccount(userID).ConfigureAwait(true);

                //check the object isn't null
                if (accountData != null)
                {
                    response.Status = true;
                    response.StatusCode = 200;
                    response.account = accountData;
                }
                else
                {
                    //there has been an error
                    response.Status = false;
                    response.Message = "Create Failed";
                    response.StatusCode = 500;
                }

            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "Create Failed";
                response.StatusCode = 500;
                //there has been an error
                Console.WriteLine(ex.Message);
            }
            return response;
        }
        #endregion
    }
}
