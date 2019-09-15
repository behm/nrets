using System;
using System.Collections.Generic;

namespace NRets.Models
{
    public class LoginResponse
    {
        // todo: verify whether key values are constant or if they vary between systems

        public Dictionary<string, string> ResponseItems { get; private set; } = new Dictionary<string, string>();

        public string MemberName { get { return ResponseItems.TryGetValue("MemberName", out var value) ? value : null; } }
        public string User { get { return ResponseItems.TryGetValue("User", out var value) ? value : null; } }
        public string Broker { get { return ResponseItems.TryGetValue("Broker", out var value) ? value : null; } }
        public string MetadataVersion { get { return ResponseItems.TryGetValue("MetadataVersion", out var value) ? value : null; } }
        public string MetadataTimestamp { get { return ResponseItems.TryGetValue("MetadataTimestamp", out var value) ? value : null; } }
        public string MinMetadataTimestamp { get { return ResponseItems.TryGetValue("MinMetadataTimestamp", out var value) ? value : null; } }
        public string TimeoutSeconds { get { return ResponseItems.TryGetValue("TimeoutSeconds", out var value) ? value : null; } }
        public string PasswordExpiration { get { return ResponseItems.TryGetValue("Expr", out var value) ? value : null; } }
        public string OfficeList { get { return ResponseItems.TryGetValue("OfficeList", out var value) ? value : null; } }
        public string GetMetadataUrlPath { get { return ResponseItems.TryGetValue("GetMetadata", out var value) ? value : null; } }
        public string GetObjectUrlPath { get { return ResponseItems.TryGetValue("GetObject", out var value) ? value : null; } }
        public string LoginUrl { get { return ResponseItems.TryGetValue("Login", out var value) ? value : null; } }
        public string LogoutUrlPath { get { return ResponseItems.TryGetValue("Logout", out var value) ? value : null; } }
        public string SearchUrlPath { get { return ResponseItems.TryGetValue("Search", out var value) ? value : null; } }
        public string ActionUrlPath { get { return ResponseItems.TryGetValue("Action", out var value) ? value : null; } }
        public string ChangePasswordUrlPath { get { return ResponseItems.TryGetValue("ChangePassword", out var value) ? value : null; } }
        public string LoginCompleteUrlPath { get { return ResponseItems.TryGetValue("LoginComplete", out var value) ? value : null; } }
        public string ServerInformationUrlPath { get { return ResponseItems.TryGetValue("ServerInformation", out var value) ? value : null; } }
        public string UpdateUrlPath { get { return ResponseItems.TryGetValue("Update", out var value) ? value : null; } }
        public string Balance { get { return ResponseItems.TryGetValue("Balance", out var value) ? value : null; } }

        /// <summary>
        /// The base URI to be used with path properties (i.e. GetMetadataUrlPath, GetObjectUrlPath, etc.)
        /// </summary>
        public Uri CapabilitiesBaseUri
        {
            get
            {
                if (string.IsNullOrWhiteSpace(LoginUrl))
                {
                    return null;
                }

                var loginUri = new Uri(LoginUrl).GetLeftPart(UriPartial.Authority);
                return new Uri(loginUri);
            }
        }
    }
}
