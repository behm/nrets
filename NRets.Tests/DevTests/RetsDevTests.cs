﻿using NRets.Tests.Shared;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace NRets.Tests.DevTests
{
    [Trait("Category", TestCategories.Development)]
    public class RetsDevTests
    {
        private readonly ITestOutputHelper _output;

        public RetsDevTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task EndToEndProcessing()
        {
            var config = TestHelper.GetApplicationConfiguration(Environment.CurrentDirectory);
            var retsSession = new RetsSession(config.LoginUrl);
            if (await retsSession.LoginAsync(config.Username, config.Password, config.UserAgent))
            {
                var metadata = await retsSession.GetMetadataAsync();
                foreach (var resource in metadata.Resources)
                {
                    _output.WriteLine($"Resource: {resource.StandardName} [{resource.VisibleName}]");
                    foreach (var retsClass in resource.Classes)
                    {
                        _output.WriteLine($"Class: {retsClass.StandardName} [{retsClass.VisibleName}]");
                        _output.WriteLine($"Tables:");
                        foreach (var table in retsClass.Tables)
                        {
                            _output.WriteLine("=====================================================================");
                            _output.WriteLine($"Table: Resource-{table.Resource} Class:{table.Class}");
                            _output.WriteLine("=====================================================================");
                            foreach (var field in table.Fields)
                            {
                                _output.WriteLine($"{field.StandardName,40} ({field.DataType}:{field.Precision})");
                            }
                        }
                    }
                }

                // todo: call other session methods

                await retsSession.LogoutAsync();
            }
        }

        [Fact]
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
