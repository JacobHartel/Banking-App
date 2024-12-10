using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GiantNationalBankAPI.Data
{
    public partial class Account
    {
        public Account()
        {
            Transactions = new HashSet<Transaction>();
        }

        [Key]
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public decimal Balance { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
