

using Core.Entites;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery,ISpecification<TEntity> spec)
        {
            var query=inputQuery;
            if(spec.Criteria!=null)
            {
                query=query.Where(spec.Criteria); //p=>=ProducttypeId=id;
            }
            if(spec.OrderBy!=null)
            {
                query=query.OrderBy(spec.OrderBy); //for sorting in ascending
            }
            if(spec.OrderByDescending!=null)
            {
                query=query.OrderByDescending(spec.OrderByDescending); //for sorting in descending
            }
            if(spec.IsPagingEnabled)
            {
                query=query.Skip(spec.Skip).Take(spec.Take);
            }
            query=spec.Includes.Aggregate(query,(current,include)=>current.Include(include));
            return query;
        }
    }
}