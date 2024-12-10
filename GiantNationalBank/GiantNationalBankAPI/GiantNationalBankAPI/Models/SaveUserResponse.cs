using GiantNationalBankAPI.Data;

namespace GiantNationalBankAPI.Models
{
    public class SaveUserResponse
    {
        public int StatusCode { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public User Data { get; set; }
        public int UserId { get; set; }

    }

}
