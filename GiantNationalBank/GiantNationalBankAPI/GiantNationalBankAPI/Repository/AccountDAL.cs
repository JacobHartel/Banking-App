using GiantNationalBankAPI.Data;
using GiantNationalBankAPI.IRepository;
using GiantNationalBankAPI.Models;
using System.Security.Principal;

namespace GiantNationalBankAPI.Repository
{
    public class AccountDAL : IAccount
    {
        #region Constructor
        //context for the database connection
        private readonly GiantNationalBankContext context;

        private readonly IConfiguration _config;

        public AccountDAL(GiantNationalBankContext Context)
        {
            context = Context;
        }

        public AccountDAL(GiantNationalBankContext Context, IConfiguration config)
        {
            context = Context;
            _config = config;
        }
        #endregion

        #region Get All Accounts
        public async Task<List<AccountModel>> GetAllAccounts()
        {
            //set up a list to hold the accounts
            List<AccountModel> accountList = new List<AccountModel>();

            try
            {
                //query the database
                var accounts = context.Accounts.ToList();

                if (accounts != null)
                {
                    foreach (var account in accounts)
                    {
                        //get all of the transactions that belong to this account as a list
                        List<Transaction> transactionList = context.Transactions.Where(x => x.AccountId == account.AccountId).ToList();

                        //get the user that owns this account
                        User user = context.Users.Where(x => x.UserId == account.UserId).FirstOrDefault<User>();

                        accountList.Add(new AccountModel()
                        {
                            AccountId = account.AccountId,
                            UserId = account.UserId,
                            Balance = account.Balance,
                            transactionList = transactionList,
                            User = new UserData
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
                            }

                        });
                    }

                    if (accountList.Count == 0)
                    {
                        return null;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("GetAllAccounts --- " + ex.Message);
                throw;
            }

            return accountList;
        }

        #endregion

        #region Get Account By ID
        public async Task<AccountModel> GetAccountByID(int accountID)
        {
            AccountModel accountData = null;

            try
            {
                //query the database
                var account = context.Accounts.Where(x => x.AccountId == accountID).FirstOrDefault<Account>();

                //get the user that owns this account
                User user = context.Users.Where(x => x.UserId == account.UserId).FirstOrDefault<User>();

                if (account != null)
                {
                    
                    //get all of the transactions that belong to this account as a list
                    List<Transaction> transactionList = context.Transactions.Where(x => x.AccountId == account.AccountId).ToList();

                    accountData = new AccountModel
                    {
                        AccountId = account.AccountId,
                        UserId = account.UserId,
                        Balance = account.Balance,
                        transactionList = transactionList,
                        User = new UserData
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
                        }

                    };
                  
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("GetAllAccounts --- " + ex.Message);
                throw;
            }

            return accountData;
        }

        #endregion

        #region Debit an Account
        public async Task<AccountModel> DebitAnAccount(int accountID, decimal amount, string name)
        {
            AccountModel accountData = null;

            try
            {
                //create the new transaction
                Transaction newDebit = new Transaction
                {
                    AccountId = accountID,
                    TransactionName = name,
                    TransactionType = false,
                    Amount = amount
                };

                //get the associated account
                var account = context.Accounts.Where(x => x.AccountId == accountID).FirstOrDefault<Account>();

                if(account != null)
                {
                    //this is a debit transaction so subtract the incoming amount from the current account balance
                    //we will not let users have a negative balance
                    if (account.Balance - amount < 0 )
                    {
                        return accountData;
                    }
                    else
                    {
                        //proceed with the transaction
                        decimal newBalance = account.Balance - amount;

                        //update the account object
                        account.Balance = newBalance;

                        //add the transaction to the database and update the account
                        using (var db = new GiantNationalBankContext(_config))
                        {
                            db.Transactions.Add(newDebit);
                            db.Accounts.Update(account);
                            db.SaveChanges();
                        }

                        //create the object we are sending back
                        accountData = new AccountModel
                        {
                            AccountId = account.AccountId,
                            UserId = account.UserId,
                            Balance = newBalance,
                            transactionList = context.Transactions.Where(x => x.AccountId == accountID).ToList()
                        };
                    }
                        
                }
               
            }
            catch (Exception ex)
            {
                Console.WriteLine("DebitAnAccount --- " + ex.Message);
                throw;
            }

            return accountData;
        }

        #endregion

        #region Credit an Account
        public async Task<AccountModel> CreditAnAccount(int accountID, decimal amount, string name)
        {
            AccountModel accountData = null;

            try
            {
                //create the new transaction
                Transaction newCredit = new Transaction
                {
                    AccountId = accountID,
                    TransactionName = name,
                    TransactionType = true,
                    Amount = amount
                };

                //Get the associated account
                var account = context.Accounts.Where(x => x.AccountId == accountID).FirstOrDefault<Account>();

                if(account != null)
                {
                    
                    //this is a credit transaction so add the incoming amount to the current account balance
                    decimal newBalance = account.Balance + amount;

                    //update the account object
                    account.Balance = newBalance;

                    //add the transaction to the database and update the account
                    using (var db = new GiantNationalBankContext(_config))
                    {
                        db.Transactions.Add(newCredit);
                        db.Accounts.Update(account);
                        db.SaveChanges();
                    }


                    //create the object we are sending back
                    accountData = new AccountModel
                    {
                        AccountId = account.AccountId,
                        UserId = account.UserId,
                        Balance = newBalance,
                        transactionList = context.Transactions.Where(x => x.AccountId == accountID).ToList()
                    };
                }  

            }
            catch (Exception ex)
            {
                Console.WriteLine("CreditAnAccount --- " + ex.Message);
                throw;
            }

            return accountData;
        }

        #endregion

        #region Create an Account
        public async Task<Account> CreateNewAccount(int userID)
        {
            Account newAccount = null;

            try
            {
                //get the user that owns this account
                User user = context.Users.Where(x => x.UserId == userID).FirstOrDefault<User>();

                if(user != null)
                {
                    //create a new account object
                    newAccount = new Account
                    {
                        UserId = userID,
                        Balance = 0
                    };

                    //add the account to the database 
                    using (var db = new GiantNationalBankContext(_config))
                    {
                        db.Accounts.Add(newAccount);
                        db.SaveChanges();
                    }

                    //attach the user to the new account object so we can have it in the response
                    newAccount.User = user;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("CreateNewAccount --- " + ex.Message);
                throw;
            }

            return newAccount;
        }
        #endregion
    }
}
