using E_Commerce.Application.common;
using E_Commerce.Application.DTOs.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Contracts
{
    public interface IAuthenticationService
    {

        Task<Result<UserDto>> LogInAsync(LogInDto logInDto ,CancellationToken ct=default);
        Task<Result<UserDto>> RegisterAsync(RegisterDto registerDto, CancellationToken ct = default);
        Task<Result<bool>> CheckEmailExistsAsync(string? email, CancellationToken ct = default);
        Task<Result<UserDto>> GetCurrentUser(string email,CancellationToken ct =default);

        Task<Result<AddressDto>> GetCurrentUserAddress(string email, CancellationToken ct=default);
        Task<Result<AddressDto>> UpSertUserAddress( string email ,AddressDto address, CancellationToken ct =default);
        Task<Result<bool>>  ChangePassword(string email,ChangePasswordDto passwordDto, CancellationToken ct = default);
        Task<Result<string>> ForgotPasswordAsync(ForgetPassWordDto dto, CancellationToken ct = default);

    }
} 
