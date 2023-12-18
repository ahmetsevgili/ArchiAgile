namespace ArchiAgile.Client.Util
{
    public class HttpClientHelperResult<TResponse>
    {
        public bool IsSuccess { get; set; }
        public TResponse Response { get; set; }
        public string ErrorCode { get; set; }
    }
}
