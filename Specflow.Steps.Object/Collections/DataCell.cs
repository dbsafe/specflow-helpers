using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Specflow.Steps.Object.Collections
{
    public class DataCell
    {
        public Type Type { get; internal set; }
        public string Name { get; internal set; }
        public object Value { get; internal set; }
        public bool IsNull { get; internal set; }
        public bool IsIgnore { get; internal set; }
        public bool IsKey { get; internal set; }
        public bool IsExpected { get; internal set; }
        public string Header { get; internal set; }

        public static DataCell Load(JToken jToken)
        {
            if (jToken.Type != JTokenType.Property)
            {
                throw new CollectionException($"Parameter {nameof(jToken)} must be a property", jToken);
            }

            var result = new DataCell { Name = (jToken as JProperty).Name };
            if (jToken.HasValues)
            {
                if (jToken.First is JValue jValue)
                {
                    result.Value = jValue.Value?.ToString();
                }
            }

            return result;
        }

        public static DataCell Load(KeyValuePair<string, string> specFlowValue)
        {
            var header = specFlowValue.Key;
            var headerParts = header.Split(':');
            var result = new DataCell
            {
                Header = header,
                Name = headerParts[0],
                IsExpected = true,
                IsNull = specFlowValue.Value != null && specFlowValue.Value.Equals("[NULL]", StringComparison.InvariantCultureIgnoreCase),
                IsIgnore = specFlowValue.Value != null && specFlowValue.Value.Equals("[IGNORE]", StringComparison.InvariantCultureIgnoreCase)
            };

            switch (headerParts.Length)
            {
                case 1:
                    result.Name = headerParts[0];
                    result.Type = typeof(string);
                    break;

                case 2:
                    result.Name = headerParts[0];

                    if (headerParts[1].Equals("Key", StringComparison.InvariantCultureIgnoreCase))
                    {
                        result.IsKey = true;
                    }
                    else
                    {
                        result.Type = DecodeType(headerParts[1]);
                    }

                    break;

                case 3:
                    result.Name = headerParts[0];

                    if (headerParts[1].Equals("Key", StringComparison.InvariantCultureIgnoreCase))
                    {
                        result.IsKey = true;
                    }
                    else
                    {
                        throw new CollectionException($"Invalid header format. Header {header}. When a header has three elements the second element must be 'Key'");
                    }

                    result.Type = DecodeType(headerParts[2]);
                    break;

                default:
                    throw new CollectionException($"Invalid header '{header}'");
            }

            result.Value = DecodeValue(result, specFlowValue.Value);
            return result;
        }

        private static object DecodeValue(DataCell valueResult, string value)
        {
            if (valueResult.IsNull)
            {
                return null;
            }

            if (valueResult.Type == typeof(DateTime))
            {
                if (!DateTime.TryParse(value, out DateTime decodedDatetime))
                {
                    var message = $"Property: {valueResult.Name}. Expected <{value}> is not a valid DateTime";
                    throw new CollectionException(message);
                }

                return decodedDatetime;
            }

            if (valueResult.Type == typeof(decimal))
            {
                if (!decimal.TryParse(value, out var decodedDecimal))
                {
                    var message = $"Property: {valueResult.Name}. Expected <{value}> is not a valid Number";
                    throw new CollectionException(message);
                }

                return decodedDecimal;
            }

            if (valueResult.Type == typeof(int))
            {
                if (!int.TryParse(value, out var decodedInt))
                {
                    var message = $"Property: {valueResult.Name}. Expected <{value}> is not a valid Integer";
                    throw new CollectionException(message);
                }

                return decodedInt;
            }

            if (valueResult.Type == typeof(bool))
            {
                if (!bool.TryParse(value, out var decodedBoolean))
                {
                    var message = $"Property: {valueResult.Name}. Expected <{value}> is not a valid Boolean";
                    throw new CollectionException(message);
                }

                return decodedBoolean;
            }

            return value;
        }

        private static Type DecodeType(string type)
        {
            switch (type)
            {
                case "DateTime":
                    return typeof(DateTime);
                case "Number":
                    return typeof(decimal);
                case "Integer":
                    return typeof(int);
                case "Boolean":
                    return typeof(bool);
                case "Guid":
                    return typeof(Guid);
                case "":
                    return typeof(string);
            }

            throw new CollectionException($"Invalid type '{type}'");
        }
    }
}
