using E_Commerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Contracts
{
    public interface IUnitOfWork
    {
        IGenericRepository<TEntity, Tkey> GetRepository<TEntity,Tkey>() where TEntity:BaseEntity<Tkey>;
        Task<int> SaveChangesAsync(CancellationToken ct=default);

    }
}
