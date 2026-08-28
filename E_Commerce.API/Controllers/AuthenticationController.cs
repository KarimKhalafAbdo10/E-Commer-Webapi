using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Xml;

namespace E_Commerce.API.Controllers
{

    public class AuthenticationController : ApiBaseController
    {
        private readonly IAuthenticationService _authenticationService;
        
        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        #region login and register
        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> LogIn(LogInDto logInDto) => ToActionResult(await _authenticationService.LogInAsync(logInDto));

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto, CancellationToken ct)
            => ToActionResult(await _authenticationService.RegisterAsync(registerDto, ct));

        #endregion

        #region Check Email Exist
        [HttpGet("emailexist")]
        public async Task<ActionResult<bool>> CheckEmailExists([FromQuery] string? email, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest("Email parameter is required.");

            return ToActionResult(await _authenticationService.CheckEmailExistsAsync(email, ct));
        }
        #endregion

        #region Get Current User
        [Authorize]
        [HttpGet("currentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser(CancellationToken ct)
        {
            return ToActionResult(await _authenticationService.GetCurrentUser(GetEmailFromToken(), ct));
        }
        #endregion

        #region Get Current User Address

        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetCurrentUserAddress(CancellationToken ct)
        {
            return ToActionResult(await _authenticationService.GetCurrentUserAddress(GetEmailFromToken(), ct));
        }

        #endregion

        #region Update User Address
        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDto>> UpdateOrInsertUserAddress(AddressDto address, CancellationToken ct)
            => ToActionResult(await _authenticationService.UpSertUserAddress(GetEmailFromToken(), address, ct));
        #endregion

        #region Change Password
        [Authorize]
        [HttpPost("changepassword")]
           
        public async Task<ActionResult<bool>> ChangePassword(ChangePasswordDto changePasswordDto, CancellationToken ct)
            => ToActionResult(await _authenticationService.ChangePassword(GetEmailFromToken(), changePasswordDto, ct));
        #endregion

        #region Forget Password
        [HttpPost("forgotpassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgetPassWordDto forgetPassWordDto, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _authenticationService.ForgotPasswordAsync(forgetPassWordDto, ct);

            if (!result.IsSuccess)
                return BadRequest(result.Errors);

            return Ok(new { Message = result.data });


        }

        #endregion

    }
}
