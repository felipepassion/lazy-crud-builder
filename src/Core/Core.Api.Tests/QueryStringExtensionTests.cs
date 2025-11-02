using Niu.Nutri.CrossCuting.Infra.Utils.Extensions;

namespace Core.Api.Tests;
public class QueryStringExtensionTests
{
    public class TestObject
    {
        public string? Name { get; set; }
        public int? Age { get; set; }
        public string? City { get; set; }
        public string? Description { get; set; }
    }

    [Fact(DisplayName = @"DADO um objeto com múltiplas propriedades preenchidas
                          QUANDO o método de transformar o objeto em string é invocado
                          ENTÃO retorna a string de consulta corretamente formatada e codificada")]
    public void ObjectToQueryString_WithMultipleProperties_ReturnsCorrectQueryString()
    {
        // Arrange
        var obj = new TestObject
        {
            Name = "John Doe",
            Age = 30,
            City = "New York",
            Description = "A developer"
        };

        // Act
        var queryString = obj.ObjectToQueryString();

        // Assert
        Assert.Equal("?Name=John%20Doe&Age=30&City=New%20York&Description=A%20developer", queryString);
    }

    [Fact(DisplayName = @"DADO um objeto com alguma propriedade vazia
                          QUANDO o método de transformar o objeto em string é invocado
                          ENTÃO retorna a string de consulta corretamente formatada e codificada")]
    public void ObjectToQueryString_WithNullValues_SkipsNullProperties()
    {
        // Arrange
        var obj = new TestObject
        {
            Name = "Jane Doe",
            Age = 25,
            City = null! // This should be skipped
        };

        // Act
        var queryString = obj.ObjectToQueryString();

        // Assert
        Assert.Equal("?Name=Jane%20Doe&Age=25", queryString);
    }

    [Fact(DisplayName = @"DADO um objeto com todas as propriedades vazias
                          QUANDO o método de transformar o objeto em string é invocado
                          ENTÃO retorna a string vazia de consulta")]
    public void ObjectToQueryString_WithNoProperties_ReturnsEmptyString()
    {
        // Arrange
        var obj = new object(); // No properties to encode

        // Act
        var queryString = obj.ObjectToQueryString();

        // Assert
        Assert.Empty(queryString);
    }

    [Fact(DisplayName = @"DADO um objeto com propriedades contendo caracteres especiais
                          QUANDO o método de transformar o objeto em string é invocado
                          ENTÃO retorna a string de consulta corretamente formatada e codificada")]
    public void ObjectToQueryString_WithSpecialCharacters_EncodesCorrectly()
    {
        // Arrange
        var obj = new TestObject
        {
            Name = "John&Jane",
            City = "New York/San Francisco"
        };

        // Act
        var queryString = obj.ObjectToQueryString();

        // Assert
        Assert.Equal("?Name=John%26Jane&City=New%20York%2FSan%20Francisco", queryString);
    }
}
