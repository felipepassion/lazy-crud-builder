using Lazy.Crud.Core.Application.DTO.Http.Models.CommonAgg.Commands.Responses;
using Lazy.Crud.CrossCuting.Infra.Utils.Extensions;

namespace CrossCuting.Infra.Utils.Tests
{
    public class StringConversionTests
    {
        [Fact]
        public void Test1()
        {
            // Arrange
            var minValue = "{ \"statusCode\": 400,  \"errors\": [    \"DuplicateUserName: Username 'felipepassion@gmail.com' is already taken.\",    \"DuplicateEmail: Email 'felipepassion@gmail.com' is already taken.\"  ]}";

            // Act
            var result = minValue.TryDeserializeJSON<GetHttpResponseDTO>(out var val);

            // Assert
            Assert.True(result);
            Assert.NotNull(val);
        }
    }
}