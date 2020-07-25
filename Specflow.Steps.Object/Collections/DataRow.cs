using Newtonsoft.Json.Linq;
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
            var ks = Values.Where(a => keyNames.Contains(a.Name)).Select(a => a.Value).ToArray();
            return string.Join("_", ks);
        }

        public string[] GetKeyPropertyNames()
        {
            return Values.Where(a => a.IsKey).Select(a => a.Name).ToArray();
        }
    }
}
