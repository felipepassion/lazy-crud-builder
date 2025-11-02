using System.Text.RegularExpressions;

namespace Niu.Nutri.Core.Application.Validators
{
    public static class CnpjValidator
    {
        public static bool ValidCNPJ(string cnpj)
        {
            if(string.IsNullOrWhiteSpace(cnpj)) return true;

            cnpj = Regex.Replace(cnpj, @"[^0-9]", "");

            if (cnpj.Length != 14)
            {
                return false;
            }

            var allDigitsEqual = true;

            for (int i = 1; i < cnpj.Length; i++)
            {
                if (cnpj[i] != cnpj[0])
                {
                    allDigitsEqual = false;
                    break;
                }
            }

            if (allDigitsEqual)
            {
                return false;
            }

            int[] multipliers = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int sum = 0;

            for (int i = 0; i < 12; i++)
            {
                sum += int.Parse(cnpj[i].ToString()) * multipliers[i];
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

            // Atualize os multiplicadores para o segundo dígito verificador
            multipliers = new int[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            sum = 0;

            for (int i = 0; i < 13; i++)
            {
                sum += int.Parse(cnpj[i].ToString()) * multipliers[i];
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
