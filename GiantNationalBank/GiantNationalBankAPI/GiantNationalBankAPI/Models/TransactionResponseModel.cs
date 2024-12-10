using GiantNationalBankAPI.Data;

namespace GiantNationalBankAPI.Models
{
    public class TransactionResponseModel
    {
        public bool Status { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } 
        public AccountModel account { get; set; }
    }
}
