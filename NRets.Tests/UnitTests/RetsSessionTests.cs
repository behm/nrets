using Moq;
using NRets.Models;
using System;
using System.Threading.Tasks;
using Xunit;

namespace NRets.Tests.UnitTests
{
    public class RetsSessionTests
    {
        [Fact]
        public void Ctor_NullLoginUrl_ThrowsArgumentNullException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => new RetsSession(null));
        }

        [Fact]
        public async Task Login_WithValidCredentials_ReturnsTrue()
        {
            // Arrange
            var mockRetsHttpClient = new Mock<IRetsHttpClient>();
            mockRetsHttpClient
                .Setup(m => m.LoginAsync(It.IsAny<Uri>()))
                .ReturnsAsync(new LoginResponse());

            var retsSession = new RetsSession("http://retsserver/login")
            {
                // Use this test seam to mock the HttpClient
                RetsHttpClient = mockRetsHttpClient.Object
            };

            // Act
            var result = await retsSession.LoginAsync("username", "password", "useragent");

            // Assert
            Assert.True(result);
            Assert.NotNull(retsSession.LoginResponse);
        }

        [Fact]
        public async Task Login_WithInvalidCredentials_ReturnsFalse()
        {
            // Arrange
            var mockRetsHttpClient = new Mock<IRetsHttpClient>();
            mockRetsHttpClient
                .Setup(m => m.LoginAsync(It.IsAny<Uri>()))
                .Returns(Task.FromResult<LoginResponse>(null));

            var retsSession = new RetsSession("https://retsserver/login")
            {
                // Use this test seam to mock the HttpClient
                RetsHttpClient = mockRetsHttpClient.Object
            };

            // Act
            var result = await retsSession.LoginAsync("invalidusername", "invalidpassword", "invaliduseragent");

            // Assert
            Assert.False(result);
            Assert.Null(retsSession.LoginResponse);
        }

        [Fact]
        public async Task Logout_WithLogin_Succeeds()
        {
            // Arrange
            var mockRetsHttpClient = new Mock<IRetsHttpClient>();
            mockRetsHttpClient
                .Setup(m => m.LoginAsync(It.IsAny<Uri>()))
                .ReturnsAsync(new LoginResponse());
            mockRetsHttpClient
                .Setup(m => m.LogoutAsync());
            var retsSession = new RetsSession("https://retsserver/login")
            {
                RetsHttpClient = mockRetsHttpClient.Object
            };

            // Act
            var response = await retsSession.LoginAsync("username","password","useragent");
            if (response == true)
            {
                await retsSession.LogoutAsync();
            }

            // Assert
            Assert.True(response);
            Assert.Null(retsSession.LoginResponse);
        }
    }
}
