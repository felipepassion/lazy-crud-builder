namespace Niu.Nutri.Core.Tests
{

    public class CPFGenerator
    {
        private static Random _random = new Random();

        public static string Generate()
        {
            var cpf = new int[11];

            for (int i = 0; i < 9; i++)
            {
                cpf[i] = _random.Next(0, 9);
            }

            cpf[9] = GenerateSecondDigit(cpf, 10);
            cpf[10] = GenerateSecondDigit(cpf, 11);

            return string.Join("", cpf.Select(x => x.ToString()).ToArray());
        }

        private static int GenerateSecondDigit(int[] cpf, int length)
        {
            int sum = 0;
            for (int i = 0, j = length; i < length - 1; i++, j--)
            {
                sum += cpf[i] * j;
            }

            var rest = sum % 11;
            if (rest < 2)
                return 0;
            else
                return 11 - rest;
        }
    }
}
