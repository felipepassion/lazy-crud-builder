using Niu.Nutri.Core.Application.DTO.Validators;

namespace Core.Application.DTO.Tests
{
    using Xunit;
    public class TelefoneValidadorTeste
    {
        [Theory(DisplayName = @"DADO um conjunto de números de telefone em diferentes formatos
                                QUANDO a função de valida-los é chamada
                                ENTÃO todos os números devem ser considerados válidos")]
        [InlineData("(11) 1234-5678")]
        [InlineData("(11) 91234-5678")]
        [InlineData("11 91234-5678")]
        [InlineData("21969791929")]
        public void ValidarNumbersValidosTeste(string Number)
        {
            var resultado = PhoneNumberValidator.IsValid(Number);
            Assert.True(resultado, $"O número {Number} deveria ser válido.");
        }

        [Theory(DisplayName = @"DADO um conjunto de números de telefone em diferentes formatos
                                QUANDO a função de valida-los é chamada
                                ENTÃO todos os números devem ser considerados inválidos")]
        [InlineData("12345678")] // Faltando DDD
        [InlineData("912345678")] // Faltando DDD
        [InlineData("11 12345-56789")] // Dígito extra
        [InlineData("11 91234-5678a")] // Caractere inválido
        [InlineData("(11) 1234")] // Faltando dígitos
        [InlineData("(11) 912345")] // Faltando dígitos
        public void ValidarNumbersInvalidosTeste(string Number)
        {
            var resultado = PhoneNumberValidator.IsValid(Number);
            Assert.False(resultado, $"O número {Number} deveria ser inválido.");
        }

        [Theory(DisplayName = @"DADO um campo de telefone vazio ou nulo
                                QUANDO a função de validar o número é chamada
                                ENTÃO todos os números devem ser considerados inválidos")]
        [InlineData(" ")]
        [InlineData(null)]
        public void ValidarNumbersVaziosTeste(string Number)
        {
            var resultado = PhoneNumberValidator.IsValid(Number);
            Assert.False(resultado, "Números vazios ou nulos deveriam ser inválidos.");
        }

        [Theory(DisplayName = @"DADO um DDD válido de um Número
                                QUANDO a função de validar o DDD é chamada
                                ENTÃO o DDD do número deve ser válido")]
        [InlineData("(11) 91234-5678")]
        [InlineData("(21) 91234-5678")]
        [InlineData("(31) 1234-5678")]
        [InlineData("(41) 1234-5678")]
        // Adicione mais DDDs válidos aqui
        public void ValidarDDDsValidosTeste(string Number)
        {
            var resultado = PhoneNumberValidator.IsValid(Number);
            Assert.True(resultado, $"O DDD do número {Number} deveria ser válido.");
        }

        [Theory(DisplayName = @"DADO um DDD inválido de um Número
                                QUANDO a função de validar o DDD é chamada
                                ENTÃO o DDD do número deve ser inválido")]
        [InlineData("(00) 91234-5678")]
        [InlineData("(10) 1234-5678")]
        [InlineData("(20) 1234-5678")]
        [InlineData("(30) 1234-5678")]
        [InlineData("(22) 9 2131-233")]
        // Adicione mais DDDs inválidos aqui
        public void ValidarDDDsInvalidosTeste(string Number)
        {
            var resultado = PhoneNumberValidator.IsValid(Number);
            Assert.False(resultado, $"O DDD do número {Number} deveria ser inválido.");
        }
    }

}