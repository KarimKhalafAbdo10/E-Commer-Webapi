using E_Commerce.Application.common;
using E_Commerce.Application.DTOs.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Contracts
{
    public interface IIdentityServices
    {
        Task<Result<IdentityUserResult>> FindUserByEmailAsync(string email , CancellationToken ct = default);
        Task<Result<bool>> CheckPasswordAsync(string email, string password, CancellationToken ct = default);
        Task<Result<IdentityUserResult>> CreateUserAsync(RegisterDto registerDto, CancellationToken ct = default!);
        Task<Result<IReadOnlyList<string>>> GetUserRoles(string email,CancellationToken ct=default);
        Task<Result<bool>> CheckEmailExistsAsync(string? email, CancellationToken ct = default);
        Task<Result<AddressDto>> GetCurrentUserAddress(string email, CancellationToken ct =default);
        Task<Result<AddressDto>> UpdateOrInsertUserAddress(string email,AddressDto address ,CancellationToken ct = default);
        Task<Result<bool>> ChangeUserPassword(string email, ChangePasswordDto passwordDto, CancellationToken ct = default);
        Task<Result<string>> ForgotPasswordAsync(ForgetPassWordDto dto, CancellationToken ct=default);

    }
}
