using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Specflow.Steps.Object;
using TechTalk.SpecFlow;

namespace Demo.CalcEng.Domain.Tests
{
    [Binding]
    [Scope(Feature = "CalcEng")]
    public class CalcEngSteps : JObjectBuilderSteps
    {
        private readonly CalcEng _calcEng = new CalcEng();

        public CalcEngSteps(TestContext testContext) : base(testContext) { }

        [When(@"I Add two numbers")]
        public void ExecuteAddTwoNumbers()
        {
            var request = Request.ToObject<TwoNumbersOperationRequest>();
            var operationResponse = _calcEng.Sum(request);
            SetResponse(JObject.FromObject(operationResponse));
        }

        [When(@"I Add several numbers")]
        public void ExecuteAddSeveralNumbers()
        {
            var request = Request.ToObject<MultiNumbersOperationRequest>();
            var operationResponse = _calcEng.Sum(request);
            SetResponse(JObject.FromObject(operationResponse));
        }

        [When(@"I execute the Div operation")]
        public void ExecuteDiv()
        {
            var request = Request.ToObject<TwoNumbersOperationRequest>();
            var operationResponse = _calcEng.Div(request);
            SetResponse(JObject.FromObject(operationResponse));
        }

        [When(@"I request prime numbers")]
        public void RequestPrimeNumbers()
        {
            var operationResponse = _calcEng.PrimeNumbers();
            SetResponse(JObject.FromObject(operationResponse));
        }
    }
}
