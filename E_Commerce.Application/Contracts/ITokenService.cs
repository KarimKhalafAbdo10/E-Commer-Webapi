using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Contracts
{
    public interface ITokenService
    {

        string GetToken(string userId, string  Email,String UserName,IReadOnlyList<string> roles);
    }
}
