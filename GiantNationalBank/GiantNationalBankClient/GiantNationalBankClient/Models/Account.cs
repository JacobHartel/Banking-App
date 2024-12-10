using System.Transactions;

namespace GiantNationalBankClient.Models
{
    public class Account
    {

        public int AccountID { get; set; }
        public int UserID { get; set; }
        public decimal Balance { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }

    }
}
