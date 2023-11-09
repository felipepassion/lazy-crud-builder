using FluentValidation.Results;

namespace LazyCrudBuilder.Core.Domain.Aggregates.CommonAgg.Commands
{
    public class CommandResponse
    {
        public bool Success { get { return this.ValidationResult?.IsValid == true; } }
        public ValidationResult ValidationResult { get; set; }
        public string Code { get; set; }
    }
}
