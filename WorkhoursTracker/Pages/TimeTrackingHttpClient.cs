using AntDesign;
using System.Net;
using System.Net.Http.Json;
using WorkhoursTracker.Helper;

namespace TimeTracking.Pages
{
    public class TimeTrackingHttpClient : HttpClient
    {
        private readonly AuthStateProvider authStateProvider;
        private readonly MessageService _message;

        public TimeTrackingHttpClient(
            IConfiguration configuration,
            AuthStateProvider authStateProvider,
            MessageService message)
        {
            //BaseAddress = new Uri("http://example.com/api/");
            this.authStateProvider = authStateProvider;
            _message = message;
        }

        internal HttpRequestMessage CreateRequest(HttpMethod method, string requestUri, string token)
        {
            var request = new HttpRequestMessage(method, requestUri);

            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Connection", "keep-alive");

            if (token != null)
                request.Headers.Add("Authorization", $"Bearer {token}");

            return request;
        }

        public async Task<T?> GetResponse<T>(HttpMethod method, string requestUri, object requestBody = null)
        {
            var token = await authStateProvider.GetTokenAsync();
            var request = RequestHelper.CreateRequest(method, requestUri, token);

            if (method == HttpMethod.Post && requestBody != null)
                request.ApppendBody(requestBody);

            var response = await SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    var timeResponseContent = await response.Content.ReadFromJsonAsync<T>();
                    if (timeResponseContent != null)
                        return timeResponseContent;
                    else
                        await _message.Warning("Response is empty.");
                }
                catch (Exception ex)
                {
                    await _message.Warning("Could not read result.");
                }
                finally
                {
                    await _message.Success("Completed successfully.");
                }
            }
            else
                await _message.Error("There was an error in sending request.");

            return default;
        }
    }
}