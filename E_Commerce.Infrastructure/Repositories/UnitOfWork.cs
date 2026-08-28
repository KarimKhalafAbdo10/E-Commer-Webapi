using E_Commerce.Domain.Common;
using E_Commerce.Domain.Contracts;
using E_Commerce.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Repositories
{
    internal class UnitOfWork(StoreDbContext dbContext) : IUnitOfWork
    {

        private readonly Dictionary<string, object> Repositories = [];
        public IGenericRepository<TEntity, Tkey> GetRepository<TEntity, Tkey>() where TEntity : BaseEntity<Tkey>
        {
            var typeName = typeof(TEntity).Name; 

            if(Repositories.TryGetValue(typeName,out object? value))
            
                return(IGenericRepository<TEntity,Tkey>) value;

            var repo = new GenericRepository<TEntity, Tkey>(dbContext);
            Repositories[typeName] = repo;
            return repo;
            

        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await dbContext.SaveChangesAsync(ct);
    }
}
