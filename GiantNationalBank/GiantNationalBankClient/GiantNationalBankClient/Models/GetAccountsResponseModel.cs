using GiantNationalBankClient.Models;

namespace GiantNationalBankAPI.Models
{
    public class GetAccountsResponseModel
    {
        public bool Status { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public List<Account> accountList { get; set; }
    }
}