using GiantNationalBankAPI.Data;

namespace GiantNationalBankAPI.Models
{
    public class CreateUserResponseModel
    {
        public bool Status { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public UserData Data { get; set; }
    }
}
