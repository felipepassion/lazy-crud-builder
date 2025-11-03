using System.Text.RegularExpressions;

namespace Lazy.Crud.Core.Application.DTO.Validators
{
    public static class PhoneNumberValidator
    {
        // Mantém compatibilidade: versão antiga apenas valida, ignorando se é estrangeiro
        public static bool IsValid(string? phoneNumber)
        {
            return true;

            return IsValid(phoneNumber, out _);
        }
 
        // Nova sobrecarga: identifica se há código de país diferente de 55
        public static bool IsValid(string? phoneNumber, out bool isForeign)
        {
            isForeign = false;
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                Console.WriteLine("O número de telefone não pode ser vazio.");
                return false;
            }

            string original = phoneNumber.Trim();

            // Detecta prefixo de código de país: +CC... ou 00CC...
            var ccMatch = Regex.Match(original, @"^(?:\+|00)(?<cc>\d{1,3})(?<rest>.*)$");
            if (ccMatch.Success)
            {
                string countryCode = ccMatch.Groups["cc"].Value;
                string rest = ccMatch.Groups["rest"].Value;

                // Remove espaços e separadores comuns do restante
                string cleanedRest = Regex.Replace(rest, @"[\s\-\(\)]", "");

                if (countryCode == "55")
                {
                    // Trata como número brasileiro (não estrangeiro)
                    phoneNumber = cleanedRest; // prossegue com validação brasileira
                }
                else
                {
                    isForeign = true;
                    // Validação internacional simplificada (formato próximo ao E.164, mas tolerando ausência de '+')
                    // Total (countryCode + rest) já tem countryCode garantido; agora validamos o conjunto completo normalizado
                    string internationalDigits = countryCode + cleanedRest;
                    // Regra: 8 a 15 dígitos totais (ITU E.164 permite até 15)
                    if (Regex.IsMatch(internationalDigits, @"^[1-9]\d{7,14}$"))
                    {
                        Console.WriteLine($"Telefone internacional válido. Código do país: {countryCode}");
                        return true;
                    }
                    Console.WriteLine("Número de telefone internacional inválido.");
                    return false;
                }
            }

            // Fluxo brasileiro
            return ValidateBrazilian(phoneNumber);
        }

        private static bool ValidateBrazilian(string? phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber)) return false;

            // Permite dígitos e caracteres de máscara brasileiros comuns (agora permite '+', caso chegue aqui por engano será filtrado no regex de limpeza)
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

            // cleanNumber esperado: DDD + número (10 ou 11 dígitos com nono)
            if (cleanNumber.Length == 11 && cleanNumber[2] == '9') // celular com nono dígito
            {
                string afterRemoving9 = cleanNumber.Remove(2, 1); // remove o dígito 9 após o DDD para checagem de base 10
                if (afterRemoving9.Length == 10)
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

        public static bool ValidDDD(this string? phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber)) return false;

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
