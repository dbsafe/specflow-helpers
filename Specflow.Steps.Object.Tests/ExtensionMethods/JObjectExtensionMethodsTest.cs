using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using Specflow.Steps.Object.ExtensionMethods;

namespace Specflow.Steps.Object.Tests.ExtensionMethods
{
    [TestClass]
    public class JObjectExtensionMethodsTest
    {
        private JObject _target = new JObject();
        public TestContext TestContext { get; set; }

        [TestCleanup]
        public void CleanUp()
        {
            var message = $"{_target}";
            Print(message);
        }

        [TestMethod]
        public void SetProperty_Given_a_text_Property_must_be_in_the_json()
        {
            _target.SetProperty("PropA", "value-a");
            _target.SetProperty("PropB", "value-b");

            var actual = _target.ToString();

            var expected = @"{
  ""PropA"": ""value-a"",
  ""PropB"": ""value-b""
}";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SetProperty_Given_a_number_Property_must_be_in_the_json()
        {
            _target.SetProperty("PropA", 10);
            _target.SetProperty("PropB", 10.1m);

            var actual = _target.ToString();

            var expected = @"{
  ""PropA"": 10.0,
  ""PropB"": 10.1
}";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SetProperty_Given_an_array_of_texts_Property_must_be_in_the_json()
        {
            _target.SetProperty("PropA", new string[] { "value-a", "value-b" });

            var actual = _target.ToString();

            var expected = @"{
  ""PropA"": [
    ""value-a"",
    ""value-b""
  ]
}";

            Assert.AreEqual(expected, actual);
        }

        private void Print(string message)
        {
            TestContext.WriteLine(message);
        }
    }
}
