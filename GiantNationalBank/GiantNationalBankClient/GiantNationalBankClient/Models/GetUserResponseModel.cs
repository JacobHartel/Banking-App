
namespace GiantNationalBankClient.Models
{
    public class GetUserResponseModel
    {
        public bool Status { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public User user { get; set; }
        public List<User> User { get; internal set; }
    }
}
