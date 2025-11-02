using Niu.Nutri.CrossCuting.Infra.Utils.Extensions;

namespace CrossCuting.Infra.Utils.Tests
{
    public class StringFormatTests
    {
        [Theory]
        [InlineData("-10")]
        [InlineData("-1000")]
        [InlineData("-160")]
        [InlineData("-120")]
        [InlineData("-1255650")]
        public void Test1(string testStr)
        {
            // Arrange
            var minValue = 0;

            // Act
            var result = testStr.FormatNumber(min: minValue);

            // Assert
            Assert.Equal(minValue.ToString(), result);
        }

        [Theory]
        [InlineData("-40")]
        [InlineData("-60")]
        [InlineData("-40650")]
        public void Test2(string testStr)
        {
            // Arrange
            var minValue = -20;

            // Act
            var result = testStr.FormatNumber(min: minValue);

            // Assert
            Assert.Equal(minValue.ToString(), result);
        }

        [Theory]
        [InlineData("-10-")]
        [InlineData("-10-10")]
        [InlineData("-10e")]
        [InlineData("e10")]
        [InlineData("e")]
        [InlineData("+10")]
        [InlineData("+")]
        public void Test3(string testStr)
        {
            // Arrange
            var expectedResultAtEnd = string.Empty;

            // Act
            var result = testStr.FormatNumber(min: 100);

            // Assert
            Assert.Equal(expectedResultAtEnd, result);
        }

        [Theory]
        [InlineData("abcdef")]
        [InlineData("ASDSADAS ")]
        [InlineData("-asdad sa")]
        public void Test4(string testStr)
        {
            // Arrange
            var expectedResult = string.Empty;

            // Act
            var result = testStr.FormatNumber();

            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
}