using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Specflow.Steps.Object;
using System;
using TechTalk.SpecFlow;

namespace Demo.CalcEng.Domain.Tests
{
    [Binding]
    [Scope(Feature = "FrameworkTest")]
    public class FrameworkSteps : JObjectBuilderSteps
    {
        private Exception _exception;
        private bool _exceptionExpected;

        public FrameworkSteps(TestContext testContext) : base(testContext)
        {
        }

        [AfterScenario]
        public void EnsureExceptionWasVerified()
        {
            if (_exception != null && !_exceptionExpected)
            {
                Assert.Fail("An exception was thrown");
            }
        }

        [When(@"returned content is the object '(.*)'")]
        public void ReturnObject(string content)
        {
            var responseContent = JToken.Parse(content);
            SetResponse(responseContent);
        }

        [When(@"returned content is the array '(.*)'")]
        public void ReturnArray(string content)
        {
            var responseContent = JToken.Parse(content);
            SetResponseWithArray(responseContent);
        }

        [When(@"returned content is the value '(.*)'")]
        public void ReturnValue(string content)
        {
            var responseContent = JToken.Parse(content);
            SetResponseWithValue(responseContent);
        }

        [Then(@"jpath '(.*)' should be the complex-element array \(framework-test\)")]
        public void AssertComplexArrayJPathFrameworkTest(string propertyName, Table table)
        {
            ExecuteAndCaptureException(() => AssertComplexArrayJPath(propertyName, table));
        }

        [Then(@"should throw exception of type ([^\s]+) with message containing '([^']*)'")]
        public void ValidateThatAnExceptionWasThrownWithMessage(string exceptionTypeName, string message)
        {
            _exceptionExpected = true;
            Assert.IsNotNull(_exception, "An exception was not thrown");
            Assert.AreEqual(exceptionTypeName, _exception.GetType().Name);
            Assert.IsTrue(_exception.Message.Contains(message), $"Exception message: '{_exception.Message}' does not contain '{message}'");
        }

        [Then(@"should throw exception ([^\s]+)")]
        public void ValidateThatAnExceptionWasThrown(string exceptionTypeName)
        {
            _exceptionExpected = true;
            Assert.IsNotNull(_exception, "An exception was not thrown");
            Assert.AreEqual(exceptionTypeName, _exception.GetType().Name);
        }

        private void ExecuteAndCaptureException(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                Print($"Exception: {ex}");
                _exception = ex;
            }
        }
    }
}
