namespace Niu.Nutri.Core.Enumeration
{
    public static class DateInputEnums
    {
        public enum Month
        {
            January = 1,
            February = 2,
            March = 3,
            April = 4,
            May = 5,
            June = 6,
            July = 7,
            August = 8,
            September = 9,
            October = 10,
            November = 11,
            December = 12
        }
        public static string GetMonthName(Month month)
        {

            return month switch
            {
                Month.January => "Janeiro",
                Month.February => "Fevereiro",
                Month.March => "Março",
                Month.April => "Abril",
                Month.May => "Maio",
                Month.June => "Junho",
                Month.July => "Julho",
                Month.August => "Agosto",
                Month.September => "Setembro",
                Month.October => "Outubro",
                Month.November => "Novembro",
                Month.December => "Dezembro",
                _ => "Mês inválido"
            };
        }

        public static string[] GetMonthsList()
        {
            string[] months = new string[12];
            for (int i = 0; i < 12; i++)
            {
                months[i] = GetMonthName((Month)(i + 1));
            }
            return months;
        }

        public static string[] GetDays()
        {
            string[] days = new string[31];
            for (int i = 0; i < 31; i++)
            {
                days[i] = (i + 1).ToString("D2");
            }

            return days;

        }

        public static string[] GetYears()
        {
            string[] years = new string[100];
            for (int i = 0; i < 100; i++)
            {
                years[i] = (DateTime.Now.Year - i).ToString("D2");
            }
            return years;
        }

        public static string FormatHour(this string? type)
        {
            if (string.IsNullOrWhiteSpace(type))
                return string.Empty;
            if (type.Length == 1)
                return "0" + type;
            return type;
        }
    }
}
