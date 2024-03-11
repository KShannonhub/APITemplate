using System.Reflection;

namespace APITemplate.Infrastructure
{
    public class SortProcesses<T, TEnitity>
    {
        private readonly string[] _orderBy;

        public SortProcesses(string[] orderBy)
        {
            _orderBy = orderBy;
        }

        public IEnumerable<SortTerm> GetAllTerms()
        {
            if (_orderBy == null)
            {
                yield break;
            }

            foreach (var term in _orderBy)
            {
                if (string.IsNullOrEmpty(term))
                {
                    continue;
                }

                var tokens = term.Split(' ');
                if (tokens.Length == 0)
                {
                    yield return new SortTerm
                    {
                        Name = term,
                    };

                    continue;
                }

                var descending = tokens.Length > 1 && tokens[1].Equals("desc", StringComparison.OrdinalIgnoreCase);
                
                yield return new SortTerm
                {
                    Name = tokens[0],
                    Descending = descending
                };
            }
        }

        public IEnumerable<SortTerm> GetValidTerms()
        {
            var queryTerms = GetAllTerms().ToArray();
            if (!queryTerms.Any()) yield break;

            var declaredTerms = GetTermsFromModel();

            foreach (var term in queryTerms)
            {
                var declaredTerm = declaredTerms
                    .SingleOrDefault(x => x.Name.Equals(term.Name, StringComparison.OrdinalIgnoreCase));
                if (declaredTerm == null) continue;

                yield return new SortTerm
                {
                    Name = declaredTerm.Name,
                    Descending = term.Descending,
                    Default = declaredTerm.Default
                };
            }
        }

        private static IEnumerable<SortTerm> GetTermsFromModel()
          => typeof(T).GetTypeInfo()
          .DeclaredProperties
          .Where(p => p.GetCustomAttributes<Sortable>().Any())
          .Select(p => new SortTerm
          {
              Name = p.Name,
              Default = p.GetCustomAttribute<Sortable>().Default
          });

        public IQueryable<TEnitity> Apply(IQueryable<TEnitity> query)
        {
            var terms = GetValidTerms().ToArray();
            if (!terms.Any())
            {
                terms = GetTermsFromModel().Where(t => t.Default).ToArray();
            }

            // If there are still no terms, return the original query
            if (!terms.Any()) return query;

            var modifiedQuery = query;
            var useThenBy = false;

            for (var i = 0; i < terms.Length; i++)
            {
                var term = terms[i];
                var property = ExpressionHelper.GetPropertyInfo<TEnitity>(term.Name);
                var obj = ExpressionHelper.Parameter<TEnitity>();

                var key = ExpressionHelper.GetPropertyExpression(obj, property);
                var keySelector = ExpressionHelper.GetLambda(typeof(TEnitity), property.PropertyType, obj, key);

                modifiedQuery = ExpressionHelper.CallOrderByOrThenBy(modifiedQuery, useThenBy, term.Descending, property.PropertyType, keySelector);
                useThenBy = true;
            }

            return modifiedQuery;
        }
    }
}
