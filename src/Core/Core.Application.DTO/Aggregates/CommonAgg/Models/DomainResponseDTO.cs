namespace Niu.Nutri.Core.Application.DTO.Aggregates.CommonAgg.Models
{
    public class DomainResponseDTO
    {
        public Exception? Exception { get; set; }
        public Dictionary<string,string>? Errors { get; set; }
        public EntityDTO? Data { get; set; }
    }

    public class DomainResponseDTO<T> where T : IEntityDTO
    {
        public Exception? Exception { get; set; }
        public Dictionary<string, string>? Errors { get; set; }
        public T? Data { get; set; }
    }
}