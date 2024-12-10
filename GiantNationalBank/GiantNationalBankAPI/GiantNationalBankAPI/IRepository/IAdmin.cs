using GiantNationalBankAPI.Models;
using GiantNationalBankAPI.Data;

namespace GiantNationalBankAPI.IRepository
{
    public interface IAdmin
    {
        /// <summary>
        /// Logs in an admin
        /// </summary>
        /// <param name="tokenData"></param>
        /// <returns></returns>
        Task<LoginResponse> LoginAdmin(LoginModel tokenData);


        /// <summary>
        /// Finds the admin based on adminID
        /// </summary>
        /// <param name="agentID"></param>
        /// <returns></returns>
        Task<Admin> FindByID(int agentID);

        /// <summary>
        /// Find the admin based on username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<Admin> FindByName(string username);

        /// <summary>
        /// Returns a list of all registered users
        /// </summary>
        /// <returns></returns>
        List<User> GetAllUsers();

    }
}
