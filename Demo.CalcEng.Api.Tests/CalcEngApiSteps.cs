using Microsoft.VisualStudio.TestTools.UnitTesting;
using Specflow.Steps.WebApi;
using TechTalk.SpecFlow;

namespace Demo.CalcEng.Api.Tests
{
    [Binding]
    [Scope(Feature = "CalcEngApi")]
    public class CalcEngApiSteps : WebApiSpecs
    {
        private static readonly WebApiSpecsConfig _config = new WebApiSpecsConfig { BaseUrl = CalcEngApiHost.BaseUrl };

        public CalcEngApiSteps(TestContext testContext) : base(testContext, _config) { }
    }
}
