using GiantNationalBankAPI.Data;

namespace GiantNationalBankAPI.Models
{
    public class AccountModel
    {
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public decimal Balance { get; set; }

        public UserData User { get; set; } = null!;

        public List<Transaction> transactionList { get; set; }
    }
}
