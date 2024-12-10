using GiantNationalBankAPI.IRepository;
using GiantNationalBankAPI.Models;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using System.Text;

namespace GiantNationalBankAPI.Data
{
    
    public class AdminDAL : IAdmin
    {
        #region Constructor
        //context for the database connection
        private readonly GiantNationalBankContext context;

        public AdminDAL(GiantNationalBankContext Context)
        {
            context = Context;
        }
        #endregion

        #region Login Admin Method
        public async Task<LoginResponse> LoginAdmin(LoginModel tokenData)
        {
            LoginResponse res = new LoginResponse();
            try
            {
                if(tokenData != null)
                {
                    //look for the admin in the database
                    var query = context.Admins
                    .Where(x => x.Login.UserName == tokenData.Username && x.Login.Password == tokenData.Password)
                    .FirstOrDefault<Admin>();

                    //if query has a result then we have a match
                    if(query != null)
                    {
                        res.Status = true;
                        res.StatusCode = 200;
                        res.Message = "Login Success";
                        return res;
                    }
                    else
                    {
                        //the user wasn't found or wasn't a match
                        res.Status = false;
                        res.StatusCode = 500;
                        res.Message = "Login Failed";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("LoginAdmin --- " + ex.Message);
                res.Status = false;
                res.StatusCode = 500;
            }

            return res;
        }
        #endregion

        #region Find Admin By AdminID Method
        public async Task<Admin> FindByID(int adminID)
        {
            Admin admin = null;

            try
            {
                if (adminID != null)
                {
                    //query the database to find the agent who has this username
                    var query = context.Admins
                        .Where(x => x.Login.Id == adminID)
                        .FirstOrDefault<Admin>();

                    if (query != null)
                    {
                        //set up the object so we can return it
                        admin = new Admin
                        {
                            AdminId = query.AdminId,
                            FirstName = query.FirstName,
                            LastName = query.LastName,
                            Email = query.Email

                        };
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("FindByID --- " + ex.Message);
            }

            return admin;
        }
        #endregion

        #region Find Admin By Name Method

        public async Task<Admin> FindByName(string username)
        {
            Admin admin = null;

            try
            {
                if (username != null)
                {
                    //query the database to find the agent who has this username
                    var query = context.Admins
                        .Where(x => x.Login.UserName == username)
                        .FirstOrDefault<Admin>();

                    if (query != null)
                    {
                        //set up the object so we can return it
                        admin = new Admin
                        {
                            AdminId = query.AdminId,
                            FirstName = query.FirstName,
                            LastName = query.LastName,
                            Email = query.Email

                        };
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("FindByName --- " + ex.Message);
            }

            return admin;
        }
        #endregion

        #region Get All Users Method
        /// <summary>
        /// Method that retrieves all users in the database
        /// </summary>
        /// <returns></returns>
        public List<User> GetAllUsers()
        {
            List<User> userList = new List<User>();

            try
            {
                //query the database to get all of the users
                var users = context.Users.ToList();

                foreach (var user in users)
                {
                    userList.Add(new User()
                    {
                        UserId = user.UserId,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Street1 = user.Street1,
                        Street2 = user.Street2,
                        City = user.City,
                        State = user.State,
                        ZipCode = user.ZipCode,
                        Phone = user.Phone

                    });
                }

                if (userList.Count == 0)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetAllUsers --- " + ex.Message);
                throw;
            }

            return userList;
        }

        #endregion
    }
}
