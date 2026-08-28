using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Contracts
{
    public interface ICacheService
    {
        Task<string?> GetAsync(string cacheKey,CancellationToken ct=default);

        Task SetAsync(string casheKey, object cacheValue, TimeSpan? timeToLive = default,CancellationToken ct=default);

    }
}
