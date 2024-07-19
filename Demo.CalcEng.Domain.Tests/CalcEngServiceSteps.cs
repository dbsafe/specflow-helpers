using Microsoft.VisualStudio.TestTools.UnitTesting;
using Specflow.Steps.Object;
using TechTalk.SpecFlow;

namespace Demo.CalcEng.Domain.Tests
{
    [Binding]
    [Scope(Feature = "CalcEng")]
    public class CalcEngServiceSteps : JObjectBuilderSteps
    {
        private readonly CalcEngService _calcEng = new();

        public CalcEngServiceSteps(TestContext testContext) : base(testContext) { }

        [When(@"I add two numbers")]
        public void ExecuteAddTwoNumbers()
        {
            var request = Request.ToObject<TwoNumbersOperationRequest>();
            var operationResponse = _calcEng.Sum(request);
            SetResponse(operationResponse);
        }

        [When(@"I Add several numbers")]
        public void ExecuteAddSeveralNumbers()
        {
            var request = Request.ToObject<MultiNumbersOperationRequest>();
            var operationResponse = _calcEng.Sum(request);
            SetResponse(operationResponse);
        }

        [When(@"I execute the Div operation")]
        public void ExecuteDiv()
        {
            var request = Request.ToObject<TwoNumbersOperationRequest>();
            var operationResponse = _calcEng.Div(request);
            SetResponse(operationResponse);
        }

        [When(@"I request prime numbers")]
        public void RequestPrimeNumbers()
        {
            var operationResponse = _calcEng.PrimeNumbers();
            SetResponse(operationResponse);
        }

        [When(@"I request domain items")]
        public void RequestDomainItems()
        {
            var operationResponse = _calcEng.GetDomainItems();
            SetResponse(operationResponse);
        }

        [When(@"I request domain items with DateTimeOffset")]
        public void RequestDomainItemsWithDateTimeOffset()
        {
            var operationResponse = _calcEng.GetDomainItemsWithDateTimeOffset();
            SetResponse(operationResponse);
        }

        [When(@"I request domain items as array")]
        public void RequestDomainItemsAsArray()
        {
            var response = _calcEng.GetDomainItemsAsArray();
            SetResponseWithArray(response);
        }

        [When(@"I request domain items by date")]
        public void RequestDomainItemsByDate()
        {
            var request = Request.ToObject<GetDomainItemsByDateRequest>();
            var operationResponse = _calcEng.GetDomainItemsByDate(request);
            SetResponse(operationResponse);
        }

        [When(@"I request pi")]
        public void RequestPi()
        {
            var operationResult = _calcEng.Pi();
            SetResponse(operationResult);
        }

        [When(@"I calculate totals")]
        public void WhenICalculateTotals()
        {
            var request = Request.ToObject<TotalsOperationRequest>();
            var operationResult = _calcEng.Totals(request);
            SetResponse(operationResult);
        }

        [When(@"I request a guid")]
        public void WhenIRequestAGuid()
        {
            var operationResult = _calcEng.GetGuid();
            SetResponse(operationResult);
        }

        [When(@"I request a guid without dashes")]
        public void WhenIRequestAGuidWithoutDashes()
        {
            var operationResult = _calcEng.GetGuidWithoutDashes();
            SetResponse(operationResult);
        }
    }
}
