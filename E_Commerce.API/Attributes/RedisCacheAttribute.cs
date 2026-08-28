using E_Commerce.Application.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace E_Commerce.API.Attributes
{
    public class RedisCacheAttribute : ActionFilterAttribute
    {
        private readonly int _duratoinInSec;

        public RedisCacheAttribute( int duratoinInSec=60)
        {
            _duratoinInSec = duratoinInSec;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)

        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
            var cacheKey = CreateCacheKey(context.HttpContext.Request);
            var data = await cacheService.GetAsync(cacheKey);

            if (!string.IsNullOrEmpty(data))
            {

                context.Result = new ContentResult
                {
                    Content = data,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK


                };
                return;


            }



            var excuted = await next.Invoke();

            if (excuted.Result is OkObjectResult { Value: not null } ok)
                await cacheService.SetAsync(cacheKey, ok.Value, TimeSpan.FromSeconds(_duratoinInSec));

        }


        private static string CreateCacheKey(HttpRequest requst)
        {
            var key = new StringBuilder();

            key.Append(requst.Path).Append('?');

            foreach (var (k,v) in requst.Query.OrderBy(k=>k.Key))
            {
                key.Append(k).Append('=').Append(v).Append('&');
            }
            return key.ToString();

        }

    }
}
