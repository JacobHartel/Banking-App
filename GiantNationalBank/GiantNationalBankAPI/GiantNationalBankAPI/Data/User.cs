using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GiantNationalBankAPI.Data
{
    public partial class User
    {
        public User()
        {
            Accounts = new HashSet<Account>();
        }

        [Key]
        public int UserId { get; set; }
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Street1 { get; set; } = null!;
        public string? Street2 { get; set; }
        public string City { get; set; } = null!;
        public string State { get; set; } = null!;
        public int ZipCode { get; set; }
        public string Phone { get; set; } = null!;

        public virtual Login Login { get; set; } = null!;
        public virtual ICollection<Account> Accounts { get; set; }
    }
}
