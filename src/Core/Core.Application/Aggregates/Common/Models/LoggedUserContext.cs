namespace Niu.Nutri.Core.Application.Aggregates.Common.Models
{
    public interface ILoggedUserContext
    {
        int UserId { get; set; }
        string? UserName { get; set; }
        string? ExternalId { get; set; }
    }

    public class LoggedUserContext(int id, string? userName) : ILoggedUserContext
    {
        public int UserId { get; set; } = id;
        public string? UserName { get; set; } = userName;
        public string? ExternalId { get; set; }
    }
}
