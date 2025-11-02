using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Niu.Nutri.Core.Application.DTO.Aggregates.CommonAgg.Models.Auth;
using Niu.Nutri.CrossCuting.Infra.Utils.Extensions;
using Niu.Nutri.CrossCutting.Infra.Log.Contexts;
using Niu.Nutri.Users.Domain.Aggregates.UsersAgg.Repositories;
using Xunit;
namespace Core.Tests
{
    public class ObjectExtensionsTests
    {
        class ObjectWithByteArray
        {
            public byte[]? ByteArray { get; set; }
        }

        [Fact]
        public void Should_Update_Byte_Array()
        {
            var obj1 = new ObjectWithByteArray
            {
                ByteArray = new byte[] { 1, 2, 3 }
            };

            var obj2 = new ObjectWithByteArray
            {
                ByteArray = new byte[] { 4, 5, 6 }
            };

            // Act
            obj1.Update(obj2);

            // Assert
            Assert.Equal(obj2.ByteArray, obj1.ByteArray);
        }
    }
}