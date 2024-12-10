using Microsoft.AspNetCore.Mvc;
using GiantNationalBankAPI.Data;
using GiantNationalBankAPI.IRepository;
using GiantNationalBankAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace GiantNationalBankAPI.Controllers
{
    #region Constructor
    [ApiController]
    [Route("[controller]")]
    [AllowAnonymous]
    public class AdminController : ControllerBase
    {
        // private readonly IUser repository;
        private readonly IAdmin repository;

        //context for the database connection
        private readonly GiantNationalBankContext context;

        //variable for holding the configuration data for login authentication
        private IConfiguration config;

        public AdminController(IConfiguration Config)
        {
            config = Config;
            context = new GiantNationalBankContext(config);
            repository = new AdminDAL(context);


        }
        #endregion

        #region Login Admin
        /// <summary>
        /// Method to log in an admin for authentication
        /// </summary>
        /// <param name="tokenData"></param>
        /// <returns></returns>
        [HttpPost("LoginAdmin", Name = "LoginAdmin")]
        [AllowAnonymous]
        public async Task<LoginResponse> LoginUser(LoginModel tokenData)
        {
            LoginResponse response = new LoginResponse();
            try
            {
                if (tokenData != null)
                {
                    //call the method that will check the user credentials
                    var loginResult = await repository.LoginAdmin(tokenData).ConfigureAwait(true);

                    if (loginResult.StatusCode == 200)
                    {
                        //login check has succeeded

                        //query the database to get the information about our logged in user
                        var admin = await repository.FindByName(tokenData.Username).ConfigureAwait(true);

                        if (admin != null)
                        {
                            //generate the authentication token
                            var tokenString = GenerateJwtToken(tokenData);

                            response.StatusCode = 200;
                            response.Status = true;
                            response.Message = "Login Successful";
                            response.AccountType = tokenData.UserType;
                            response.Authtoken = tokenString;
                            response.AdminData = new AdminData
                            {

                                AdminID = admin.AdminId,
                                FirstName = admin.FirstName,
                                LastName = admin.LastName,
                                Email = admin.Email
                            };

                            return response;
                        }
                    }
                    else
                    {
                        response.StatusCode = 500;
                        response.Status = false;
                        response.Message = "Login Failed";
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("LoginAdmin --- " + ex.Message);
                response.StatusCode = 500;
                response.Status = false;
                response.Message = "Login Failed";
            }

            return response;
        }
        #endregion

        #region GetUsers
        [HttpGet("GetUsers", Name = "GetUsers")]
        [AllowAnonymous]
        public async Task<GetUsersResponseModel> GetAllUsers()
        {
            GetUsersResponseModel response = new GetUsersResponseModel();

            //set up a list to hold the incoming users we will get from the db
            List<User> userList = new List<User>();
            try
            {
                userList = repository.GetAllUsers();

                //check the list isn't empty
                if(userList.Count != 0)
                {
                    response.Status = true;
                    response.StatusCode = 200;
                    response.userList = userList;
                }
                else
                {
                    //there has been an error
                    response.Status = false;
                    response.Message = "Get Failed";
                    response.StatusCode = 0;
                }

            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "Get Failed";
                response.StatusCode = 0;
                //there has been an error
                Console.WriteLine(ex.Message);
            }
            return response;
        }
        #endregion

        #region Generate JWT Token
        /// <summary>
        /// generate the token for registration
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        private string GenerateJwtToken(LoginModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenAuthentication:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.Username),
                //new Claim(JwtRegisteredClaimNames., userInfo.Password),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            DateTime expiredDate = DateTime.UtcNow.AddDays(15);
            var token = new JwtSecurityToken(config["TokenAuthentication:Issuer"],
                config["TokenAuthentication:Issuer"],
                claims,
                expires: expiredDate,
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion
    }
}
