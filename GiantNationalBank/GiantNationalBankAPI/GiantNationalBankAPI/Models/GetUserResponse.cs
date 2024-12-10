using GiantNationalBankAPI.Data;

namespace GiantNationalBankAPI.Models
{
    public class GetUserResponse
    {
        public int StatusCode { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public List<User> Data { get; set; }

    }

}
