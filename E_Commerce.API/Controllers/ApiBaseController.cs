using E_Commerce.Application.common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiBaseController : ControllerBase
    {


        public static ActionResult<T> ToActionResult<T>(Result<T> result)
        {
            if (result.IsSuccess) return new OkObjectResult(result.data);
            return ToProblem(result.Errors);
        }

        public static ActionResult ToActionResult(Result result)
        {
            if (result.IsSuccess) return new OkResult();
            return ToProblem(result.Errors);

        }
        protected static ObjectResult ToProblem(IReadOnlyList<Error> errors)
        {
            var firstError = errors[0];
            var status = firstError.Type switch
            {
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.validation => StatusCodes.Status400BadRequest,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                _ => StatusCodes.Status500InternalServerError



            };
            var problem = new ProblemDetails
            {
                Status = status,
                Title = firstError.Code,
                Detail = firstError.description,

                Extensions = { ["errors"] = errors }
            };
            return new ObjectResult(problem) { StatusCode = status };
        }


        protected string GetEmailFromToken()
        {
            try
            {
                var email = User.FindFirstValue(ClaimTypes.Email);
                if (string.IsNullOrWhiteSpace(email))
                    throw new UnauthorizedAccessException("No Email Claims Found");
                return email;
            }
            catch (Exception ex)
            {
                throw new UnauthorizedAccessException("Unable to extract email from token", ex);
            }
        }
    }
}
