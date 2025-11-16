namespace Lazy.Crud.Builder.Application.DTO.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToBrazilianTimeString(this DateTime? dateTime)
        {
            if (dateTime == null)
                return string.Empty;

            var brasilTimeZone = TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");
            var brasilTime = TimeZoneInfo.ConvertTimeFromUtc(dateTime.Value, brasilTimeZone);
            return brasilTime.ToString("HH:mm");
        }
    }
}
