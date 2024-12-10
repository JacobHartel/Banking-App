using GiantNationalBankAPI.Data;

namespace GiantNationalBankAPI.Models
{
    public class CreateAccountResponseModel
    {
        public bool Status { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public Account account { get; set; }
    }
}
