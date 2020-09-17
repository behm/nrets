using NRets.Parsers;
using NRets.Tests.Shared;
using System;
using System.Linq;
using System.Text;
using Xunit;

namespace NRets.Tests.UnitTests
{
    public class ResponseParserTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void ParseLoginResponse_NullOrWhitespaceReplyText_ThrowsArgumentNullException(string responseText)
        {
            // Arrange
            var responseParser = new ResponseParser();

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => responseParser.ParseLoginResponse(responseText));
        }

        [Fact]
        public void ParseLoginResponse_GivenResponseText_ParsesSuccessfully()
        {
            // Arrange
            var responseText = new StringBuilder();
            responseText.AppendFormat($"MemberName=RETS Vendor Brian Behm{Environment.NewLine}");
            responseText.AppendFormat($"User=225,0,2,2058{Environment.NewLine}");
            responseText.AppendFormat($"Broker=412,412{Environment.NewLine}");
            responseText.AppendFormat($"MetadataVersion=19.2.2548{Environment.NewLine}");
            responseText.AppendFormat($"MetadataTimestamp=2019-02-01T18:28:27.3Z{Environment.NewLine}");
            responseText.AppendFormat($"MinMetadataTimestamp=2019-02-01T18:28:27.3Z{Environment.NewLine}");
            responseText.AppendFormat($"TimeoutSeconds=32400{Environment.NewLine}");
            responseText.AppendFormat($"GetMetadata=/rets/fnisrets.aspx/RASE/getmetadata{Environment.NewLine}");
            responseText.AppendFormat($"GetObject=/rets/fnisrets.aspx/RASE/getobject{Environment.NewLine}");
            responseText.AppendFormat($"Login=https://rase.rets.paragonrels.com/rets/fnisrets.aspx/RASE/login{Environment.NewLine}");
            responseText.AppendFormat($"Logout=/rets/fnisrets.aspx/RASE/logout{Environment.NewLine}");
            responseText.AppendFormat($"Search=/rets/fnisrets.aspx/RASE/search{Environment.NewLine}");

            var responseParser = new ResponseParser();

            // Act
            var result = responseParser.ParseLoginResponse(responseText.ToString());

            // Assert
            Assert.Equal("RETS Vendor Brian Behm", result.MemberName);
            Assert.Equal("225,0,2,2058", result.User);
            Assert.Equal("412,412", result.Broker);
            Assert.Equal("19.2.2548", result.MetadataVersion);
            Assert.Equal("2019-02-01T18:28:27.3Z", result.MetadataTimestamp);
            Assert.Equal("2019-02-01T18:28:27.3Z", result.MinMetadataTimestamp);
            Assert.Equal("32400", result.TimeoutSeconds);
            Assert.Equal("/rets/fnisrets.aspx/RASE/getmetadata", result.GetMetadataUrlPath);
            Assert.Equal("/rets/fnisrets.aspx/RASE/getobject", result.GetObjectUrlPath);
            Assert.Equal("https://rase.rets.paragonrels.com/rets/fnisrets.aspx/RASE/login", result.LoginUrl);
            Assert.Equal("/rets/fnisrets.aspx/RASE/logout", result.LogoutUrlPath);
            Assert.Equal("/rets/fnisrets.aspx/RASE/search", result.SearchUrlPath);
        }

        [Fact]
        public void ParseMetadata_GivenMetadataSystemReponseText_ParsesSuccessfully()
        {
            // Arrange
            var responseText = TestHelper.GetEmbeddedResourceText("metadata-system-response.txt");
            var responseParser = new ResponseParser();

            // Act
            var result = responseParser.ParseMetadataSystemResponse(responseText);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.MetadataVersion);
            Assert.NotNull(result.MetadataTimestamp);
            Assert.Equal("19.2.2548", result.MetadataVersion);
            Assert.Equal("2019-02-01T18:28:27.3Z", result.MetadataTimestamp);
            Assert.Equal(13, result.Resources.Count());
            Assert.Equal(34, result.Resources.Sum(x => x.Classes.Count()));
        }
    }
}
