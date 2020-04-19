using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using TechTalk.SpecFlow;

namespace Demo.CalcEng.Api.Tests
{
    [Binding]
    public static class CalcEngApiHost
    {
        public const string BaseUrl = "http://localhost:5000";

        private static IWebHost _webHost;

        [BeforeTestRun]
        public static void StartHost()
        {
            _webHost = WebHost.CreateDefaultBuilder()
                .UseUrls(BaseUrl)
                .UseStartup<Startup>()
                .Build();

            _webHost.Start();
        }

        [AfterTestRun]
        public static void StopHost()
        {
            _webHost.StopAsync(TimeSpan.FromSeconds(3)).Wait();
        }
    }
}
