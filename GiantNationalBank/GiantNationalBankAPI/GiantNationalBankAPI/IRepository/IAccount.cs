using GiantNationalBankAPI.Models;
using GiantNationalBankAPI.Data;

namespace GiantNationalBankAPI.IRepository
{
    public interface IAccount
    {
        /// <summary>
        /// Returns a list of all accounts
        /// </summary>
        /// <returns></returns>
        Task<List<AccountModel>> GetAllAccounts();

        /// <summary>
        /// Returns the account information for a specific account
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        Task<AccountModel> GetAccountByID(int accountID);

        /// <summary>
        /// Debits an account with the specified amount
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        Task<AccountModel> DebitAnAccount(int accountID, decimal amount, string name);

        /// <summary>
        /// Credits an account with the specified amount
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="amount"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<AccountModel> CreditAnAccount(int accountID, decimal amount, string name);

        /// <summary>
        /// Creates a new account for a user
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        Task<Account> CreateNewAccount(int userID);
    }
}
