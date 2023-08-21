using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace WorkhoursTracker.Helper
{
    public static class RequestHelper
    {
        public static HttpRequestMessage CreateRequest(HttpMethod method, string requestUri, string? token = null)
        {
            var request = new HttpRequestMessage(method, requestUri);

            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Connection", "keep-alive");

            if (token != null)
                request.Headers.Add("Authorization", $"Bearer {token}");

            return request;
        }

        public static HttpRequestMessage ApppendBody(this HttpRequestMessage request, object requestBody)
        {
            string json = JsonConvert.SerializeObject(requestBody);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return request;
        }
    }
}