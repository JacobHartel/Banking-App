using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GiantNationalBankAPI.Data
{
    public partial class Admin
    {
        [Key]
        public int AdminId { get; set; }
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        public virtual Login Login { get; set; } = null!;
    }
}
