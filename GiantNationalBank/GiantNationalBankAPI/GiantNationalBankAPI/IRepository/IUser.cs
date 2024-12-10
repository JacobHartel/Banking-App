using GiantNationalBankAPI.Models;
using GiantNationalBankAPI.Data;


namespace GiantNationalBankAPI.IRepository
{
    public interface IUser
    {


        /// <summary>
        /// Saves a new user who has registered
        /// </summary>
        /// <param name="usermodel"></param>
        /// <returns></returns>
        Task<SaveUserResponse> SaveUserRecord(RegistrationModel usermodel);


        /// <summary>
        /// Logs in an already registered user
        /// </summary>
        /// <param name="tokenData"></param>
        /// <returns></returns>
        Task<LoginResponse> LoginUser(LoginModel tokenData);

        /// <summary>
        /// Finds the user data for a user based on username
        /// </summary>
        /// <param name="tokenData"></param>
        /// <returns></returns>
        Task<UserData> FindByName(string username);


        /// <summary>
        /// Gets a list of all accounts by user
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        Task<List<AccountModel>> GetAccountsByUser(int userID);

        /// <summary>
        /// Returns the user information 
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        Task<UserData> GetUserByID(int userID);

        /// <summary>
        /// Updates an existing user
        /// </summary>
        /// <param name="usermodel"></param>
        /// <returns></returns>
        Task<SaveUserResponse> UpdateUserRecord(UserData usermodel);
    }
}
