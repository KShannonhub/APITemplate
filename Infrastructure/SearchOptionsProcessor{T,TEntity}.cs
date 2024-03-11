using System.Linq.Expressions;
using System.Reflection;

namespace APITemplate.Infrastructure
{
    public class SearchOptionsProcessor<T, TEntity>
    {
        private readonly string[] _search;

        public SearchOptionsProcessor(string[] search)
        {
            _search = search;
        }

        public IEnumerable<SearchTerm> GetAllTerms()
        {
            if (_search == null)
            {
                yield break;
            }

            foreach (var expression in _search)
            {
                if (string.IsNullOrEmpty(expression))
                {
                    continue;
                }

                var tokens = expression.Split(' ');
                if (tokens.Length == 0)
                {
                    yield return new SearchTerm
                    {
                        ValidSyntax = false,
                        Name = expression,
                    };

                    continue;
                }

                if (tokens.Length < 3)
                {
                    yield return new SearchTerm
                    {
                        ValidSyntax = true,
                        Name = tokens[0],
                        Operator = tokens[1],
                        Value = string.Join(" ", tokens.Skip(2))
                    };

                    continue;
                }

                var searchCondition = tokens.Length > 1 ? tokens[1] : string.Empty;

                yield return new SearchTerm
                {
                    Name = tokens[0],
                    Operator = searchCondition
                };
            }
        }   

        private static IEnumerable<SearchTerm> GetTermsFromModel()
        {
            return typeof(T).GetTypeInfo()
                .DeclaredProperties
                .Where(p => p.GetCustomAttributes<SearchableAttribute>().Any())
                .Select(p => new SearchTerm
                {
                    Name = p.Name,

                });
        }

        public IEnumerable<SearchTerm> GetValidTerms()
        {
            var queryTerms = GetAllTerms().ToArray();
            if (!queryTerms.Any()) yield break;

            var declaredTerms = GetTermsFromModel();

            foreach (var term in queryTerms)
            {
                var declaredTerm = declaredTerms
                    .SingleOrDefault(x => x.Name.Equals(term.Name, StringComparison.OrdinalIgnoreCase));
                if (declaredTerm == null) continue;

                yield return new SearchTerm
                {
                    Name = declaredTerm.Name,
                    Operator = term.Operator,
                    Value = term.Value,
                    ValidSyntax = term.ValidSyntax
                };
            }
        }

        public IQueryable<TEntity> Apply(IQueryable<TEntity> query)
        {
            var terms = GetValidTerms().ToArray();

            if(!terms.Any())
            {
                return query;
            }

            var modifiedQuery = query;

            foreach (var term in terms)
            {
                var propertyInfo = ExpressionHelper
                    .GetPropertyInfo<TEntity>(term.Name);
                var obj = ExpressionHelper.Parameter<TEntity>();

                // Build the LINQ expression backwards:
                // query = query.Where(x => x.Property == "Value");
                var left = ExpressionHelper.GetPropertyExpression(obj, propertyInfo);
                var right = Expression.Constant(term.Value);
                var comparisonExpression = Expression.Equal(left, right);

                var lambdaExpression = ExpressionHelper.GetLambda<TEntity, bool>(obj, comparisonExpression);

                modifiedQuery = ExpressionHelper.CallWhere(
                                       modifiedQuery, lambdaExpression);
            }

            return modifiedQuery;
        }
    }
}
