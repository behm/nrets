using NRets.Tests.Shared;
using System;
using System.Threading.Tasks;
using Xunit;

namespace NRets.Tests.DevTests
{
    public class RetsDevTests
    {
        [Fact]
        [Trait("Category", TestCategories.Development)]
        public async Task EndToEndProcessing()
        {
            var config = TestHelper.GetApplicationConfiguration(Environment.CurrentDirectory);
            var retsSession = new RetsSession(config.LoginUrl);
            if (await retsSession.LoginAsync(config.Username, config.Password, config.UserAgent))
            {
                // todo: call other session methods

                await retsSession.LogoutAsync();
            }
        }

        [Fact]
        [Trait("Category", TestCategories.Development)]
        public async Task Login_WithValidCredential_ReturnsTrue()
        {
            // Arrange
            var config = TestHelper.GetApplicationConfiguration(Environment.CurrentDirectory);
            var retsSession = new RetsSession(config.LoginUrl);

            // Act
            var result = await retsSession.LoginAsync(config.Username, config.Password, config.UserAgent);

            // Assert
            Assert.True(result);
            Assert.NotNull(retsSession.LoginResponse);
        }

        [Fact(DisplayName = "Unsuccessful login test", Skip = "Should HTTP exeption be handled and converted to bool or let it come")]
        [Trait("Category", TestCategories.Development)]
        public async Task Login_WithInvalidCredentials_Unsuccessful()
        {
            // Arrange
            var config = TestHelper.GetApplicationConfiguration(Environment.CurrentDirectory);
            var retsSession = new RetsSession(config.LoginUrl);

            // Act
            var result = await retsSession.LoginAsync("invalid-username", "invalid-password", "invalid-useragent");

            // Assert
            Assert.False(result);
            Assert.Null(retsSession.LoginResponse);
        }

        [Fact]
        [Trait("Category", TestCategories.Development)]
        public async Task Logout_WithLogin_Succeeds()
        {
            // Arrange
            var config = TestHelper.GetApplicationConfiguration(Environment.CurrentDirectory);
            var retsSession = new RetsSession(config.LoginUrl);

            // Act
            var response = await retsSession.LoginAsync(config.Username, config.Password, config.UserAgent);
            if (response == true)
            {
                await retsSession.LogoutAsync();
            }

            // Assert
            Assert.True(response);
            Assert.Null(retsSession.LoginResponse);
        }

        //[Fact]
        //public async Task GetMetadata_ReturnsMetadata()
        //{
        //    // Arrange
        //    (var loginUrl, var username, var password, var userAgent) = GetLoginInformation();
        //    var retsSession = new RetsSession(loginUrl);

        //    // Act
        //    var success = await retsSession.LoginAsync(username, password, userAgent);
        //    var result = await retsSession.GetMetadata();

        //    // Assert
        //    Assert.NotNull(result);

        //}
    }
}
