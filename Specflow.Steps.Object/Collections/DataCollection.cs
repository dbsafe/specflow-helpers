using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;

namespace Specflow.Steps.Object.Collections
{
    public class DataCollection
    {
        public DataRow[] Rows { get; set; }

        public static DataCollection Load(Table specFlowTable)
        {
            var rows = new List<DataRow>();
            foreach (var specFlowRow in specFlowTable.Rows)
            {
                var row = DataRow.Load(specFlowRow);
                rows.Add(row);
            }

            return new DataCollection
            {
                Rows = rows.ToArray(),
            };
        }

        public static DataCollection Load(JEnumerable<JToken> jTokens)
        {
            var rows = new List<DataRow>();
            foreach (var jToken in jTokens)
            {
                var row = DataRow.Load(jToken);
                rows.Add(row);
            }

            return new DataCollection { Rows = rows.ToArray() };
        }
    }
}
