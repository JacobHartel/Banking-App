using GiantNationalBankAPI.Data;

namespace GiantNationalBankAPI.Models
{
    public class GetAccountsResponseModel
    {
        public bool Status { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } 
        public List<AccountModel> accountList { get; set; }
    }
}
