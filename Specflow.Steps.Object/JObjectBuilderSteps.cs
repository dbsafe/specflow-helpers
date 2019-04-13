using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Specflow.Steps.Object.ExtensionMethods;
using System;
using System.Runtime.CompilerServices;
using TechTalk.SpecFlow;

namespace Specflow.Steps.Object
{
    [Binding]
    public class JObjectBuilderSteps
    {
        public TestContext TestContext { get; }
        public JObject Request { get; } = new JObject();
        public JObject Response { get; private set; }

        public JObjectBuilderSteps(TestContext testContext)
        {
            TestContext = testContext;
        }

        public void SetResponse(JObject response)
        {
            Response = response;
        }

        #region Given

        /// <summary>
        /// Assigns a string value to a property
        /// Ex:
        /// Given property property-name equals to "value-to-be-assigned"
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        [Given(@"property ([^\s]+) equals to ""(.*)""")]
        public void SetRequestProperty(string name, string value)
        {
            ExecuteProtected(() => SetRequestContentProperty(name, value));
        }

        /// <summary>
        /// Assigns a number to a property
        /// Ex:
        /// Given property secondNumber equals to the number 20
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        [Given(@"property ([^\s]+) equals to the number ([-+]?[\d]*[\.]?[\d]+)")]
        public void SetRequestProperty(string name, decimal value)
        {
            ExecuteProtected(() => SetRequestContentProperty(name, value));
        }

        #endregion

        #region Then

        /// <summary>
        /// Assert an expected numeric property
        /// Ex:
        /// Then property result should be the number 100
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="expectedPropertyValue"></param>
        [Then(@"property ([^\s]+) should be the number ([-+]?[\d]*[\.]?[\d]+)")]
        public void AssertNumericProperty(string propertyName, decimal expectedPropertyValue)
        {
            ExecuteProtected(() =>
            {
                ValidateResponseProperty(propertyName, expectedPropertyValue);
            });
        }

        #endregion

        protected void ExecuteProtected(Action action, [CallerMemberName]string caller = null)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error when processing method {caller}\n{ex.ToString()}";
                Print(errorMessage);
                throw;
            }
        }

        protected void Print(string message)
        {
            TestContext.WriteLine($"{DateTime.Now.ToString("HH:mm:ss.fff")}\n{message}");
        }

        protected virtual void ValidateResponse()
        {
            Assert.IsNotNull(Response, "Response is not assigned");
        }

        private void SetRequestContentProperty(string name, string value)
        {
            Request.SetProperty(name, value);
        }

        private void SetRequestContentProperty(string name, decimal value)
        {
            Request.SetProperty(name, value);
        }

        private void ValidateResponseProperty(string name, decimal value)
        {
            var jToken = FindProperty(name);
            Assert.IsTrue(jToken is JValue, $"Property {name} is not a single value");
            var jValue = jToken as JValue;
            Assert.IsNotNull(jValue.Value, $"Property {name} is null");
            Assert.IsTrue(IsNumber(jValue), $"Property {name} is not a number");
            Assert.IsTrue(decimal.TryParse(jValue.Value.ToString(), out decimal convertedValue), $"Property {name} is not a valid number");
            Assert.AreEqual(value, convertedValue, $"Property: {name}");
        }

        private JToken FindProperty(string name, bool canBeNull = false)
        {
            ValidateResponse();
            var jToken = Response.SelectToken($"$.{name}");
            if (!canBeNull)
            {
                Assert.IsNotNull(jToken, $"Property {name} not found in the response");
            }

            return jToken;
        }

        private bool IsNumber(JToken jToken)
        {
            return jToken.Type == JTokenType.Float || jToken.Type == JTokenType.Integer;
        }
    }
}
