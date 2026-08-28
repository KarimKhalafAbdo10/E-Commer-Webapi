using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.common
{
    public class IdentityUserResult
    {
        public IdentityUserResult(string id, string displayName, string userName, string email)
        {
            Id = id;
            DisplayName = displayName;
            UserName = userName;
            Email = email;
        }

        public string Id { get; set; } = default!;
        public string DisplayName { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
    }
}
