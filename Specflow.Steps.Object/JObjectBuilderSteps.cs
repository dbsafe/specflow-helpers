using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Specflow.Steps.Object.Collections;
using Specflow.Steps.Object.ExtensionMethods;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

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

        public void SetResponse(object response)
        {
            Response = JObject.FromObject(response);
        }

        #region Given

        [Given(@"property ([^\s]+) equals to '(.*)'")]
        public void SetRequestProperty(string name, string value)
        {
            ExecuteProtected(() => SetRequestContentProperty(name, value));
        }

        [Given(@"property ([^\s]+) equals to the number ([-+]?[\d]*[\.]?[\d]+)")]
        public void SetRequestProperty(string name, decimal value)
        {
            ExecuteProtected(() => SetRequestContentProperty(name, value));
        }

        /// <summary>
        /// Assigns several properties
        /// Ex:
        /// Given properties
        /// | name    | value       |
        /// | Address | 10 Main St. |
        /// | City    | MyTown      |
        /// </summary>
        /// <param name="table"></param>
        [Given(@"properties")]
        public void SetRequestProperties(Table table)
        {
            ExecuteProtected(() => SetRequestContentProperties(table));
        }

        [Given(@"property ([^\s]+) is an empty array")]
        public void SetRequestPropertyAsEmptyArray(string name)
        {
            ExecuteProtected(() => SetRequestContentPropertyAsEmptyArray(name));
        }

        [Given(@"property ([^\s]+) is the array '(.*)'")]
        public void SetRequestPropertyAsArray(string name, string itemsCsv)
        {
            ExecuteProtected(() => SetRequestContentPropertyAsArray(name, itemsCsv));
        }

        #endregion

        #region Then

        [Then(@"property ([^\s]+) should be the number ([-+]?[\d]*[\.]?[\d]+)")]
        public void AssertNumericProperty(string propertyName, decimal expectedPropertyValue)
        {
            ExecuteProtected(() =>
            {
                ValidateResponseProperty(propertyName, expectedPropertyValue);
            });
        }

        [Then(@"property ([^\s]+) should be the datetime '(.*)'")]
        public void AssertDateTimeProperty(string propertyName, DateTime expectedPropertyValue)
        {
            ExecuteProtected(() =>
            {
                ValidateResponseProperty(propertyName, expectedPropertyValue);
            });
        }

        [Then(@"property ([^\s]+) should be null")]
        [Then(@"property ([^\s]+) should be NULL")]
        public void AssertNullProperty(string propertyName)
        {
            ExecuteProtected(() =>
            {
                ValidateNullProperty(propertyName);
            });
        }

        [Then(@"property ([^\s]+) should be (False|false|True|true)")]
        public void AssertBooleanProperty(string propertyName, bool expectedPropertyValue)
        {
            ExecuteProtected(() =>
            {
                ValidateResponseProperty(propertyName, expectedPropertyValue);
            });
        }

        [Then(@"property ([^\s]+) should be '(.*)'")]
        public void AssertTextProperty(string propertyName, string expectedPropertyValue)
        {
            ExecuteProtected(() =>
            {
                ValidateResponseProperty(propertyName, expectedPropertyValue);
            });
        }

        [Then(@"property ([^\s]+) should be the single-element array '(.*)'")]
        public void AssertArrayProperty(string propertyName, string itemsCsv)
        {
            ExecuteProtected(() =>
            {
                ValidateArrayProperty(propertyName, itemsCsv);
            });
        }

        [Then(@"property ([^\s]+) should be the single-element array")]
        public void AssertArrayProperty(string propertyName, Table table)
        {
            ExecuteProtected(() =>
            {
                ValidateSingleColumnArray(propertyName, table);
            });
        }

        [Then(@"property ([^\s]+) should be the complex-element array")]
        public void AssertComplexArrayProperty(string propertyName, Table table)
        {
            ExecuteProtected(() =>
            {
                ValidateMultiColumnArray(propertyName, table);
            });
        }

        private void ValidateSingleColumnArray(string arrayPropertyName, Table table)
        {
            var actualToken = FindProperty(arrayPropertyName);
            Assert.AreEqual(JTokenType.Array, actualToken.Type, $"Property {arrayPropertyName}. Actual type is not an array");

            var expectedArray = table.Rows.Select(a => a[0]).ToArray();
            var actualArray = actualToken.Children().Select(a => a.ToString()).ToArray();
            var areArrayEqual = expectedArray.SequenceEqual(actualArray);

            if (!areArrayEqual)
            {
                Assert.Fail($"Array property {arrayPropertyName}. Actual and expected don't match");
            }
        }

        private void ValidateMultiColumnArray(string arrayPropertyName, Table table)
        {
            var actualToken = FindProperty(arrayPropertyName);
            Assert.AreEqual(JTokenType.Array, actualToken.Type, $"Property {arrayPropertyName}. Actual type is not an array");

            var expectedDataset = DataCollection.Load(table);
            var actualDataset = DataCollection.Load(actualToken.Children());
            if (!DataCompare.Compare(expectedDataset, actualDataset, out string message))
            {
                Assert.Fail($"Array property {arrayPropertyName}.\n{message}");
            }
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

        private void SetRequestContentProperties(Table table)
        {
            var items = table.CreateSet<ObjectProperty>();
            foreach (var item in items)
            {
                Request[item.Name] = item.Value;
            }
        }

        private void SetRequestContentPropertyAsEmptyArray(string name)
        {
            Request.SetProperty(name, new string[0]);
        }

        private void SetRequestContentPropertyAsArray(string name, string itemsCvs)
        {
            var items = itemsCvs.Split(',').Select(a => a.Trim()).ToArray();
            Request.SetProperty(name, items);
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

        private void ValidateResponseProperty(string name, DateTime value)
        {
            var jToken = FindProperty(name);
            Assert.IsTrue(jToken is JValue, $"Property {name} is not a single value");
            var jValue = jToken as JValue;
            Assert.IsNotNull(jValue.Value, $"Property {name} is null");
            Assert.IsTrue(IsDateTime(jValue), $"Property {name} is not a datetime");
            Assert.IsTrue(DateTime.TryParse(jValue.Value.ToString(), out DateTime convertedValue), $"Property {name} is not a valid number");
            Assert.AreEqual(value, convertedValue, $"Property: {name}");
        }

        private void ValidateResponseProperty(string name, bool value)
        {
            var jToken = FindProperty(name);
            Assert.IsTrue(jToken is JValue, $"Property {name} is not a single value");
            var jValue = jToken as JValue;
            Assert.IsNotNull(jValue.Value, $"Property {name} is null");
            Assert.IsTrue(IsBoolean(jValue), $"Property {name} is not a boolean");
            Assert.IsTrue(bool.TryParse(jValue.Value.ToString(), out bool convertedValue), $"Property {name} is not a valid boolean");
            Assert.AreEqual(value, convertedValue, $"Property: {name}");
        }

        private void ValidateResponseProperty(string name, string value)
        {
            var jToken = FindProperty(name);
            Assert.IsTrue(jToken is JValue, $"Property {name} is not a single value");
            var jValue = jToken as JValue;
            Assert.IsNotNull(jValue.Value, $"Property {name} is null");
            Assert.AreEqual(value, jValue.Value.ToString(), $"Property: {name}");
        }

        private void ValidateNullProperty(string name)
        {
            var jToken = FindProperty(name, true);
            if (jToken == null)
            {
                return;
            }

            Assert.IsTrue(jToken is JValue, $"Property {name} is not a single value");
            var jValue = jToken as JValue;
            Assert.IsNull(jValue.Value, $"Property {name} is not null");
        }

        private void ValidateArrayProperty(string propertyName, string itemsCsv)
        {
            var actualToken = FindProperty(propertyName);
            Assert.AreEqual(JTokenType.Array, actualToken.Type, $"Property {propertyName}. Actual type is not an array");

            var expectedArray = itemsCsv.Split(',').Select(a => a.Trim());
            var actualArray = actualToken.Children().Select(a => a.ToString()).ToArray();
            var areArrayEqual = expectedArray.SequenceEqual(actualArray);

            if (!areArrayEqual)
            {
                Assert.Fail($"Array property {propertyName}. Actual and expected don't match");
            }
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

        private bool IsDateTime(JToken jToken)
        {
            return jToken.Type == JTokenType.Date;
        }

        private bool IsNumber(JToken jToken)
        {
            return jToken.Type == JTokenType.Float || jToken.Type == JTokenType.Integer;
        }

        private bool IsBoolean(JToken jToken)
        {
            return jToken.Type == JTokenType.Boolean;
        }
    }
}
