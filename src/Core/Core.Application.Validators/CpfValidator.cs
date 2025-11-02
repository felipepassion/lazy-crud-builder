using System.Text.RegularExpressions;

namespace Niu.Nutri.Core.Application.Validators
{
    public static class CpfValidator
    {
        public static bool ValidCPF(string cpf)
        {
            if (string.IsNullOrEmpty(cpf)) return false;

            cpf = Regex.Replace(cpf, "[^0-9]", "");
            if (cpf.Length != 11) return false;
            if (new string(cpf[0], 11) == cpf) return false;

            int soma = 0;
            for (int i = 0; i < 9; i++) soma += int.Parse(cpf[i].ToString()) * (10 - i);

            int resto = soma % 11;
            if (resto < 2) resto = 0;
            else resto = 11 - resto;

            if (resto != int.Parse(cpf[9].ToString())) return false;

            soma = 0;
            for (int i = 0; i < 10; i++) soma += int.Parse(cpf[i].ToString()) * (11 - i);

            resto = soma % 11;
            if (resto < 2) resto = 0;
            else resto = 11 - resto;

            if (resto != int.Parse(cpf[10].ToString())) return false;

            return true;
        }
    }
}