using System.Linq.Expressions;

namespace Core.Specifications
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        public BaseSpecification()
        {

        }
        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        public Expression<Func<T, bool>> Criteria {get;}

        public List<Expression<Func<T, object>>> Includes {get;}=
        new List<Expression<Func<T,Object>>>();
        // for sorting below two added 59
        public Expression<Func<T, object>> OrderBy { get ;private set;}
        public Expression<Func<T, object>> OrderByDescending { get;private set;}

        public int Take { get;private set;}

        public int Skip { get;private set;}

        public bool IsPagingEnabled { get;private set;}

        protected void AddInclude(Expression<Func<T,Object>>  includeExpression)
        {
            Includes.Add(includeExpression);
        }

        // for sorting below two added 59
        protected void AddOrderBy(Expression<Func<T, object>> orderByexpression)
        {
            OrderBy=orderByexpression;
        }
        protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescexpression)
        {
            OrderByDescending=orderByDescexpression;
        }
        protected void ApplyPaging(int skip,int take)
        {
            Skip=skip;
            Take=take;
            IsPagingEnabled=true;

        }
    }
}