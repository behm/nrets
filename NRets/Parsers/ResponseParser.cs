using NRets.Models;
using System;

namespace NRets.Parsers
{
    public interface IResponseParser
    {
        LoginResponse ParseLoginResponse(string responseText);
    }

    // NOTE: Reference to RETS spec can be found here - https://www.rapattoni.com/rapdocs/support/mls/retsdocs/RETS_1_7_2.pdf

    public class ResponseParser : IResponseParser
    {
        public LoginResponse ParseLoginResponse(string responseText)
        {
            if (string.IsNullOrWhiteSpace(responseText))
            {
                throw new ArgumentNullException(nameof(responseText));
            }

            var loginResponse = new LoginResponse();

            var lines = responseText.Split(Environment.NewLine.ToCharArray());
            foreach (var line in lines)
            {
                var parts = line.Split("=".ToCharArray());
                if (parts.Length < 2)
                {
                    continue;
                }

                loginResponse.ResponseItems.Add(parts[0], parts[1]);
            }

            return loginResponse;
        }
    }
}
