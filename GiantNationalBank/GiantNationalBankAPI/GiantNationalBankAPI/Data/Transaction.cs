using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GiantNationalBankAPI.Data
{
    public partial class Transaction
    {
        [Key]
        public int TransactionId { get; set; }
        public int AccountId { get; set; }
        public string TransactionName { get; set; } = null!;
        public bool TransactionType { get; set; }
        public decimal Amount { get; set; }

   }
}
