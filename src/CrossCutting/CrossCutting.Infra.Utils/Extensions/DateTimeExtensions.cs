namespace Lazy.Crud.CrossCutting.Infra.Utils.Extensions;

public static class DateTimeExtensions
{
    public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
    {
        int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
        return dt.AddDays(-diff).Date;
    }

    public static DateTime EndOfWeek(this DateTime dt, DayOfWeek endOfWeek)
    {
        int diff = ((int)endOfWeek - (int)dt.DayOfWeek + 7) % 7;
        return dt.AddDays(diff).Date;
    }

    public static DateOnly StartOfWeek(this DateOnly dt, DayOfWeek startOfWeek)
    {
        int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
        return dt.AddDays(-diff);
    }

    public static DateOnly EndOfWeek(this DateOnly dt, DayOfWeek endOfWeek)
    {
        int diff = ((int)endOfWeek - (int)dt.DayOfWeek + 7) % 7;
        return dt.AddDays(diff);
    }

    public static DateOnly StartOfMonth(this DateOnly date)
    {
        return new DateOnly(date.Year, date.Month, 1);
    }

    public static DateOnly EndOfMonth(this DateOnly date)
    {
        int daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
        return new DateOnly(date.Year, date.Month, daysInMonth);
    }
}
