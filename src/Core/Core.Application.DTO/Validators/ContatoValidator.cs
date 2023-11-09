using System.Text.RegularExpressions;

namespace LazyCrud.Core.Application.DTO.Validators
{
    public static class PhoneNumberValidator
    {
        public static bool IsValid(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
            {
                Console.WriteLine("O número de telefone não pode ser vazio.");
                return false;
            }

            // Checa se o número contém apenas dígitos e caracteres permitidos em uma máscara de número de telefone
            if (!Regex.IsMatch(phoneNumber, @"^[\d \-\(\)]+$"))
            {
                Console.WriteLine("Número de telefone inválido. Apenas dígitos e caracteres de máscara são permitidos.");
                return false;
            }

            if (!ValidDDD(phoneNumber))
            {
                Console.WriteLine("DDD inválido.");
                return false;
            }

            string cleanNumber = Regex.Replace(phoneNumber, @"\D", ""); // remove caracteres não numéricos

            if (cleanNumber.Length == 11 && cleanNumber[2] == '9') // celular com nono dígito
            {
                cleanNumber = cleanNumber.Remove(2, 1); // remove o dígito 9 do DDD
                if (cleanNumber.Length == 10)
                {
                    Console.WriteLine("Telefone móvel válido.");
                    return true;
                }
            }
            else if (cleanNumber.Length == 10 && cleanNumber[2] != '9') // telefone fixo não deve começar com '9'
            {
                Console.WriteLine("Telefone fixo válido.");
                return true;
            }

            Console.WriteLine("Número de telefone inválido.");
            return false;
        }

        private static bool ValidDDD(string phoneNumber)
        {
            string phoneRegex = @"^\(?(?<ddd>[1-9]{2})\)?[\s-]?\d";
            Match match = Regex.Match(phoneNumber, phoneRegex);

            if (match.Success)
            {
                string ddd = match.Groups["ddd"].Value;
                HashSet<string> validDDD = new HashSet<string> { "11", "12", "13", "14", "15", "16", "17", "18", "19", "21", "22", "24", "27", "28", "31", "32", "33", "34", "35", "37", "38", "41", "42", "43", "44", "45", "46", "47", "48", "49", "51", "53", "54", "55", "61", "62", "63", "64", "65", "66", "67", "68", "69", "71", "73", "74", "75", "77", "79", "81", "82", "83", "84", "85", "86", "87", "88", "89", "91", "92", "93", "94", "95", "96", "97", "98", "99" };
                return validDDD.Contains(ddd);
            }

            return false;
        }
    }
}
