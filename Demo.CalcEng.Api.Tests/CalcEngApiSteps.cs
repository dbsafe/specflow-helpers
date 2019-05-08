using Microsoft.VisualStudio.TestTools.UnitTesting;
using Specflow.Steps.WebApi;
using TechTalk.SpecFlow;

namespace Demo.CalcEng.Api.Tests
{
    [Binding]
    [Scope(Feature = "CalcEngApi")]
    public class CalcEngApiSteps : WebApiSpecs
    {
        private static WebApiSpecsConfig _config;

        static CalcEngApiSteps()
        {
            _config = new WebApiSpecsConfig
            {
                BaseUrl = "http://localhost:5000"
            };
        }

        public CalcEngApiSteps(TestContext testContext) : base(testContext, _config) { }
    }
}
