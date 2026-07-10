using E_Commerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Contracts
{
    public interface IGenericRepository<TEntity,Tkey> where TEntity : BaseEntity<Tkey>
    {
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task<TEntity?> GetByIdAsync(Tkey id, CancellationToken ct=default);
        Task<TEntity?> GetByIdAsync(ISpecifications<TEntity,Tkey>spec, CancellationToken ct=default);
        Task<IReadOnlyList<TEntity>> GetAllAsync( CancellationToken ct=default);
        Task<IReadOnlyList<TEntity>> GetAllAsync(ISpecifications<TEntity,Tkey>spec ,CancellationToken ct=default);
    }
}
