using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Specflow.Steps.WebApi.HttpClient
{
    public enum HttpRequestType
    {
        GET,
        POST,
        PUT,
        DELETE
    }

    public class HttpClientExRequest
    {
        public HttpRequestType? RequestType { get; set; }
        public string Url { get; set; }
        public string Content { get; set; }
        public IList<KeyValuePair<string, string>> Headers { get; set; }
    }

    public class HttpClientEx
    {
        public async Task<HttpResponseMessage> SendRequest(HttpClientExRequest request)
        {
            switch (request.RequestType)
            {
                case HttpRequestType.POST:
                    return await PostAsync(request);

                case HttpRequestType.GET:
                    return await GetAsync(request);

                case HttpRequestType.DELETE:
                    return await DeleteAsync(request);

                case HttpRequestType.PUT:
                    return await PutAsync(request);
                default:
                    throw new InvalidOperationException($"RequestType '{request.RequestType}' not supported");
            }
        }

        private Task<HttpResponseMessage> PostAsync(HttpClientExRequest request)
        {
            return ExecuteRequest(async (client) =>
            {
                var httpContent = GetHttpContent(request);
                SetHeaders(request, client);
                return await client.PostAsync(request.Url, httpContent);
            });
        }

        private Task<HttpResponseMessage> PutAsync(HttpClientExRequest request)
        {
            return ExecuteRequest(async (client) =>
            {
                var httpContent = GetHttpContent(request);
                SetHeaders(request, client);
                return await client.PutAsync(request.Url, httpContent);
            });
        }
        
        private Task<HttpResponseMessage> DeleteAsync(HttpClientExRequest request)
        {
            return ExecuteRequest(async (client) =>
            {
                SetHeaders(request, client);
                return await client.DeleteAsync(request.Url);
            });
        }

        private Task<HttpResponseMessage> GetAsync(HttpClientExRequest request)
        {
            return ExecuteRequest(async (client) =>
            {
                SetHeaders(request, client);
                return await client.GetAsync(request.Url);
            });
        }

        private StringContent GetHttpContent(HttpClientExRequest request)
        {
            if (request.Content == null)
            {
                return null;
            }

            string contentAsText = request.Content.ToString();
            return new StringContent(contentAsText, null, "application/json");
        }

        private void SetHeaders(HttpClientExRequest request, System.Net.Http.HttpClient client)
        {
            if (request.Headers == null)
            {
                return;
            }

            foreach (var header in request.Headers)
            {
                client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }

        private async Task<HttpResponseMessage> ExecuteRequest(Func<System.Net.Http.HttpClient, Task<HttpResponseMessage>> func)
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                return await func(client);
            }
        }
    }
}
