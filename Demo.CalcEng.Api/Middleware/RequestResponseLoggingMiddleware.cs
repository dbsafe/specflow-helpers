using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.CalcEng.Api.Middleware
{
    public static class RequestResponseLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestResponseLoggingMiddleware>();
        }
    }

    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private const string NULL_MSG_ITEM = "[NULL]";
        private const string EMPTY_MSG_ITEM = "[EMPTY]";

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<RequestResponseLoggingMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            var requestLog = await FormatRequest(context.Request);
            _logger.LogInformation($"\n{requestLog}");

            var originalBodyStream = context.Response.Body;
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await _next(context);

                var responseLog = await FormatResponse(context.Response);
                _logger.LogInformation($"\n{responseLog}");
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private static async Task<string> FormatRequest(HttpRequest request)
        {
            var bodyText = await GetDisplayContentAsync(request);
            var url = UriHelper.GetDisplayUrl(request);
            var headers = GetDisplayHeaders(request.Headers);
            return $"REQUEST [HttpRequest]\nMETHOD: {request.Method}\nURL: {url}\nHEADERS:\n{headers}\nBODY:\n{bodyText}\n";
        }

        private static async Task<string> FormatResponse(HttpResponse response)
        {
            var bodyText = await GetDisplayContentAsync(response);
            var headers = GetDisplayHeaders(response.Headers);
            return $"RESPONSE [HttpResponse]\nSTATUS CODE:{response.StatusCode}\nHEADERS:\n{headers}\nBODY:\n{bodyText}\n";
        }

        private static async Task<string> GetDisplayContentAsync(HttpResponse response)
        {
            if (response.Body == null)
            {
                return NULL_MSG_ITEM;
            }

            response.Body.Seek(0, SeekOrigin.Begin);
            var bodyText = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return bodyText == string.Empty ? EMPTY_MSG_ITEM : bodyText;
        }

        private static async Task<string> GetDisplayContentAsync(HttpRequest request)
        {
            if (request.Body == null)
            {
                return NULL_MSG_ITEM;
            }

            var bodyStream = new MemoryStream();
            await request.Body.CopyToAsync(bodyStream);
            bodyStream.Seek(0, SeekOrigin.Begin);
            var bodyText = new StreamReader(bodyStream).ReadToEnd();
            bodyStream.Seek(0, SeekOrigin.Begin);

            request.Body.Dispose();
            request.Body = bodyStream;

            return bodyText == string.Empty ? EMPTY_MSG_ITEM : bodyText;
        }

        private static string GetDisplayHeaders(IHeaderDictionary headers)
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
    }
}
