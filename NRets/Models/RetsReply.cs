namespace NRets.Models
{
    public class RetsReply
    {
        public string ReplyCode { get; set; }
        public string ReplyText { get; set; }
        public string RetsResponse { get; set; }
        public bool IsSuccess
        {
            get
            {
                return ReplyCode.Equals("0");
            }
        }
    }
}
