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

        /// <summary>
        /// Used to get the Compose Key from the Actual values
        /// </summary>
        /// <param name="keyNames">Name of the fields that are Key in the Expected values</param>
        /// <returns></returns>
        public string GetComposedKeyByName(IEnumerable<string> keyNames)
        {
            return BuildComposeKey(GetCellsByName(keyNames));
        }

        /// <summary>
        /// Used to get the Compose Key from the Expected values
        /// </summary>
        /// <returns></returns>
        public string GetComposedKey()
        {
            return BuildComposeKey(GetComposedKeyCells());
        }

        private string BuildComposeKey(IEnumerable<DataCell> composeKeyCells)
        {
            var cellKeys = composeKeyCells
                .Select(DataCellToKey);
            return string.Join("/", cellKeys);
        }

        private string DataCellToKey(DataCell dataCell)
        {
            if (dataCell.Type == typeof(Guid))
            {
                return dataCell.Value.ToString().ToLower();
            }

            if (dataCell.Type == typeof(DateTime))
            {
                var dateTime = dataCell.Value.GetType() == typeof(DateTime) ? (DateTime)dataCell.Value : DateTime.Parse(dataCell.Value.ToString());

                return $"{dateTime.Ticks}_{dateTime.Kind}";
            }

            if (dataCell.Type == typeof(DateTimeOffset))
            {
                var dateTime = dataCell.Value.GetType() == typeof(DateTimeOffset) ? (DateTimeOffset)dataCell.Value : DateTimeOffset.Parse(dataCell.Value.ToString());

                return $"{dateTime.UtcTicks}";
            }

            return dataCell.Value.ToString();
        }

        public IEnumerable<DataCell> GetCellsByName(IEnumerable<string> names)
        {
            IEnumerable<DataCell> cells = Enumerable.Empty<DataCell>();

            // add one key at the time to keep the order.
            foreach (var name in names)
            {
                var newKey = Values.Where(a => a.Name == name);
                cells = cells.Union(newKey);
            }

            return cells;
        }

        public IEnumerable<DataCell> GetComposedKeyCells() => Values.Where(a => a.IsKey);

        public IEnumerable<string> GetKeyPropertyNames()
        {
            return GetComposedKeyCells().Select(a => a.Name).ToArray();
        }
    }
}
