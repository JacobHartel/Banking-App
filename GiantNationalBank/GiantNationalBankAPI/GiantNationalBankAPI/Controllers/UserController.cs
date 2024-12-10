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
    public class UserController : ControllerBase
    {
        // private readonly IUser repository;
        private readonly IUser repository;

        //context for the database connection
        private readonly GiantNationalBankContext context;

        //variable for holding the configuration data for login authentication
        private IConfiguration config;

        public UserController(IConfiguration Config)
        {
            config = Config;
            context = new GiantNationalBankContext(config);
            repository = new UserDAL(context, config);
            

        }
        #endregion

        #region RegisterUser
        [HttpPost("RegisterUser", Name = "RegisterUser")]
        [AllowAnonymous]
        public async Task<CreateUserResponseModel> RegisterUser(RegistrationModel userData)
        {
            CreateUserResponseModel response = new CreateUserResponseModel();

            if (userData != null)
            {
                try
                {
                    //call the method that will save the user record
                    var user = await repository.SaveUserRecord(userData).ConfigureAwait(true);

                    if (user.StatusCode == 200) //status success
                    {
                        //user has been added
                        
                        response.Status = true;
                        response.Message = "Registration Successful";
                        response.StatusCode = 200;

                        // send back the user information we just added
                        response.Data = new UserData();

                        response.Data.UserId = user.Data.UserId;
                        response.Data.FirstName = user.Data.FirstName;
                        response.Data.LastName = user.Data.LastName;
                        response.Data.Email = user.Data.Email;
                        response.Data.Phone = user.Data.Phone;
                        response.Data.Street1 = user.Data.Street1;
                        response.Data.Street2 = user.Data.Street2;
                        response.Data.City = user.Data.City;
                        response.Data.State = user.Data.State;
                        response.Data.ZipCode = user.Data.ZipCode;
                        response.Data.Phone = user.Data.Phone;
                        
                    }
                    else
                    {
                        //there has been an error
                        response.Status = false;
                        response.Message = "Registration Failed";
                        response.StatusCode = 500;
                        
                    }
                }
                catch (Exception ex)
                {
                    //there has been an error
                    response.Status = false;
                    response.Message = "Registration Failed";
                    response.StatusCode = 500;
                    Console.WriteLine(ex.Message);
                }
               

            }

            return response;
        }
        #endregion

        #region LoginUser
        [HttpPost("LoginUser", Name = "LoginUser")]
        [AllowAnonymous]
        public async Task<LoginResponse> LoginUser(LoginModel tokenData)
        {
            LoginResponse response = new LoginResponse();
            try
            {
                if(tokenData != null)
                {
                    //call the method that will check the user credentials
                    var loginResult = await repository.LoginUser(tokenData).ConfigureAwait(true);

                    if(loginResult.StatusCode == 200)
                    {
                        //login check has succeeded

                        //query the database to get the information about our logged in user
                        var user = await repository.FindByName(tokenData.Username).ConfigureAwait(true);

                        if(user != null)
                        {
                            //generate the authentication token
                            var tokenString = GenerateJwtToken(tokenData);

                            response.StatusCode = 200;
                            response.Status = true;
                            response.Message = "Login Successful";
                            response.AccountType = tokenData.UserType;
                            response.Authtoken = tokenString;
                            response.UserData = new UserData
                            {
                                UserId = user.UserId,
                                Email = user.Email,
                                FirstName = user.FirstName,
                                LastName = user.LastName,
                                Street1 = user.Street1,
                                Street2 = user.Street2,
                                City = user.City,
                                State = user.State,
                                ZipCode = user.ZipCode,
                                Phone = user.Phone
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
                Console.WriteLine("LoginUser --- " + ex.Message);
                response.StatusCode = 500;
                response.Status = false;
                response.Message = "Login Failed";
            }

            return response;
        }
        #endregion

        #region GetAccountByUser
        [HttpGet("GetAccountByUser", Name = "GetAccountByUser")]
        [AllowAnonymous]
        public async Task<GetAccountsResponseModel> GetAccountByUser(int userID)
        {
            GetAccountsResponseModel response = new GetAccountsResponseModel();

            //set up a list to hold the accounts
            List<AccountModel> accountList = new List<AccountModel>();

            try
            {
                accountList = await repository.GetAccountsByUser(userID).ConfigureAwait(true);

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

        #region SaveUserRecord
        [HttpPost("SaveUserRecord", Name = "SaveUserRecord")]
        [AllowAnonymous]
        public async Task<SaveUserResponse> SaveUserRecord(UserData userData)
        {
            SaveUserResponse response = new SaveUserResponse();

            try
            {
                var user = await repository.UpdateUserRecord(userData).ConfigureAwait(true);

                //check the list isn't empty
                if (response != null)
                {
                    response.Status = true;
                    response.StatusCode = 200;
                    // send back the user information we just added
                    response.Data = user.Data;
                }
                else
                {
                    //there has been an error
                    response.Status = false;
                    response.Message = "Save Failed";
                    response.StatusCode = 500;
                }

            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "Save Failed";
                response.StatusCode = 500;
                //there has been an error
                Console.WriteLine(ex.Message);
            }
            return response;
        }
        #endregion

        #region GetUserByID
        [HttpGet("GetUserByID", Name = "GetUserByID")]
        [AllowAnonymous]
        public async Task<GetUserResponseModel> GetUserByID(int userID)
        {
            GetUserResponseModel response = new GetUserResponseModel();

            try
            {
                response.user = await repository.GetUserByID(userID).ConfigureAwait(true);

                //check ther use is null
                if (response.user != null)
                {
                    response.Status = true;
                    response.StatusCode = 200;
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

        #region JwtToken
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