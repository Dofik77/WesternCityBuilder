namespace PdUtils.Web
{
    public struct WebResponse<T>
    {
        public readonly long ResponseCode;
        public T Value;
        public string Error;

        public bool IsSuccess => string.IsNullOrEmpty(Error);

        public WebResponse(long responseCode) : this()
        {
            ResponseCode = responseCode;
        }
    }
}