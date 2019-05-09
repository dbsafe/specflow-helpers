using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Specflow.Steps.Object;
using Specflow.Steps.WebApi.HttpClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Specflow.Steps.WebApi
{
    public class WebApiSpecsConfig
    {
        public string BaseUrl { get; set; }
    }

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
        public JObject Content { get; set; }
        public IList<KeyValuePair<string, string>> Headers { get; set; }
    }

    [Binding]
    public class WebApiSpecs : JObjectBuilderSteps
    {
        private const string EMPTY_MSG_ITEM = "[EMPTY]";
        private const string NULL_MSG_ITEM = "[NULL]";
        private bool _requestSent = false;

        private WebApiSpecsConfig _config;
        private HttpClientExRequest _httpRequest;
        private Dictionary<string, string> _responseHeaders;

        public HttpResponseMessage HttpResponse { get; private set; }

        public WebApiSpecs(TestContext testContext, WebApiSpecsConfig config)
            : base(testContext)
        {
            _config = config;
        }

        #region Steps

        [When(@"I send a (POST|GET|PUT|DELETE) request to ([\w\W]+)")]
        public void CreateClientRequest(HttpRequestType requestType, string path)
        {
            ExecuteProtected(() =>
            {
                var request = GetCurrentRequest();
                request.Url = $"{_config.BaseUrl}/{path}";
                request.RequestType = requestType;
                ValidateRequest();
                SendHttpRequestAsync().Wait();
            });
        }

        #endregion

        protected override void ValidateResponse()
        {
            ValidateHttpResponse();
            Assert.IsNotNull(Response, "The http response does not have data.");
        }

        protected virtual void PrintError(string message)
        {
            Print($"*******************    ERROR    *******************\n{message}");
        }

        protected virtual void PrintWarning(string message)
        {
            Print($"*******************    WARN    *******************\n{message}");
        }

        private void ValidateHttpResponse()
        {
            if (!_requestSent)
            {
                PrintAndThrowException("A request to the server needs to be sent first.");
            }

            if (HttpResponse == null)
            {
                PrintAndThrowException($"The {nameof(HttpResponse)} is not assigned");
            }
        }

        private void PrintAndThrowException(string message)
        {
            PrintError(message);
            throw new Exception(message);
        }

        private HttpClientExRequest GetCurrentRequest()
        {
            if (_httpRequest == null)
            {
                _httpRequest = new HttpClientExRequest { Content = Request };
            }

            return _httpRequest;
        }

        private void ValidateRequest()
        {
            var request = GetCurrentRequest();

            if (!request.RequestType.HasValue)
            {
                throw new InvalidOperationException("Request type must have a value");
            }

            if ((request.RequestType.Value == HttpRequestType.GET || request.RequestType.Value == HttpRequestType.DELETE) &&
                (request.Content != null && request.Content.HasValues))
            {
                PrintWarning("Requests of type GET/DELETE cannot have a content");
            }

            if (string.IsNullOrWhiteSpace(request.Url))
            {
                Assert.Fail("Path must be specified");
            }
        }

        private void ExtractHeadersFromHttpResponse()
        {
            _responseHeaders = new Dictionary<string, string>();
            if (HttpResponse.Headers != null)
            {
                foreach (var header in HttpResponse.Headers)
                {
                    foreach (var headervalue in header.Value)
                    {
                        _responseHeaders[header.Key] = headervalue;
                    }
                }
            }
        }

        private void ExtractContentFromHttpResponse()
        {
            if (HttpResponse.Content == null)
            {
                return;
            }

            var content = HttpResponse.Content.ReadAsStringAsync().Result;
            if (content != string.Empty)
            {
                try
                {
                    var responseContent = JObject.Parse(content);
                    SetResponse(responseContent);
                }
                catch (Exception ex)
                {
                    var message = $"Unable to decode the response.";
                    PrintError(message);
                    throw new Exception(message, ex);
                }
            }
        }

        private async Task PrintResponseAsync()
        {
            if (HttpResponse == null)
            {
                Print($"RESPONSE: {NULL_MSG_ITEM}");
                return;
            }

            var headers = GetDisplayHeaders(_responseHeaders);
            var bodyText = await GetDisplayContentAsync(HttpResponse.Content);
            var text = $"RESPONSE\nSTATUSCODE: {(int)HttpResponse.StatusCode}\nREASONPHRASE: {HttpResponse.ReasonPhrase}\nHEADERS:\n{headers}\nBODY:\n{bodyText}";
            Print(text);
        }

        private async Task SendHttpRequestAsync()
        {
            PrintRequest();
            var client = new HttpClientEx();
            HttpResponse = await client.SendRequest(_httpRequest);
            ExtractHeadersFromHttpResponse();
            ExtractContentFromHttpResponse();
            await PrintResponseAsync();
        }

        private static string GetDisplayHeaders(IEnumerable<KeyValuePair<string, string>> headers)
        {
            if (headers == null)
            {
                return NULL_MSG_ITEM;
            }

            if (headers.Count() == 0)
            {
                return EMPTY_MSG_ITEM;
            }

            var lines = headers.Select(header => $"{header.Key}:{header.Value}");
            return $"{string.Join("\n", lines)}";
        }

        private static string GetDisplayContent(HttpClientExRequest request)
        {
            if (request.Content == null)
            {
                return NULL_MSG_ITEM;
            }

            var text = request.Content.ToString();
            return text == string.Empty ? EMPTY_MSG_ITEM : text;
        }

        private static async Task<string> GetDisplayContentAsync(HttpContent content)
        {
            if (content == null)
            {
                return NULL_MSG_ITEM;
            }

            var text = await content.ReadAsStringAsync();
            return text == string.Empty ? EMPTY_MSG_ITEM : text;
        }

        private void PrintRequest()
        {
            var request = _httpRequest;
            var headers = GetDisplayHeaders(request.Headers);
            var bodyText = GetDisplayContent(request);
            var text = $"REQUEST\nMETHOD: {request.RequestType}\nURL: {request.Url}\nHEADERS:\n{headers}\nBODY:\n{bodyText}\n";
            Print(text);
        }
    }
}
