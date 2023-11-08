namespace LazyCrud.Core.Application.DTO.Aggregates.CommonAgg.Models
{
    public class DomainResponseDTO
    {
        public Exception? Exception { get; set; }
        public Dictionary<string,string> Errors { get; set; }
        public EntityDTO Data { get; set; }
    }
}