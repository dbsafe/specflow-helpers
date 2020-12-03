using System;
using System.Linq;

namespace Specflow.Steps.Object.Collections
{
    public static class DataCompare
    {
        public static bool Compare(DataCollection expected, DataCollection actual, out string message)
        {
            if (expected.Rows.Length != actual.Rows.Length)
            {
                message = $"The number of items does not match. Expected: {expected.Rows.Length}, Actual: {actual.Rows.Length}";
                return false;
            }

            for (int i = 0; i < expected.Rows.Length; i++)
            {
                var expectedRow = expected.Rows[i];
                if (!Compare(i, expectedRow, actual, out message))
                {
                    return false;
                }
            }

            message = string.Empty;
            return true;
        }

        private static bool Compare(int index, DataRow expectedRow, DataCollection actual, out string message)
        {
            var keyPropertyNames = expectedRow.GetKeyPropertyNames();
            var composedKey = expectedRow.GetComposedKey(keyPropertyNames);
            if (string.IsNullOrEmpty(composedKey))
            {
                return CompareUsingIndex(index, expectedRow, actual, out message);
            }
            else
            {
                return CompareUsingComposedKey(keyPropertyNames, composedKey, index, expectedRow, actual, out message);
            }
        }


        private static bool CompareUsingComposedKey(
            string[] keyPropertyNames,
            string composedKey,
            int index,
            DataRow expectedRow,
            DataCollection actual,
            out string message)
        {
            var actualRowsFound = actual.Rows.Where(a => a.GetComposedKey(keyPropertyNames) == composedKey).ToArray();
            if (actualRowsFound.Length == 0)
            {
                message = $"Expected row at the position {index + 1} was not found in actual";
                return false;
            }

            if (actualRowsFound.Length > 1)
            {
                message = $"Expected row key at the position {index + 1} was found {actualRowsFound.Count()} times in actual";
                return false;
            }

            if (!Compare(expectedRow, actualRowsFound[0], out string messageAboutRows))
            {
                message = $"The rows at position {index + 1} are different.\n{messageAboutRows}";
                return false;
            }

            message = string.Empty;
            return true;
        }

        private static bool CompareUsingIndex(int index, DataRow expectedRow, DataCollection actual, out string message)
        {
            if (!Compare(expectedRow, actual.Rows[index], out string messageAboutRows))
            {
                message = $"The rows at position {index + 1} are different.\n{messageAboutRows}";
                return false;
            }

            message = string.Empty;
            return true;
        }

        public static bool Compare(DataRow expected, DataRow actual, out string message)
        {
            if (expected.Values.Length != actual.Values.Length)
            {
                message = $"The number of items does not match. Expected: <{expected.Values.Length}>, Actual: <{actual.Values.Length}>";
                return false;
            }

            for (int i = 0; i < expected.Values.Length; i++)
            {
                var expectedValue = expected.Values[i];
                var actualValue = actual.Values.Where(a => a.Name == expectedValue.Name).ToArray();

                if (actualValue.Length == 0)
                {
                    message = $"Property '{expectedValue.Name}' not found";
                    return false;
                }

                if (actualValue.Length > 1)
                {
                    message = $"Duplicated property '{expectedValue.Name}'";
                    return false;
                }

                if (!Compare(expectedValue, actualValue[0], out message))
                {
                    return false;
                }
            }

            message = string.Empty;
            return true;
        }

        private static bool CompareNumber(DataCell expected, DataCell actual, out string message)
        {
            var expectedDecimal = (decimal)expected.Value;
            if (!decimal.TryParse(actual.Value.ToString(), out decimal actualDecimal))
            {
                message = $"Property: {expected.Name}. Actual <{expected.Value}> is not a valid Number";
                return false;
            }

            if (expectedDecimal != actualDecimal)
            {
                message = $"Property: {expected.Name}. Expected <{expected.Value}>, Actual: <{actual.Value}>";
                return false;
            }

            message = string.Empty;
            return true;
        }

        private static bool CompareDateTime(DataCell expected, DataCell actual, out string message)
        {
            var expectedDateTime = (DateTime)expected.Value;
            if (!DateTime.TryParse(actual.Value.ToString(), out DateTime actualDateTime))
            {
                message = $"Property: {expected.Name}. Actual <{expected.Value}> is not a valid DateTime";
                return false;
            }

            if (expectedDateTime != actualDateTime)
            {
                message = $"Property: {expected.Name}. Expected <{expected.Value}>, Actual: <{actual.Value}>";
                return false;
            }

            message = string.Empty;
            return true;
        }

        private static bool CompareText(DataCell expected, DataCell actual, out string message)
        {
            if (expected.Value == null && actual.Value == null)
            {
                message = string.Empty;
                return true;
            }

            if (expected.Value == null || actual.Value == null)
            {
                message = $"Property: {expected.Name}. Expected <{NormalizeText(expected.Value)}>, Actual: <{NormalizeText(actual.Value)}>";
                return false;
            }

            if (expected.Value.ToString() != actual.Value.ToString())
            {
                message = $"Property: {expected.Name}. Expected <{NormalizeText(expected.Value)}>, Actual: <{NormalizeText(actual.Value)}>";
                return false;
            }

            message = string.Empty;
            return true;
        }

        private static string NormalizeText(object value)
        {
            if (value == null)
            {
                return "null";
            }
            else
            {
                return value.ToString();
            }
        }

        public static bool Compare(DataCell expected, DataCell actual, out string message)
        {
            if ((expected.IsNull && actual.Value == null)
                || expected.IsIgnore)
            {
                message = string.Empty;
                return true;
            }

            if (expected.IsNull && actual.Value != null)
            {
                message = $"Property: {expected.Name} is expected to be null. Actual: <{actual.Value}>";
                return false;
            }

            if (expected.Type == typeof(DateTime))
            {
                return CompareDateTime(expected, actual, out message);
            }

            if (expected.Type == typeof(decimal) || expected.Type == typeof(int))
            {
                return CompareNumber(expected, actual, out message);
            }

            return CompareText(expected, actual, out message);
        }
    }
}
