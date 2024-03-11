using APITemplate.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace APITemplate.Models
{
    public class SortOptions<T, TEntity> : IValidatableObject
    {
        public string[] OrderBy { get; set; }

        // APS Call this to validate the sort options
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var processor = new SortProcesses<T, TEntity>(OrderBy);

            var validTerms = processor.GetValidTerms().Select(x => x.Name);

            var invalidTerms = processor.GetAllTerms().Select(x => x.Name)
                .Except(validTerms, StringComparer.OrdinalIgnoreCase);

            foreach (var term in invalidTerms)
            {
                yield return new ValidationResult(
                                       $"Invalid sort term '{term}'.",
                                                          new[] { nameof(OrderBy) });
            }
        }

        public IQueryable<TEntity> Apply(IQueryable<TEntity> query)
        {
            var processor = new SortProcesses<T, TEntity>(OrderBy);

            return processor.Apply(query);
        }
    }
}
