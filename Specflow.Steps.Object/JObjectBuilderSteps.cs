using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Specflow.Steps.Object.Collections;
using Specflow.Steps.Object.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Specflow.Steps.Object
{
    public class FieldFilter
    {
        public string FieldName { get; set; }
        public string FieldValues { get; set; }
    }
    public class JObjectBuilderSteps
    {
        private readonly Dictionary<string, IEnumerable<FieldFilter>> _fieldFilters = new Dictionary<string, IEnumerable<FieldFilter>>(20);

        protected bool UseNullForMissingProperties { get; set; }

        public TestContext TestContext { get; }
        public JObject Request { get; private set; }
        public JObject Response { get; private set; }
        public JArray ArrayResponse { get; private set; }

        public JObjectBuilderSteps(TestContext testContext)
        {
            TestContext = testContext;
        }

        public void SetResponse(object response)
        {
            Response = JObject.FromObject(response);
        }

        public void SetResponseWithArray(object arrayResponse)
        {
            ArrayResponse = JArray.FromObject(arrayResponse);
        }

        public void SetResponse(JObject response)
        {
            Response = response;
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

        [Given(@"property ([^\s]+) is the complex-element array")]
        public void SetRequestPropertyAsArrayWithComplexElements(string name, Table table)
        {
            ExecuteProtected(() => SetRequestContentPropertyAsComplexElementArray(name, table));
        }

        [Given(@"I filter property ([^\s]+) by")]
        public void FilterArrayWithComplexElements(string name, Table table)
        {
            ExecuteProtected(() => SetFilterArrayWithComplexElements(name, table));
        }

        #endregion

        #region Then

        [Then(@"property ([^\s]+) should be the number ([-+]?[\d]*[\.]?[\d]+)")]
        public void AssertNumericProperty(string propertyName, decimal expectedPropertyValue)
        {
            ExecuteProtected(() =>
            {
                ValidateResponseProperty(propertyName, expectedPropertyValue, false);
            });
        }

        [Then(@"jpath '(.*)' should be the number ([-+]?[\d]*[\.]?[\d]+)")]
        public void AssertNumericJPath(string jpath, decimal expectedPropertyValue)
        {
            ExecuteProtected(() =>
            {
                ValidateResponseProperty(jpath, expectedPropertyValue, true);
            });
        }

        [Then(@"property ([^\s]+) should be the guid '([0-9A-Fa-f-]+)'")]
        public void AssertGuidProperty(string propertyName, Guid expectedPropertyValue)
        {
            ExecuteProtected(() =>
            {
                ValidateResponseProperty(propertyName, expectedPropertyValue, false);
            });
        }

        
        [Then(@"jpath '(.*)' should be the guid '([0-9A-Fa-f-]+)'")]
        public void AssertGuidJPath(string jpath, Guid expectedPropertyValue)
        {
            ExecuteProtected(() =>
            {
                ValidateResponseProperty(jpath, expectedPropertyValue, true);
            });
        }

        [Then(@"property ([^\s]+) should be the datetime '(.*)'")]
        public void AssertDateTimeProperty(string propertyName, DateTime expectedPropertyValue)
        {
            ExecuteProtected(() =>
            {
                ValidateResponseProperty(propertyName, expectedPropertyValue, false);
            });
        }

        [Then(@"jpath '(.*)' should be the datetime '(.*)'")]
        public void AssertDateTimeJPath(string jpath, DateTime expectedPropertyValue)
        {
            ExecuteProtected(() =>
            {
                ValidateResponseProperty(jpath, expectedPropertyValue, true);
            });
        }

        [Then(@"property ([^\s]+) should be null")]
        [Then(@"property ([^\s]+) should be NULL")]
        public void AssertNullProperty(string propertyName)
        {
            ExecuteProtected(() =>
            {
                ValidateNullProperty(propertyName, false);
            });
        }

        [Then(@"jpath '(.*)' should be null")]
        [Then(@"jpath '(.*)' should be NULL")]
        public void AssertNullJPath(string jpath)
        {
            ExecuteProtected(() =>
            {
                ValidateNullProperty(jpath, true);
            });
        }

        [Then(@"property ([^\s]+) should be (False|false|True|true)")]
        public void AssertBooleanProperty(string propertyName, bool expectedPropertyValue)
        {
            ExecuteProtected(() =>
            {
                ValidateResponseProperty(propertyName, expectedPropertyValue, false);
            });
        }

        [Then(@"jpath '(.*)' should be (False|false|True|true)")]
        public void AssertBooleanJPath(string jpath, bool expectedPropertyValue)
        {
            ExecuteProtected(() =>
            {
                ValidateResponseProperty(jpath, expectedPropertyValue, true);
            });
        }

        [Then(@"property ([^\s]+) should be '(.*)'")]
        public void AssertTextProperty(string propertyName, string expectedPropertyValue)
        {
            ExecuteProtected(() =>
            {
                ValidateResponseProperty(propertyName, expectedPropertyValue, false);
            });
        }

        [Then(@"jpath '(.*)' should be '(.*)'")]
        public void AssertTextJPath(string jpath, string expectedPropertyValue)
        {
            ExecuteProtected(() =>
            {
                ValidateResponseProperty(jpath, expectedPropertyValue, true);
            });
        }

        [Then(@"property ([^\s]+) should be the single-element array '(.*)'")]
        public void AssertArrayProperty(string propertyName, string itemsCsv)
        {
            ExecuteProtected(() =>
            {
                ValidateArrayProperty(propertyName, itemsCsv, false);
            });
        }

        [Then(@"jpath '(.*)' should be the single-element array '(.*)'")]
        public void AssertArrayJPath(string jpath, string itemsCsv)
        {
            ExecuteProtected(() =>
            {
                ValidateArrayProperty(jpath, itemsCsv, true);
            });
        }

        [Then(@"property ([^\s]+) should be the single-element array")]
        public void AssertArrayProperty(string propertyName, Table table)
        {
            ExecuteProtected(() =>
            {
                ValidateSingleColumnArray(propertyName, table, false);
            });
        }

        [Then(@"jpath '(.*)' should be the single-element array")]
        public void AssertArrayJPath(string jpath, Table table)
        {
            ExecuteProtected(() =>
            {
                ValidateSingleColumnArray(jpath, table, true);
            });
        }

        [Then(@"property ([^\s]+) should be the complex-element array")]
        public void AssertComplexArrayProperty(string propertyName, Table table)
        {
            ExecuteProtected(() =>
            {
                ValidateMultiColumnArray(propertyName, table, false);
            });
        }

        [Then(@"content should be the complex-element array")]
        public void AssertContentAsComplexArrayProperty(Table table)
        {
            ExecuteProtected(() =>
            {
                ValidateContentAsMultiColumnArray(table);
            });
        }

        [Then(@"jpath '(.*)' should be the complex-element array")]
        public void AssertComplexArrayJPath(string jpath, Table table)
        {
            ExecuteProtected(() =>
            {
                ValidateMultiColumnArray(jpath, table, true);
            });
        }

        [Then(@"property ([^\s]+) should be an empty array")]
        public void AssertEmptyArrayProperty(string propertyName)
        {
            ExecuteProtected(() =>
            {
                ValidateEmptyArrayProperty(propertyName, false);
            });
        }

        [Then(@"jpath '(.*)' should be an empty array")]
        public void AssertEmptyArrayJPath(string jpath)
        {
            ExecuteProtected(() =>
            {
                ValidateEmptyArrayProperty(jpath, true);
            });
        }

        [Then(@"property ([^\s]+) should be an array with ([\d]+) items")]
        public void AssertPropertyArrayCount(string propertyName, int count)
        {
            ExecuteProtected(() =>
            {
                ValidateArrayCount(propertyName, count, false);
            });
        }

        [Then(@"jpath '(.*)' should be an array with ([\d]+) items")]
        public void AssertJPathArrayCount(string jpath, int count)
        {
            ExecuteProtected(() =>
            {
                ValidateArrayCount(jpath, count, true);
            });
        }

        [Then(@"property ([^\s]+) should be an array with 1 item")]
        public void AssertPropertyArrayHasOneItem(string propertyName)
        {
            ExecuteProtected(() =>
            {
                ValidateArrayCount(propertyName, 1, false);
            });
        }

        [Then(@"jpath '(.*)' should be an array with 1 item")]
        public void AssertJPathArrayHasOneItem(string jpath)
        {
            ExecuteProtected(() =>
            {
                ValidateArrayCount(jpath, 1, true);
            });
        }

        [Then(@"property ([^\s]+) should be a number between (.*) and (.*)")]
        public void AssertNumericProperty(string propertyName, decimal minValue, decimal maxValue)
        {
            ExecuteProtected(() =>
            {
                ValidateNumericProperty(propertyName, minValue, maxValue, false);
            });
        }

        [Then(@"jpath '(.*)' should be a number between (.*) and (.*)")]
        public void AssertNumericJPath(string jpath, decimal minValue, decimal maxValue)
        {
            ExecuteProtected(() =>
            {
                ValidateNumericProperty(jpath, minValue, maxValue, true);
            });
        }

        [Then(@"property ([^\s]+) should be a datetime between '(.*)' and '(.*)'")]
        public void AssertDateTimeProperty(string propertyName, DateTime minValue, DateTime maxValue)
        {
            ExecuteProtected(() =>
            {
                ValidateDateTimeProperty(propertyName, minValue, maxValue, false);
            });
        }

        [Then(@"jpath '(.*)' should be a datetime between '(.*)' and '(.*)'")]
        public void AssertDateTimeJPath(string jpath, DateTime minValue, DateTime maxValue)
        {
            ExecuteProtected(() =>
            {
                ValidateDateTimeProperty(jpath, minValue, maxValue, true);
            });
        }

        private void ValidateSingleColumnArray(string arrayPropertyName, Table table, bool isJPath)
        {
            var actualToken = isJPath ? FindJPath(arrayPropertyName) : FindProperty(arrayPropertyName);
            Assert.AreEqual(JTokenType.Array, actualToken.Type, $"Property {arrayPropertyName}. Actual type is not an array");

            var expectedArray = table.Rows.Select(a => a[0]).ToArray();
            var actualArray = actualToken.Children().Select(a => a.ToString()).ToArray();
            var areArrayEqual = expectedArray.SequenceEqual(actualArray);

            if (!areArrayEqual)
            {
                Assert.Fail($"Array property {arrayPropertyName}. Actual and expected don't match");
            }
        }

        private void ValidateMultiColumnArray(string arrayPropertyName, Table table, bool isJPath)
        {
            var actualToken = isJPath ? FindJPath(arrayPropertyName) : FindProperty(arrayPropertyName);
            Assert.AreEqual(JTokenType.Array, actualToken.Type, $"Property {arrayPropertyName}. Actual type is not an array");

            var expectedDataset = DataCollection.Load(table);
            var actualDataset = DataCollection.Load(actualToken.Children());
            actualDataset = FilterRows(actualDataset, arrayPropertyName);
            if (!DataCompare.Compare(expectedDataset, actualDataset, out string message))
            {
                Assert.Fail($"Array property {arrayPropertyName}.\n{message}");
            }
        }

        private void ValidateContentAsMultiColumnArray(Table table)
        {
            ValidateArrayResponse();

            var expectedDataset = DataCollection.Load(table);
            var actualDataset = DataCollection.Load(ArrayResponse.Children());
            if (!DataCompare.Compare(expectedDataset, actualDataset, out string message))
            {
                Assert.Fail($"Array Response.\n{message}");
            }
        }

        private DataCollection FilterRows(DataCollection source, string arrayPropertyName)
        {
            if (!_fieldFilters.ContainsKey(arrayPropertyName))
            {
                return source;
            }

            var filters = _fieldFilters[arrayPropertyName];
            var filteredRows = source.Rows.ToArray();
            foreach (var filter in filters)
            {
                var values = filter.FieldValues.Split(',').Select(v => v.Trim());
                filteredRows = filteredRows.Where(r => r.Values.Where(c => c.Name == filter.FieldName && values.Contains(c.Value.ToString())).Any())
                    .ToArray();
            }

            return new DataCollection { Rows = filteredRows };
        }

        #endregion

        protected void ExecuteProtected(Action action, [CallerMemberName] string caller = null)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error when processing method {caller}\n{ex}";
                Print(errorMessage);
                throw;
            }
        }

        protected void Print(string message)
        {
            TestContext.WriteLine($"{DateTime.Now:HH:mm:ss.fff}\n{message}");
        }

        protected virtual void ValidateResponse()
        {
            Assert.IsNotNull(Response, "Response is not assigned");
        }

        protected virtual void ValidateArrayResponse()
        {
            Assert.IsNotNull(ArrayResponse, "Array Response is not assigned");
        }

        private void SetRequestContentProperty(string name, string value)
        {
            InitializeRequest();
            Request.SetProperty(name, value);
        }

        private void SetRequestContentProperty(string name, decimal value)
        {
            InitializeRequest();
            Request.SetProperty(name, value);
        }

        private void SetRequestContentProperties(Table table)
        {
            InitializeRequest();
            var items = table.CreateSet<ObjectProperty>();
            foreach (var item in items)
            {
                Request[item.Name] = item.Value;
            }
        }

        private void SetRequestContentPropertyAsEmptyArray(string name)
        {
            InitializeRequest();
            Request.SetProperty(name, new string[0]);
        }

        private void SetRequestContentPropertyAsArray(string name, string itemsCvs)
        {
            InitializeRequest();
            var items = itemsCvs.Split(',').Select(a => a.Trim()).ToArray();
            Request.SetProperty(name, items);
        }

        private void SetRequestContentPropertyAsComplexElementArray(string name, Table table)
        {
            InitializeRequest();
            var prop = CreateJArrayFromTable(table);
            Request.Add(name, prop);
        }

        private void SetFilterArrayWithComplexElements(string propertyName, Table filter)
        {
            Assert.IsTrue(filter.Header.Contains(nameof(FieldFilter.FieldName)), $"Column '{nameof(FieldFilter.FieldName)}' is missing in the filter");
            Assert.IsTrue(filter.Header.Contains(nameof(FieldFilter.FieldValues)), $"Column '{nameof(FieldFilter.FieldValues)}' is missing in the filter");

            var filters = filter.CreateSet<FieldFilter>();
            _fieldFilters[propertyName] = filters;
        }

        protected JArray CreateJArrayFromTable(Table table)
        {
            var items = new List<JObject>();
            var dataCollection = DataCollection.Load(table);
            foreach (var row in dataCollection.Rows)
            {
                var item = new JObject();
                items.Add(item);

                foreach (var cell in row.Values)
                {
                    if (cell.IsNull)
                    {
                        continue;
                    }

                    if (cell.Type == typeof(decimal))
                    {
                        var value = (decimal)cell.Value;
                        item.SetProperty(cell.Name, value);
                        continue;
                    }

                    if (cell.Type == typeof(int))
                    {
                        var value = (int)cell.Value;
                        item.SetProperty(cell.Name, value);
                        continue;
                    }

                    if (cell.Type == typeof(bool))
                    {
                        var value = (bool)cell.Value;
                        item.SetProperty(cell.Name, value);
                        continue;
                    }

                    item.SetProperty(cell.Name, cell.Value.ToString());
                }
            }

            return JArray.FromObject(items);
        }

        private void InitializeRequest()
        {
            if (Request == null)
            {
                Request = new JObject();
            }
        }

        private void ValidateResponseProperty(string name, Guid value, bool isJPath)
        {
            var jToken = isJPath ? FindJPath(name) : FindProperty(name);
            Assert.IsTrue(jToken is JValue, $"Property {name} is not a single value");
            var jValue = jToken as JValue;
            Assert.IsNotNull(jValue.Value, $"Property {name} is null");
            Assert.IsTrue(Guid.TryParse(jValue.Value.ToString(), out Guid convertedValue), $"Property {name} is not a valid guid");
            Assert.AreEqual(value, convertedValue, $"Property: {name}");
        }

        private void ValidateResponseProperty(string name, decimal value, bool isJPath)
        {
            var jToken = isJPath ? FindJPath(name) : FindProperty(name);
            Assert.IsTrue(jToken is JValue, $"Property {name} is not a single value");
            var jValue = jToken as JValue;
            Assert.IsNotNull(jValue.Value, $"Property {name} is null");
            Assert.IsTrue(IsNumber(jValue), $"Property {name} is not a number");
            Assert.IsTrue(decimal.TryParse(jValue.Value.ToString(), out decimal convertedValue), $"Property {name} is not a valid number");
            Assert.AreEqual(value, convertedValue, $"Property: {name}");
        }

        private void ValidateResponseProperty(string name, DateTime value, bool isJPath)
        {
            var jToken = isJPath ? FindJPath(name) : FindProperty(name);
            Assert.IsTrue(jToken is JValue, $"Property {name} is not a single value");
            var jValue = jToken as JValue;
            Assert.IsNotNull(jValue.Value, $"Property {name} is null");
            Assert.IsTrue(IsDateTime(jValue), $"Property {name} is not a datetime");
            Assert.IsTrue(DateTime.TryParse(jValue.Value.ToString(), out DateTime convertedValue), $"Property {name} is not a valid number");
            Assert.AreEqual(value, convertedValue, $"Property: {name}");
        }

        private void ValidateResponseProperty(string name, bool value, bool isJPath)
        {
            var jToken = isJPath ? FindJPath(name) : FindProperty(name);
            Assert.IsTrue(jToken is JValue, $"Property {name} is not a single value");
            var jValue = jToken as JValue;
            Assert.IsNotNull(jValue.Value, $"Property {name} is null");
            Assert.IsTrue(IsBoolean(jValue), $"Property {name} is not a boolean");
            Assert.IsTrue(bool.TryParse(jValue.Value.ToString(), out bool convertedValue), $"Property {name} is not a valid boolean");
            Assert.AreEqual(value, convertedValue, $"Property: {name}");
        }

        private void ValidateResponseProperty(string name, string value, bool isJPath)
        {
            var jToken = isJPath ? FindJPath(name) : FindProperty(name);
            Assert.IsTrue(jToken is JValue, $"Property {name} is not a single value");
            var jValue = jToken as JValue;
            Assert.IsNotNull(jValue.Value, $"Property {name} is null");
            Assert.AreEqual(value, jValue.Value.ToString(), $"Property: {name}");
        }

        private void ValidateNullProperty(string name, bool isJPath)
        {
            var jToken = isJPath ? FindJPath(name) : FindProperty(name);
            if (jToken == null)
            {
                return;
            }

            Assert.IsTrue(jToken is JValue, $"Property {name} is not a single value");
            var jValue = jToken as JValue;
            Assert.IsNull(jValue.Value, $"Property {name} is not null");
        }

        private JToken FindArrayProperty(string propertyName, bool isJPath)
        {
            var actualToken = isJPath ? FindJPath(propertyName) : FindProperty(propertyName);
            Assert.AreEqual(JTokenType.Array, actualToken.Type, $"Property {propertyName}. Actual type is not an array");

            return actualToken;
        }

        private void ValidateArrayProperty(string propertyName, string itemsCsv, bool isJPath)
        {
            var actualToken = FindArrayProperty(propertyName, isJPath);

            var expectedArray = itemsCsv.Split(',').Select(a => a.Trim());
            var actualArray = actualToken.Children().Select(a => a.ToString()).ToArray();
            var areArrayEqual = expectedArray.SequenceEqual(actualArray);

            if (!areArrayEqual)
            {
                Assert.Fail($"Array property {propertyName}. Actual and expected don't match");
            }
        }

        private void ValidateEmptyArrayProperty(string propertyName, bool isJPath)
        {
            var actualToken = FindArrayProperty(propertyName, isJPath);
            var actualArray = actualToken.Children().Select(a => a.ToString()).ToArray();
            Assert.AreEqual(0, actualArray.Length, $"Array property {propertyName} is not empty");
        }

        private void ValidateArrayCount(string arrayPropertyName, int count, bool isJPath)
        {
            var actualToken = FindArrayProperty(arrayPropertyName, isJPath);
            var actualCount = (actualToken as JArray).Count;
            Assert.AreEqual(count, actualCount, $"Array: {arrayPropertyName}. Actual number of items is {actualCount}");
        }

        private void ValidateNumericProperty(string propertyName, decimal minValue, decimal maxValue, bool isJPath)
        {
            if (minValue > maxValue)
            {
                Assert.Fail($"Invalid range [{minValue} - {maxValue}]");
            }

            var jToken = isJPath ? FindJPath(propertyName) : FindProperty(propertyName);
            Assert.IsTrue(jToken is JValue, $"Property {propertyName} is not a single value");
            var jValue = jToken as JValue;
            Assert.IsNotNull(jValue.Value, $"Property {propertyName} is null");
            Assert.IsTrue(IsNumber(jValue), $"Property {propertyName} is not a number");
            Assert.IsTrue(decimal.TryParse(jValue.Value.ToString(), out decimal convertedValue), $"Property {propertyName} is not a valid number");

            if (convertedValue < minValue)
            {
                Assert.Fail($"Property {propertyName} is less than {minValue}");
            }

            if (convertedValue > maxValue)
            {
                Assert.Fail($"Property {propertyName} is greater than {maxValue}");
            }
        }

        private void ValidateDateTimeProperty(string propertyName, DateTime minValue, DateTime maxValue, bool isJPath)
        {
            if (minValue > maxValue)
            {
                Assert.Fail($"Invalid range [{minValue} - {maxValue}]");
            }

            var jToken = isJPath ? FindJPath(propertyName) : FindProperty(propertyName);
            Assert.IsTrue(jToken is JValue, $"Property {propertyName} is not a single value");
            var jValue = jToken as JValue;
            Assert.IsNotNull(jValue.Value, $"Property {propertyName} is null");
            Assert.IsTrue(IsDateTime(jValue), $"Property {propertyName} is not a datetime");
            Assert.IsTrue(DateTime.TryParse(jValue.Value.ToString(), out DateTime convertedValue), $"Property {propertyName} is not a valid datetime");

            if (convertedValue < minValue)
            {
                Assert.Fail($"Property {propertyName} is less than {minValue}");
            }

            if (convertedValue > maxValue)
            {
                Assert.Fail($"Property {propertyName} is greater than {maxValue}");
            }

        }

        private JToken FindProperty(string name)
        {
            ValidateResponse();
            var jToken = Response.SelectToken($"$.{name}");
            if (!UseNullForMissingProperties)
            {
                Assert.IsNotNull(jToken, $"Property {name} not found in the response");
            }

            return jToken;
        }

        private JToken FindJPath(string jPath)
        {
            var currentResponse = GetCurrentResponse();
            Assert.IsNotNull(currentResponse, "Content does not have a value");
            var jToken = currentResponse.SelectToken(jPath);
            
            if (!UseNullForMissingProperties)
            {
                Assert.IsNotNull(jToken, $"JPath {jPath} not found in the response");
            }

            return jToken;
        }

        private JToken GetCurrentResponse()
        {
            if (Response != null)
            {
                return Response;
            }

            if (ArrayResponse != null)
            {
                return ArrayResponse;
            }
            
            return null;
        }

        private static bool IsDateTime(JToken jToken)
        {
            return jToken.Type == JTokenType.Date;
        }

        private static bool IsNumber(JToken jToken)
        {
            return jToken.Type == JTokenType.Float || jToken.Type == JTokenType.Integer;
        }

        private bool IsBoolean(JToken jToken)
        {
            return jToken.Type == JTokenType.Boolean;
        }
    }
}
