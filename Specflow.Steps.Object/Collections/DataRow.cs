using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Specflow.Steps.Object.Collections
{
    public class DataRow
    {
        public DataCell[] Values { get; set; }

        public static DataRow Load(TableRow specFlowRow)
        {
            var values = new List<DataCell>();
            foreach (var specFlowValue in specFlowRow)
            {
                var value = DataCell.Load(specFlowValue);
                values.Add(value);
            }

            return new DataRow
            {
                Values = values.ToArray(),
            };
        }

        public static DataRow Load(JToken jToken)
        {
            var values = new List<DataCell>();
            foreach (var item in jToken)
            {
                var value = DataCell.Load(item);
                values.Add(value);
            }

            return new DataRow { Values = values.ToArray() };
        }

        public string GetComposedKey(string[] keyNames)
        {
            IEnumerable<string> keys = Enumerable.Empty<string>();
            // add one key at the time to keep the order.
            foreach (var keyName in keyNames)
            {
                var newKey = Values
                    .Where(a => a.Name == keyName)
                    .Select(a => a.Type == typeof(Guid) ? a.Value.ToString().ToLower() : a.Value.ToString());
                keys = keys.Union(newKey);
            }

            return string.Join("_", keys);
        }

        public string[] GetKeyPropertyNames()
        {
            return Values.Where(a => a.IsKey).Select(a => a.Name).ToArray();
        }
    }
}
