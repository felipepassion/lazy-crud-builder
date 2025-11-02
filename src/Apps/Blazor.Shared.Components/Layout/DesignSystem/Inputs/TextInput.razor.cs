using Microsoft.Extensions.FileSystemGlobbing.Internal;
using Niu.Nutri.CrossCuting.Infra.Utils.Extensions;
using System.Text.RegularExpressions;

namespace Niu.Nutri.Shared.Blazor.Components.Layout.DesignSystem.Inputs
{
    public enum Masks
    {
        Phone,
        CPF,
        Email
    }
    public static class MasksExtensions
    {

        #region Helpers

        // Helper para remover todos os caracteres não numéricos
        public static string RemoveMask(this string value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            return new string(value.Where(char.IsDigit).ToArray());
        }

        #endregion

        #region Phone

        public static bool IsValidPhone(this string value) =>
            value.Count(x => x == '(') <= 1
            && value.Count(x => x == ')') <= 1
            && value.Count(x => x == '-') <= 1;

        public static string PhoneMask(this string value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;

            value = value.RemoveMask();

            if (value.Length > 11)
            {
                value = value.Substring(0, 11);
            }

            if (value.Length > 2)
                value = value.Insert(0, "(").Insert(3, ") ");

            if (value.Length > 9)
                value = value.Insert(10, "-");

            return value;
        }

        #endregion

        #region CPF

        // Apenas valida caracteres permitidos para digitação, não a validade do CPF em si.
        public static bool IsValidCpf(this string value) => true;

        public static string CpfMask(this string value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;

            value = value.RemoveMask();

            if (value.Length > 11)
            {
                value = value.Substring(0, 11);
            }

            if (value.Length > 9)
                value = value.Insert(9, "-");
            if (value.Length > 6)
                value = value.Insert(6, ".");
            if (value.Length > 3)
                value = value.Insert(3, ".");

            return value;
        }

        #endregion

        #region Email

        // A validação de e-mail é complexa, aqui apenas verificamos a presença de caracteres comuns.
        public static bool ContainsAnyEmailChar(this string value) =>
             value.Contains('@') || value.Contains('.');

        #endregion

        public static string GetValueByMask(this Masks mask, string input)
        {
            switch (mask)
            {
                case Masks.Phone:
                    return input.PhoneMask();
                case Masks.CPF:
                    return input.CpfMask();
                case Masks.Email: // Email não possui formatação de máscara, retorna o valor original
                    return input;
                default:
                    throw new ArgumentException($"Mask not implemented {mask}");
            }
        }

        public static bool IsValidByMask(this Masks mask, string input)
        {
            switch (mask)
            {
                case Masks.Phone:
                    // Verifica se, após remover a máscara, todos os caracteres são dígitos.
                    return input.RemoveMask().All(char.IsDigit);
                case Masks.CPF:
                    // Verifica se, após remover a máscara, todos os caracteres são dígitos.
                    return input.RemoveMask().All(char.IsDigit);
                case Masks.Email:
                    // Para email, a validação é mais flexível durante a digitação.
                    return true;
                default:
                    throw new ArgumentException($"Validation not implemented {mask}");
            }
        }
    }

}
