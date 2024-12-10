using GiantNationalBankAPI.Data;

namespace GiantNationalBankAPI.Models
{
    public class GetUserResponseModel
    {
        public bool Status { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public UserData user { get; set; }
    }
}
