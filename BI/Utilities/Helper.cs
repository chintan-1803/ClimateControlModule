using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BI.Utilities
{
    public class Helper
    {
        public async Task<HttpResponseMessage> SendRequestToExternalApi(Method method, string requestUrl)
        {
            HttpClient _httpClient = new HttpClient();

            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) " + "AppleWebKit/537.36 (KHTML, like Gecko) " + "Chrome/115.0.0.0 Safari/537.36");

            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var url = requestUrl;

            switch (method)
            {
                case Method.Get:
                    return await _httpClient.GetAsync(url);
                //case Method.Post:  Chintan: Uncomment and modify the following lines if you need to support POST, PUT, DELETE methods
                //    return await _httpClient.PostAsync(url, null); // Replace null with actual content if needed
                //case Method.Put:
                //    return await _httpClient.PutAsync(url, null); // Replace null with actual content if needed
                //case Method.Delete:
                //    return await _httpClient.DeleteAsync(url);
                default:
                    throw new NotSupportedException($"Method {method} is not supported.");
            }
        }
    }


    public enum Method
    {
        Get = 0,
        Post = 1,
        Put = 2,
        Delete = 3 
    }
}