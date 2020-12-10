using System.Linq;
using Core.Entities;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class SpecificationEvaluator<TEntity> where TEntity: BaseEntity
    {
        // We are calling GetQuery method and passing to it our entity as an IQueryable and call it inputQuery
        // e.g. we are replacing entity with Product, and it is gonna be IQueruable<Product>
        // then we are saying get me the product is whatever we've specified as the spec.Criteria
        public static IQueryable<TEntity> GetQuery(
            IQueryable<TEntity> inputQuery, 
            ISpecification<TEntity> spec)
        {
            var query = inputQuery; 
            if(spec.Criteria != null) {
                query = query.Where(spec.Criteria);
            }

            // Our includes are going to be aggregates 
            // because we are combining all of our inlcude operations
            query = spec.Includes.Aggregate(
                query, 
                (current, include) => current.Include(include));
            
            return query;
        }
    }
}