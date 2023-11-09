using LazyCrudBuilder.Core.Application.DTO.Validators;

namespace Core.Application.DTO.Tests
{
    using Xunit;
    public class TelefoneValidadorTeste
    {
        [Theory]
        [InlineData("(11) 1234-5678")]
        [InlineData("(11) 91234-5678")]
        [InlineData("11 91234-5678")]
        [InlineData("21969791929")]
        public void ValidarNumerosValidosTeste(string numero)
        {
            var resultado = PhoneNumberValidator.IsValid(numero);
            Assert.True(resultado, $"O n�mero {numero} deveria ser v�lido.");
        }

        [Theory]
        [InlineData("12345678")] // Faltando DDD
        [InlineData("912345678")] // Faltando DDD
        [InlineData("11 12345-56789")] // D�gito extra
        [InlineData("11 91234-5678a")] // Caractere inv�lido
        [InlineData("(11) 1234")] // Faltando d�gitos
        [InlineData("(11) 912345")] // Faltando d�gitos
        public void ValidarNumerosInvalidosTeste(string numero)
        {
            var resultado = PhoneNumberValidator.IsValid(numero);
            Assert.False(resultado, $"O n�mero {numero} deveria ser inv�lido.");
        }

        [Theory]
        [InlineData(" ")]
        [InlineData(null)]
        public void ValidarNumerosVaziosTeste(string numero)
        {
            var resultado = PhoneNumberValidator.IsValid(numero);
            Assert.False(resultado, "N�meros vazios ou nulos deveriam ser inv�lidos.");
        }

        [Theory]
        [InlineData("(11) 91234-5678")]
        [InlineData("(21) 91234-5678")]
        [InlineData("(31) 1234-5678")]
        [InlineData("(41) 1234-5678")]
        // Adicione mais DDDs v�lidos aqui
        public void ValidarDDDsValidosTeste(string numero)
        {
            var resultado = PhoneNumberValidator.IsValid(numero);
            Assert.True(resultado, $"O DDD do n�mero {numero} deveria ser v�lido.");
        }

        [Theory]
        [InlineData("(00) 91234-5678")]
        [InlineData("(10) 1234-5678")]
        [InlineData("(20) 1234-5678")]
        [InlineData("(30) 1234-5678")]
        [InlineData("(22) 9 2131-233")]
        // Adicione mais DDDs inv�lidos aqui
        public void ValidarDDDsInvalidosTeste(string numero)
        {
            var resultado = PhoneNumberValidator.IsValid(numero);
            Assert.False(resultado, $"O DDD do n�mero {numero} deveria ser inv�lido.");
        }
    }

}