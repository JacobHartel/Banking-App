namespace GiantNationalBankClient.Models
{
    public class Transactions
    {
        public int TransactionID { get; set; }
        public int AccountID { get; set; }
        public bool? TransactionName { get; set; }
        public decimal Amount { get; set; }
    }
}
