using System;

namespace PdUtils.Web
{
    public interface IWebRequester<T>
    {
        void Get(string url, Action<WebResponse<T>> callback);
        void Get(string url, Action<WebResponse<string>> callback);
    }
}