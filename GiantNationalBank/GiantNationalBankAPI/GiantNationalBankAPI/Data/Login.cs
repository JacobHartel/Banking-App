using System;
using System.Collections.Generic;

namespace GiantNationalBankAPI.Data
{
    public partial class Login
    {
        public int Id { get; set; }
        public string UserType { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;

        public virtual User Id1 { get; set; } = null!;
        public virtual Admin IdNavigation { get; set; } = null!;
    }
}
