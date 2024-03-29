﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public bool BypassServerCertificateValidation { get; set; }
    }

    public enum WebApiSpecsRequestContentType
    {
        StringContent,
        JObjectContent
    }

    public enum ContentType
    {
        NonContent,
        Unknown,
        Json,
        Text
    }

    public class WebApiSpecsRequest
    {
        public HttpRequestType? RequestType { get; set; }
        public string Url { get; set; }
        public JObject JObjectContent { get; set; }
        public string StringContent { get; set; }
        public IList<KeyValuePair<string, string>> Headers { get; set; }
        public WebApiSpecsRequestContentType ContentType { get; set; }
    }

    public class WebApiSpecs : JObjectBuilderSteps
    {
        private const string EMPTY_MSG_ITEM = "[EMPTY]";
        private const string NULL_MSG_ITEM = "[NULL]";
        private bool _requestSent = false;
        private string _requestStringContent = null;
        private string _responseStringContent = null;
        private ContentType _reponseContentType = ContentType.Unknown;

        private readonly WebApiSpecsConfig _config;

        private readonly List<KeyValuePair<string, string>> _headers = new List<KeyValuePair<string, string>>();

        public HttpResponseMessage HttpResponse { get; private set; }

        public WebApiSpecs(TestContext testContext, WebApiSpecsConfig config)
            : base(testContext)
        {
            _config = config;
            UseNullForMissingProperties = true;
        }

        #region Steps

        [Given(@"content equals to '([\w\W]+)'")]
        public void SetContent(string content)
        {
            ExecuteProtected(() =>
            {
                Assert.IsNull(_requestStringContent, "The content can be assigned only one time");
                _requestStringContent = content;
            });
        }

        [Given(@"header ([^\s]+) equals to '(.*)'")]
        public void SetHttpHeader(string name, string value)
        {
            ExecuteProtected(() =>
            {
                _headers.Add(new KeyValuePair<string, string>(name, value));
            });
        }

        [Given(@"content is the complex-element array")]
        public void SetContentAsArrayWithComplexElements(Table table)
        {
            ExecuteProtected(() => SetContentAsComplexElementArray(table));
        }

        [When(@"I send a (POST|GET|PUT|DELETE|PATCH) request to ([\w\W]+)")]
        public void CreateClientRequest(HttpRequestType requestType, string path)
        {
            ExecuteProtected(() =>
            {
                _requestSent = false;
                var request = CreateRequest();
                request.Url = $"{_config.BaseUrl}/{path}";
                request.RequestType = requestType;
                request.Headers = _headers;
                ValidateRequest(request);
                SendHttpRequestAsync(request).Wait();
            });
        }

        [Then(@"StatusCode should be (\d+)")]
        public void AssertStatusCode(int statusCode)
        {
            ExecuteProtected(() =>
            {
                ValidateHttpResponse();
                Assert.AreEqual(statusCode, (int)HttpResponse.StatusCode, "Unexpected StatusCode");
            });
        }

        [Then(@"ReasonPhrase should be '([\w\W]+)'")]
        public void AssertReasonPhrase(string reasonPhrase)
        {
            ExecuteProtected(() =>
            {
                ValidateHttpResponse();
                Assert.AreEqual(reasonPhrase, HttpResponse.ReasonPhrase, "Unexpected ReasonPhrase");
            });
        }

        [Then(@"header ([^\s]+) should be '(.*)'")]
        public void AssertHeader(string name, string value)
        {
            ExecuteProtected(() =>
            {
                ValidateHttpResponse();

                Assert.IsNotNull(HttpResponse.Headers, "Headers property is null");
                Assert.IsTrue(HttpResponse.Headers.Contains(name), $"Header {name} not found");
                Print($"Header with the name '{name}' found");

                var headerValues = HttpResponse.Headers.GetValues(name);
                Assert.IsTrue(headerValues.Any(header => header == value), $"Header '{name}' was found but the value did not match");
            });
        }

        [Then(@"content header ([^\s]+) should be '(.*)'")]
        public void AssertContentHeader(string name, string value)
        {
            ExecuteProtected(() =>
            {
                ValidateHttpResponse();

                Assert.IsNotNull(HttpResponse.Content, "Content is null");
                Assert.IsNotNull(HttpResponse.Content.Headers, "Content Headers property is null");
                Assert.IsTrue(HttpResponse.Content.Headers.Contains(name), $"Header {name} not found");
                Print($"Header with the name '{name}' found");

                var headerValues = HttpResponse.Content.Headers.GetValues(name);
                Assert.IsTrue(headerValues.Any(header => header == value), $"Header '{name}' was found but the value did not match");
            });
        }

        [Then(@"content should be '(.*)'")]
        public void AssertContentAsText(string expectedContent)
        {
            ExecuteProtected(() =>
            {
                ValidateHttpResponse();
                Assert.IsNotNull(HttpResponse.Content, "Content is null");
                Assert.AreEqual(expectedContent, _responseStringContent);
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

        private void ValidateHttpResponse()
        {
            if (!_requestSent)
            {
                PrintAndThrowException("A request must be sent before validating the response.");
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

        private void ValidateRequest(WebApiSpecsRequest request)
        {
            if (!request.RequestType.HasValue)
            {
                throw new InvalidOperationException("Request type must have a value");
            }

            if (string.IsNullOrWhiteSpace(request.Url))
            {
                Assert.Fail("Path must be specified");
            }
        }

        private static ContentType GetContentType(HttpResponseMessage httpResponse)
        {
            switch (httpResponse.Content?.Headers?.ContentType?.MediaType)
            {
                case null:
                    return ContentType.NonContent;
                case "application/json":
                    return ContentType.Json;
                case "text/plain":
                    return ContentType.Text;
                default:
                    return ContentType.Unknown;
            }
        }

        private void ExtractContentFromHttpResponse()
        {
            _reponseContentType = GetContentType(HttpResponse);
            if (_reponseContentType == ContentType.NonContent)
            {
                return;
            }

            _responseStringContent = HttpResponse.Content.ReadAsStringAsync().Result;
            if (_reponseContentType == ContentType.Json && _responseStringContent != string.Empty)
            {
                ParseJsonContent();
                return;
            }
        }

        private void ParseJsonContent()
        {
            try
            {
                var responseContent = JToken.Parse(_responseStringContent);
                if (IsValueType(responseContent.Type))
                {
                    SetResponseWithValue(responseContent);
                    return;
                }

                switch (responseContent.Type)
                {
                    case JTokenType.Array:
                        SetResponseWithArray(responseContent);
                        return;
                    case JTokenType.Object:
                        SetResponse(responseContent);
                        return;
                    default:
                        throw new InvalidOperationException($"Unexpected Json type in the response content '{responseContent.Type}'");
                }
            }
            catch (Exception ex)
            {
                var message = $"Unable to decode the response.\nContent:\n{_responseStringContent}";
                PrintError(message);
                throw new Exception(message, ex);
            }
        }

        private async Task PrintResponseAsync(HttpResponseMessage response)
        {
            var headers = GetDisplayHeaders(response);
            var contentHeaders = GetDisplayContentHeaders(response);
            var bodyText = await GetDisplayContentAsync(response.Content);
            var text = $@"RESPONSE [HttpResponseMessage]
STATUSCODE: {(int)HttpResponse.StatusCode}
REASONPHRASE: {HttpResponse.ReasonPhrase}
HEADERS:
{headers}
CONTENT-HEADERS:
{contentHeaders}
BODY:
{bodyText}
";
            Print(text);
        }

        private async Task SendHttpRequestAsync(WebApiSpecsRequest request)
        {
            PrintRequest(request);
            var client = new HttpClientEx
            {
                BypassServerCertificateValidation = _config.BypassServerCertificateValidation
            };

            var httpClientExRequest = MapRequest(request);
            HttpResponse = await client.SendRequest(httpClientExRequest);
            _requestSent = true;
            ExtractContentFromHttpResponse();
            await PrintResponseAsync(HttpResponse);
        }

        private static HttpClientExRequest MapRequest(WebApiSpecsRequest webApiSpecsRequest)
        {
            if (webApiSpecsRequest == null)
            {
                throw new ArgumentNullException(nameof(webApiSpecsRequest));
            }

            return new HttpClientExRequest
            {
                Content = MapContent(webApiSpecsRequest),
                Headers = webApiSpecsRequest.Headers,
                RequestType = webApiSpecsRequest.RequestType,
                Url = webApiSpecsRequest.Url
            };
        }

        private static string MapContent(WebApiSpecsRequest webApiSpecsRequest)
        {
            switch (webApiSpecsRequest.ContentType)
            {
                case WebApiSpecsRequestContentType.StringContent:
                    return webApiSpecsRequest.StringContent;
                case WebApiSpecsRequestContentType.JObjectContent:
                    return webApiSpecsRequest.JObjectContent?.ToString();
                default:
                    throw new InvalidOperationException($"Unexpected content type {webApiSpecsRequest.ContentType}");
            }
        }

        private static string GetDisplayHeaders(HttpResponseMessage response)
        {
            if (response == null)
            {
                return NULL_MSG_ITEM;
            }

            var headers = response.Headers;
            var lines = headers.Select(header => $"{header.Key}:{string.Join(",", header.Value)}");
            return $"{string.Join("\n", lines)}";
        }

        private static string GetDisplayContentHeaders(HttpResponseMessage response)
        {
            var headers = response.Content?.Headers;
            if (headers is null)
            {
                return NULL_MSG_ITEM;
            }

            var lines = headers.Select(header => $"{header.Key}:{string.Join(",", header.Value)}");
            return $"{string.Join("\n", lines)}";
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

        private static string GetDisplayContent(WebApiSpecsRequest webApiSpecsRequest)
        {
            var content = MapContent(webApiSpecsRequest);

            if (content == null)
            {
                return NULL_MSG_ITEM;
            }

            return content == string.Empty ? EMPTY_MSG_ITEM : content;
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

        private void PrintRequest(WebApiSpecsRequest webApiSpecsRequest)
        {
            var headers = GetDisplayHeaders(webApiSpecsRequest.Headers);
            var bodyText = GetDisplayContent(webApiSpecsRequest);
            var text = $"REQUEST [WebApiSpecsRequest]\nMETHOD: {webApiSpecsRequest.RequestType}\nURL: {webApiSpecsRequest.Url}\nHEADERS:\n{headers}\nBODY:\n{bodyText}\n";
            Print(text);
        }

        private WebApiSpecsRequest CreateRequest()
        {
            const string message = "The content must be built by assigning properties, a json string value, or an array. Only one of these methods can be used in the scenario.";
            Assert.IsFalse(Request != null && _requestStringContent != null, message);

            if (_requestStringContent != null)
            {
                return new WebApiSpecsRequest { StringContent = _requestStringContent, ContentType = WebApiSpecsRequestContentType.StringContent };
            }
            else
            {
                return new WebApiSpecsRequest { JObjectContent = Request, ContentType = WebApiSpecsRequestContentType.JObjectContent };
            }
        }

        private void SetContentAsComplexElementArray(Table table)
        {
            Assert.IsNull(_requestStringContent, "The content can be assigned only one time");
            var content = CreateJArrayFromTable(table);
            _requestStringContent = content.ToString();
        }
    }
}
