namespace Niu.Nutri.Core.Application.DTO.Aggregates.CommonAgg.Models.Auth
{
    public class UserInfoDTO
    {
        public string? UserId { get; set; }
        public bool IsAuthenticated { get; set; }
        public required string Email { get; set; }
        public Dictionary<string, string> ExposedClaims { get; set; } = new Dictionary<string, string>();
    }
}
