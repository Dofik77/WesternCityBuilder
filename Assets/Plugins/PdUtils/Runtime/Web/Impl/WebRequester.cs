using System;
using Newtonsoft.Json;
using UnityEngine.Networking;

namespace PdUtils.Web.Impl
{
    public class WebRequester<T> : IWebRequester<T>
    {
        public void Get(string url, Action<WebResponse<T>> callback)
        {
            var request = UnityWebRequest.Get(url);
            var async = request.SendWebRequest();
            async.completed += operation =>
            {
                var response = new WebResponse<T>(request.responseCode);   
                if (request.result == UnityWebRequest.Result.ProtocolError || request.result == UnityWebRequest.Result.ConnectionError)
                {
                    response.Error = request.error;
                    callback?.Invoke(response);
                }
                else
                {
                    var body = request.downloadHandler.text;
                    if (!string.IsNullOrEmpty(body))
                    {
                        var result = JsonConvert.DeserializeObject<T>(body);
                        response.Value = result;
                    }
                    callback?.Invoke(response);
                }
            };
        }

        public void Get(string url, Action<WebResponse<string>> callback)
        {
            var request = UnityWebRequest.Get(url);
            var async = request.SendWebRequest();
            async.completed += operation =>
            {
                var response = new WebResponse<string>(request.responseCode);   
                if (request.result == UnityWebRequest.Result.ProtocolError || request.result == UnityWebRequest.Result.ConnectionError)
                {
                    response.Error = request.error;
                    callback?.Invoke(response);
                }
                else
                {
                    response.Value = request.downloadHandler.text;
                    callback?.Invoke(response);
                }
            };
        }
    }
}