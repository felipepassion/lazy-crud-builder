namespace Lazy.Crud.{Aggregate}.Enumerations
{
    /// <summary>Aggregate-level enumerations (kept in the Enumerations project by convention).</summary>
    public enum {Aggregate}Enumerations
    {
        None = 0,
    }

    /// <summary>Lifecycle of a product listing. Enumerations used by entities live here because the
    /// query-model project (Api.Queries) references only the Enumerations project.</summary>
    public enum ProductStatus
    {
        Draft = 0,
        Published = 1,
        Discontinued = 2,
    }
}
