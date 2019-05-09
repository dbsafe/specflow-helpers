using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Demo.CalcEng.Api.Tests
{
    [TestClass]
    public class CalcEngApiHost
    {
        public const string BaseUrl = "http://localhost:5000";

        private static IWebHost _webHost;

        [AssemblyInitialize]
#pragma warning disable IDE0060 // Remove unused parameter
        public static void StartHost(TestContext testContext)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            _webHost = WebHost.CreateDefaultBuilder()
                .UseUrls(BaseUrl)
                .UseStartup<Startup>()
                .Build();

            _webHost.Start();
        }

        [TestCleanup]
        public static void StopHost()
        {
            _webHost.StopAsync(TimeSpan.FromSeconds(3)).Wait();
        }
    }
}
