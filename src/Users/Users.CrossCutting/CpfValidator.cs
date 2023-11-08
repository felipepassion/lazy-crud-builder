using System.Text.RegularExpressions;

namespace Users.CrossCutting
{
    public static class CpfValidator
    {
        public static bool ValidateCNPJ(string cnpj)
        {
            cnpj = Regex.Replace(cnpj, @"[^0-9]", "");

            if (cnpj.Length != 14)
            {
                return false;
            }

            int[] multiplier1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplier2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int sum = 0;

            for (int i = 0; i < 12; i++)
            {
                sum += int.Parse(cnpj[i].ToString()) * multiplier1[i];
            }

            int remainder = (sum % 11);

            if (remainder < 2)
            {
                remainder = 0;
            }
            else
            {
                remainder = 11 - remainder;
            }

            if (int.Parse(cnpj[12].ToString()) != remainder)
            {
                return false;
            }

            sum = 0;

            for (int i = 0; i < 13; i++)
            {
                sum += int.Parse(cnpj[i].ToString()) * multiplier2[i];
            }

            remainder = (sum % 11);

            if (remainder < 2)
            {
                remainder = 0;
            }
            else
            {
                remainder = 11 - remainder;
            }

            if (int.Parse(cnpj[13].ToString()) != remainder)
            {
                return false;
            }

            return true;
        }
    }
}