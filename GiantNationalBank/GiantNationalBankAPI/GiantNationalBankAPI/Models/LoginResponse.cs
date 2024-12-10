namespace GiantNationalBankAPI.Models
{
    public class LoginResponse
    {
        public int UserID { get; set; }
        public string AccountType { get; set; } = null;
        public bool Status { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = null;
        public string Authtoken { get; set; } = null;

        public UserData UserData { get; set; }

        public AdminData AdminData { get; set; }

    }
}
