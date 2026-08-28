using E_Commerce.Domain.Common;
using E_Commerce.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Specifications
{
    internal static class SpecificationEvaluator
    {
        public static IQueryable<TEntity> CreateQuery<TEntity,TKey>(IQueryable<TEntity> input,ISpecifications<TEntity,TKey> spec) where TEntity:BaseEntity<TKey>
        {
            
            var query = input;

            if(spec.Critria != null)
            {
                query = query.Where(spec.Critria);
            }


            if ( spec.IncludeExpresssions.Any())
            {
                 query=  spec.IncludeExpresssions.Aggregate(query, (current, next) => current.Include(next));
            }

            if (spec.OrderBy != null)
            {
                query=query.OrderBy(spec.OrderBy);
            }
            else if(spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }

            if(spec.IsPaginated)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }

                return query;
        }
    }
}
