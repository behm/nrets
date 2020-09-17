using NRets.Models;
using NRets.Parsers;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace NRets
{
    public class RetsSession
    {
        public IRetsHttpClient _retsHttpClient;
        private readonly string _loginUrl;

        public LoginResponse LoginResponse { get; private set; }

        /// <summary>
        /// RetsHttpClient mostly used as a test seam
        /// </summary>
        public IRetsHttpClient RetsHttpClient { get; set; }


        public RetsSession(string loginUrl)
        {
            if (string.IsNullOrWhiteSpace(loginUrl)) { throw new ArgumentNullException(nameof(loginUrl)); }

            _loginUrl = loginUrl;
        }

        public async Task<bool> LoginAsync(string username, string password, string userAgent)
        {
            var httpClient = GetHttpClient(username, password, userAgent);
            _retsHttpClient = RetsHttpClient ?? new RetsHttpClient(httpClient, new ReplyParser(), new ResponseParser());

            LoginResponse = await _retsHttpClient.LoginAsync(new Uri(_loginUrl));

            return LoginResponse != null;
        }

        public async Task LogoutAsync()
        {
            await _retsHttpClient.LogoutAsync();
            LoginResponse = null;
        }

        public async Task<RetsMetadata> GetMetadataAsync()
        {
            return await _retsHttpClient.GetMetadataAsync();
        }

        private HttpClient GetHttpClient(string username, string password, string userAgent)
        {
            var handler = new HttpClientHandler
            {
                // todo: come up with an optional or better way of handling SSL Validation
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; },

                // todo: determine if certain TLS versions are required - make this configurable
                //SslProtocols = System.Security.Authentication.SslProtocols.Tls12;

            };

            var authorizationHeader = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}"));

            var httpClient = new HttpClient(handler);
            httpClient.DefaultRequestHeaders.Add("RETS-Version", "RETS/1.7.2");//RetsVersion.Version_1_7_2.AsHeader());
            if (!string.IsNullOrWhiteSpace(userAgent))
            {
                httpClient.DefaultRequestHeaders.Add("User-Agent", userAgent);
            }
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authorizationHeader);


            return httpClient;
        }
    }
}
