using E_Commerce.Application.common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Servicies
{
    internal class AuthenticationService : IAuthenticationService
    {
        private readonly IIdentityServices _identityServices;
        private readonly ITokenService _tokenService;

        public AuthenticationService(IIdentityServices identityServices, ITokenService tokenService)
        {
            _identityServices = identityServices;
            _tokenService = tokenService;
        }

        public Task<Result<bool>> ChangePassword(ChangePasswordDto passwordDto, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> CheckEmailExistsAsync(string? email, CancellationToken ct = default)
      => _identityServices.CheckEmailExistsAsync(email, ct);

        public async Task<Result<UserDto>> GetCurrentUser(string email, CancellationToken ct = default)
        {
            var user = await _identityServices.FindUserByEmailAsync(email, ct);
            if (!user.IsSuccess)
                return Result<UserDto>.Fail(user.Errors);

            var userData = user.data;
            var roles = await _identityServices.GetUserRoles(email, ct);

            // If getting roles fails, use empty list

            var token = _tokenService.GetToken(userData.Id, userData.Email, userData.DisplayName,roles.data);

            return Result<UserDto>.Ok(new UserDto() 
            { 
                DisplayName = userData.DisplayName,  
                Email = userData.Email, 
                Token = token 
            });
        }

        public Task<Result<AddressDto>> GetCurrentUserAddress(string email, CancellationToken ct = default)
       =>_identityServices.GetCurrentUserAddress(email, ct);

        

        public async Task<Result<UserDto>> LogInAsync(LogInDto logInDto, CancellationToken ct = default)
        {
            var userResult = await _identityServices.FindUserByEmailAsync(logInDto.Email, ct);
            if (!userResult.IsSuccess)
                return Result<UserDto>.Fail(userResult.Errors);

            var passwordResult = await _identityServices.CheckPasswordAsync(logInDto.Email, logInDto.Password, ct);
            if (!passwordResult.IsSuccess)
                return Result<UserDto>.Fail(passwordResult.Errors);
            if(!passwordResult.data)
                return Result<UserDto>.Fail(Error.Unauthorized("Invalid password"));

            var userData = userResult.data;
            var userRoles = await _identityServices.GetUserRoles(userData.Email, ct);

            // If getting roles fails, use empty list
            var roles = userRoles.IsSuccess ? userRoles.data : new List<string>();

            var token = _tokenService.GetToken(userData.Id, userData.Email, userData.DisplayName, roles);
            return Result<UserDto>.Ok(new UserDto() 
            {
                Email = logInDto.Email,
                DisplayName = userResult.data.DisplayName,
                Token = token
            });

        }

        public async Task<Result<UserDto>> RegisterAsync(RegisterDto registerDto, CancellationToken ct = default)
        {
            var user = await _identityServices.CreateUserAsync(registerDto, ct);
            if (!user.IsSuccess)
                return Result<UserDto>.Fail(user.Errors);

            var userData = user.data;
            var userRoles = await _identityServices.GetUserRoles(userData.Email, ct);

            // During registration, if getting roles fails, use empty list as new users typically have no roles
            var roles = userRoles.IsSuccess ? userRoles.data : new List<string>();

            var token = _tokenService.GetToken(userData.Id, userData.Email, userData.DisplayName, roles);

            return Result<UserDto>.Ok(new UserDto()
            {
                Email = user.data.Email,
                DisplayName = user.data.DisplayName,
                Token = token
            });
        }

        public Task<Result<AddressDto>> UpSertUserAddress(string email, AddressDto address, CancellationToken ct = default)
      => _identityServices.UpdateOrInsertUserAddress(email, address, ct);

        public Task<Result<bool>> ChangePassword(string email, ChangePasswordDto passwordDto, CancellationToken ct = default)
      => _identityServices.ChangeUserPassword(email, passwordDto, ct);

        public Task<Result<string>> ForgotPasswordAsync(ForgetPassWordDto dto, CancellationToken ct = default)
       => _identityServices.ForgotPasswordAsync(dto,ct);
    }
}
