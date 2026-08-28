using E_Commerce.Domain.Common;
using E_Commerce.Domain.Contracts;
using E_Commerce.Infrastructure.Data;
using E_Commerce.Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Repositories
{
    internal class GenericRepository<TEntity,Tkey>(StoreDbContext dbContext) : IGenericRepository<TEntity, Tkey> where TEntity :BaseEntity<Tkey>
    {
        public void Add(TEntity entity) =>dbContext.Set<TEntity>().Add(entity);

        public async Task<int> CountAsync(ISpecifications<TEntity, Tkey> spec, CancellationToken ct=default)
        {
            return await SpecificationEvaluator.CreateQuery(dbContext.Set<TEntity>(), spec).CountAsync(ct);
        }

        public void Delete(TEntity entity) => dbContext.Set<TEntity>().Remove(entity);
   

        public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken ct=default)
        =>await dbContext.Set<TEntity>().AsNoTracking().ToListAsync(ct);

        public async Task<IReadOnlyList<TEntity>> GetAllAsync(ISpecifications<TEntity, Tkey> spec, CancellationToken ct = default)
        {
                    var query = SpecificationEvaluator.CreateQuery(dbContext.Set<TEntity>(), spec);

            return await query.ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(Tkey id, CancellationToken ct = default)
       => await dbContext.Set<TEntity>().FindAsync(id,ct);

        public async Task<TEntity?> GetByIdAsync(ISpecifications<TEntity, Tkey> spec, CancellationToken ct = default)
        {
            var query = SpecificationEvaluator.CreateQuery(dbContext.Set<TEntity>(), spec);
            return await query.FirstOrDefaultAsync();
        }

        public void Update(TEntity entity)=>dbContext.Set<TEntity>().Update(entity);
    }
}
