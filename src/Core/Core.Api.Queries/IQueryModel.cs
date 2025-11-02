namespace Niu.Nutri.Core.Api.Queries
{
    public interface IQueryModel
    {
        public string? Selector { get; set; }
        public string? OrderBy { get; set; }
        public bool? OrderByDesc { get; set; }
        public bool? GetOnlySummary { get; set; }
        public bool IsOrSpecification { get; set; }
    }
}
