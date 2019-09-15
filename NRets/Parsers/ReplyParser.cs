using NRets.Models;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace NRets.Parsers
{
    public interface IReplyParser
    {
        RetsReply Parse(string replyText);
    }

    public class ReplyParser : IReplyParser
    {
        public RetsReply Parse(string replyText)
        {
            var textReader = new StringReader(replyText);
            var document = XDocument.Load(textReader);
            var ns = document.Root.GetDefaultNamespace();
            var root = document.Root;
            var retsResponse = root.Descendants(ns + "RETS-RESPONSE").FirstOrDefault();

            return new RetsReply
            {
                ReplyCode = root.Attribute("ReplyCode")?.Value,
                ReplyText = root.Attribute("ReplyText")?.Value,
                RetsResponse = retsResponse?.Value
            };
        }
    }
}
