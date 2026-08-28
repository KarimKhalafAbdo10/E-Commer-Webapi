using E_Commerce.Application.common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Identity;
using E_Commerce.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration.Internal;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Services
{
    internal class Identityservice : IIdentityServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISendEmailService _sendEmailService;
        private readonly IConfiguration _config;

        public Identityservice(UserManager<ApplicationUser> userManager,ISendEmailService sendEmailService,IConfiguration config)
        {
            _userManager = userManager;
            _sendEmailService = sendEmailService;
            _config = config;
        }

        public async Task<Result<bool>> ChangeUserPassword(string email, ChangePasswordDto passwordDto, CancellationToken ct = default)
        {
            // Normalize email to lowercase for case-insensitive lookup
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
                return Result<bool>.Fail(Error.NotFound($"User With {email} Is Not Found"));

            var result = await _userManager.ChangePasswordAsync(user, passwordDto.CurrentPassword, passwordDto.NewPassword);
            if (!result.Succeeded)
                return Result<bool>.Fail(Error.Failure("Failure", string.Join(";", result.Errors.Select(e => e.Description))));

            return Result<bool>.Ok(true);
        }

        public async Task<Result<bool>> CheckEmailExistsAsync(string? email, CancellationToken ct = default)
        {
            // Validate email is not empty
            if (string.IsNullOrWhiteSpace(email))
                return Result<bool>.Fail(new Error("InvalidEmail", "Email cannot be empty"));

            // Normalize email to lowercase for case-insensitive lookup
            
            return Result<bool>.Ok(await _userManager.FindByEmailAsync(email) is not null);
        }

        

        public async Task<Result<bool>> CheckPasswordAsync(string email, string password, CancellationToken ct = default)
        {
            // Normalize email to lowercase for case-insensitive lookup
            //var normalizedEmail = email?.ToLower() ?? string.Empty;
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return Result<bool>.Fail(Error.NotFound($"User With {email} Is Not Found"));
            else
                return Result<bool>.Ok(await _userManager.CheckPasswordAsync(user, password));
        }

        public async Task<Result<IdentityUserResult>> CreateUserAsync(RegisterDto registerDto, CancellationToken ct = default)
        {
            var user = new ApplicationUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,  // Normalize email to lowercase
                UserName = registerDto.UserName,
                PhoneNumber = registerDto.PhoneNumber
            };

            var userResult = await _userManager.CreateAsync(user, registerDto.Password);

            if (!userResult.Succeeded)
            {
                var errors = userResult.Errors.Select(e => new Error(e.Code, e.Description)).ToList();
                return Result<IdentityUserResult>.Fail(errors);
            }

            return Result<IdentityUserResult>.Ok(new IdentityUserResult(user.Id, user.DisplayName, user.UserName, user.Email));
        }

        public async Task<Result<IdentityUserResult>> FindUserByEmailAsync(string email, CancellationToken ct = default)
        {
            // Normalize email to lowercase for case-insensitive lookup
            //var normalizedEmail = email?.ToLower() ?? string.Empty;
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return Result<IdentityUserResult>.Fail(Error.NotFound($"User With {email} Is Not Found"));
            else
                return Result<IdentityUserResult>.Ok(new IdentityUserResult(user.Id, user.DisplayName, user.UserName!, user.Email!));
        }

        public async Task<Result<string>> ForgotPasswordAsync(ForgetPassWordDto dto, CancellationToken ct = default)
        {
            var user =await _userManager.FindByEmailAsync(dto.Email);
            // Return success even if user doesn't exist to prevent email enumeration attacks
            if (user == null)
            {
                return Result<string>.Ok("If your email exists in our system, a password reset link has been sent.");
            }
            var token =await _userManager.GeneratePasswordResetTokenAsync(user);
            string resetLink;
            if (dto.ClientType.ToLower() == "mobile")
            {
                var mobileAppUrl = _config["ClientSettings:MobileDeepLink"];
                resetLink = $"{mobileAppUrl}?email={Uri.EscapeDataString(user.Email!)}&token={Uri.EscapeDataString(token)}";
            }
            else
            {
                var webUrl = _config["ClientSettings:WebBaseUrl"];
                resetLink = $"{webUrl}/reset-password?email={Uri.EscapeDataString(user.Email!)}&token={Uri.EscapeDataString(token)}";
            }
            var emailBody = $@"
            <div style='font-family: Arial, sans-serif; padding: 20px;'>
                <h2>Reset Your Password</h2>
                <p>We received a request to reset your password. Click the button below to proceed:</p>
                <a href='{resetLink}' style='background-color: #007bff; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px; display: inline-block;'>Reset Password</a>
                <p style='margin-top: 15px; color: #666;'>If you did not request this, please ignore this email.</p>
            </div>";

            await _sendEmailService.SendEmailAsync(user.Email!, "Reset Your Password", emailBody, ct);

            return Result<string>.Ok("If your email exists in our system, a password reset link has been sent.");
        }

        public async Task<Result<AddressDto>> GetCurrentUserAddress(string email, CancellationToken ct = default)
        {

          var user= await _userManager.Users.Include(x => x.Address).FirstOrDefaultAsync(x => x.Email == email);

            if (user?.Address is null)
                return Result<AddressDto>.Fail(Error.NotFound("Address Not Found", $" Address Of User With Email {email} Not Found"));
            var address = user.Address;
            return Result<AddressDto>.Ok(

                new AddressDto()
                {
                     City = address.City,
                     Street = address.Street,
                     Country = address.Country,
                     FirstName = address.FirstName,
                     LastName= address.LastName,
                }
                
                );
                 
                
                
                

        }

        public async Task<Result<IReadOnlyList<string>>> GetUserRoles(string email, CancellationToken ct = default)
        {
            // Normalize email to lowercase for case-insensitive lookup
            //var normalizedEmail = email?.ToLower() ?? string.Empty;
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) 
                return Result<IReadOnlyList<string>>.Fail(Error.NotFound($"User Not Found", $"User with email {email} not found"));

            var roles = await _userManager.GetRolesAsync(user);
            return Result<IReadOnlyList<string>>.Ok(roles.ToList());
        }

        public async Task<Result<AddressDto>> UpdateOrInsertUserAddress(string email,AddressDto address ,CancellationToken ct = default)
        {
            var user =await _userManager.Users.Include(e => e.Address).FirstOrDefaultAsync(e => e.Email == email);

            if(user?.Address == null)
            {
                user!.Address = new Address()
                {
                     FirstName=address.FirstName,
                     LastName=address.LastName,
                     Street=address.Street,
                     City=address.City,
                     Country=address.Country
                };
            }

            else
            {
                user.Address.FirstName = address.FirstName;
                user.Address.LastName = address.LastName;
                user.Address.Street = address.Street;
                user.Address.City = address.City;
                user.Address.Country = address.Country;

            }
         var result=  await _userManager.UpdateAsync(user);
            if (result.Succeeded)
                return address;
            else
                return Error.Failure("Failure",string.Join(";",result.Errors.Select(e=>e.Description)));
        }
    }
}
