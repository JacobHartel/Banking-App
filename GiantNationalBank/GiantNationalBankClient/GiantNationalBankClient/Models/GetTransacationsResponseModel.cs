using System.Transactions;

public class GetTransactionsResponseModel
{
    public bool Status { get; set; }
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public List<Transaction> TransactionList { get; set; }
}