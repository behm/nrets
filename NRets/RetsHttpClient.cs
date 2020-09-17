using NRets.Models;
using NRets.Parsers;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NRets
{
    public interface IRetsHttpClient
    {
        Task<LoginResponse> LoginAsync(Uri loginUri);
        Task LogoutAsync();
        Task<RetsMetadata> GetMetadataAsync();
    }

    public class RetsHttpClient : IRetsHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly IReplyParser _replyParser;
        private readonly IResponseParser _responseParser;

        private LoginResponse _loginResponse;

        public RetsHttpClient(HttpClient httpClient, IReplyParser replyParser, IResponseParser responseParser)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _replyParser = replyParser ?? throw new ArgumentNullException(nameof(replyParser));
            _responseParser = responseParser ?? throw new ArgumentNullException(nameof(responseParser));
        }

        public async Task<LoginResponse> LoginAsync(Uri loginUri)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, loginUri))
            {
                using (var response = await _httpClient.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();

                    // Parse the response
                    var retsReplyText = await response.Content.ReadAsStringAsync();

                    var retsReply = _replyParser.Parse(retsReplyText);

                    if (retsReply.IsSuccess)
                    {
                        _loginResponse = _responseParser.ParseLoginResponse(retsReply.RetsResponse);

                        return _loginResponse;
                    }

                    return null;
                }
            }
        }

        public async Task LogoutAsync()
        {
            if (_loginResponse == null)
            {
                return;
            }

            using (var request = new HttpRequestMessage(HttpMethod.Get, new Uri(_loginResponse.CapabilitiesBaseUri, _loginResponse.LogoutUrlPath)))
            {
                using (var response = await _httpClient.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();

                    // Parse the response
                    var retsReplyText = await response.Content.ReadAsStringAsync();

                    var retsReply = _replyParser.Parse(retsReplyText);
                }
            }
        }

        public async Task<RetsMetadata> GetMetadataAsync()
        {
            if (_loginResponse == null)
            {
                return null;
            }

            // Construct the URL
            //var parameters = new Dictionary<string, string>();
            //parameters.Add("Type", "METADATA-SYSTEM");
            //parameters.Add("ID", "*");
            //parameters.Add("Format", "STANDARD-XML");
            //parameters.Values.Select(x => $"{x.}")
            var getMetadataUri = new UriBuilder(_loginResponse.CapabilitiesBaseUri)
            {
                Path = _loginResponse.GetMetadataUrlPath,
                Query = "Type=METADATA-SYSTEM&ID=*&Format=STANDARD-XML",    // todo: programatically construct this based on metadata options that are/will be passed in
            }.Uri;

            using (var request = new HttpRequestMessage(HttpMethod.Get, getMetadataUri))
            {
                using (var response = await _httpClient.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();

                    // Parse the response
                    var retsReplyText = await response.Content.ReadAsStringAsync();

                    var retsReply = _replyParser.Parse(retsReplyText);

                    // todo: parse the metadata response
                    if (retsReply.IsSuccess)
                    {
                        var retsMetadata = _responseParser.ParseMetadataSystemResponse(retsReplyText);
                        return retsMetadata;
                    }

                    return null;
                }
            }
        }
    }
}
