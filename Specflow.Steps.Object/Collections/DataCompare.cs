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
                var areEquals = Compare(i, expectedRow, actual, out message);
                if (!areEquals)
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
                if (CompareUsingComposedKey(keyPropertyNames, composedKey, expectedRow, actual, out message))
                {
                    return true;
                }
                
                message = $"Comparing rows at position {index + 1}.\n{message}";
                return false;
            }
        }

        private static bool IsExpectedValid(DataCell expected, out string message)
        {
            if (expected.Type == typeof(Guid))
            {
                if (!Guid.TryParse(expected.Value.ToString(), out Guid _))
                {
                    message = $"Property: {expected.Name}. Expected <{expected.Value}> is not a valid Guid";
                    return false;
                }
            }

            if (expected.Type == typeof(int) || expected.Type == typeof(decimal))
            {
                if (!decimal.TryParse(expected.Value.ToString(), out decimal _))
                {
                    message = $"Property: {expected.Name}. Expected <{expected.Value}> is not a valid Number";
                    return false;
                }
            }

            if (expected.Type == typeof(DateTime))
            {
                if (!DateTime.TryParse(expected.Value.ToString(), out DateTime actualDateTime))
                {
                    message = $"Property: {expected.Name}. Expected <{expected.Value}> is not a valid DateTime";
                    return false;
                }
            }

            message = string.Empty;
            return true;
        }

        private static bool AreExpectedKeysValid(string[] keyPropertyNames, DataRow expectedRow, out string message)
        {
            foreach (var keyPropertyName in keyPropertyNames)
            {
                var expected = expectedRow.Values.First(a => a.Name == keyPropertyName);
                if (!IsExpectedValid(expected, out message))
                {
                    return false;
                }
            }

            message = string.Empty;
            return true;
        }

        private static bool CompareUsingComposedKey(
            string[] keyPropertyNames,
            string composedKey,
            DataRow expectedRow,
            DataCollection actual,
            out string message)
        {
            if (!AreExpectedKeysValid(keyPropertyNames, expectedRow, out message))
            {
                return false;
            }

            var actualRowsFound = actual.Rows.Where(a => a.GetComposedKey(keyPropertyNames) == composedKey).ToArray();
            if (actualRowsFound.Length == 0)
            {
                message = $"Expected row not found in actual";
                return false;
            }

            if (actualRowsFound.Length > 1)
            {
                message = $"Expected row key found {actualRowsFound.Count()} times in actual";
                return false;
            }

            var areEquals = Compare(expectedRow, actualRowsFound[0], out message);
            if (!areEquals)
            {
                return false;
            }

            message = string.Empty;
            return true;
        }

        private static bool CompareUsingIndex(int index, DataRow expectedRow, DataCollection actual, out string message)
        {
            if (!Compare(expectedRow, actual.Rows[index], out string messageAboutRows))
            {
                message = $"Comparing rows at position {index + 1}.\n{messageAboutRows}";
                return false;
            }

            message = string.Empty;
            return true;
        }

        public static bool Compare(DataRow expected, DataRow actual, out string message)
        {
            var expectsNulls = expected.Values.Any(a => a.IsNull);
            if (!expectsNulls && expected.Values.Length != actual.Values.Length)
            {
                message = $"The number of items does not match. Expected: <{expected.Values.Length}>, Actual: <{actual.Values.Length}>";
                return false;
            }

            for (int i = 0; i < expected.Values.Length; i++)
            {
                var expectedValue = expected.Values[i];
                var actualValue = actual.Values.Where(a => a.Name == expectedValue.Name).ToArray();

                if (expectedValue.IsNull && actualValue.Length == 0)
                {
                    continue;
                }

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

                var areEquals = Compare(expectedValue, actualValue[0], out message);
                if (!areEquals)
                {
                    return false;
                }
            }

            message = string.Empty;
            return true;
        }

        private static bool CompareNumber(DataCell expected, DataCell actual, out string message)
        {
            if (IsActualValueNull(expected, actual, out message))
            {
                return false;
            }

            if (!decimal.TryParse(expected.Value.ToString(), out decimal expectedNumber))
            {
                message = $"Property: {expected.Name}. Expected <{expected.Value}> is not a valid Number";
                return false;
            }

            if (!decimal.TryParse(actual.Value.ToString(), out decimal actualNumber))
            {
                message = $"Property: {expected.Name}. Actual <{actual.Value}> is not a valid Number";
                return false;
            }

            if (expectedNumber != actualNumber)
            {
                message = $"Property: {expected.Name}. Expected <{expected.Value}>, Actual: <{actual.Value}>";
                return false;
            }

            message = string.Empty;
            return true;
        }

        private static bool CompareDateTime(DataCell expected, DataCell actual, out string message)
        {
            if (IsActualValueNull(expected, actual, out message))
            {
                return false;
            }

            var expectedDateTime = (DateTime)expected.Value;
            if (!DateTime.TryParse(actual.Value.ToString(), out DateTime actualDateTime))
            {
                message = $"Property: {expected.Name}. Actual <{actual.Value}> is not a valid DateTime";
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

        private static bool CompareGuid(DataCell expected, DataCell actual, out string message)
        {
            if (IsActualValueNull(expected, actual, out message))
            {
                return false;
            }

            if (!Guid.TryParse(expected.Value.ToString(), out Guid expectedGuid))
            {
                message = $"Property: {expected.Name}. Expected <{expected.Value}> is not a valid Guid";
                return false;
            }

            if (!Guid.TryParse(actual.Value.ToString(), out Guid actualGuid))
            {
                message = $"Property: {expected.Name}. Actual <{actual.Value}> is not a valid Guid";
                return false;
            }

            if (expectedGuid != actualGuid)
            {
                message = $"Property: {expected.Name}. Expected <{expected.Value}>, Actual: <{actual.Value}>";
                return false;
            }

            message = string.Empty;
            return true;
        }

        private static bool IsActualValueNull(DataCell expected, DataCell actual, out string message)
        {
            if (actual.Value is null)
            {
                message = $"Property: {expected.Name}. Expected <{expected.Value}>, Actual is null";
                return true;
            }

            message = string.Empty;
            return false;
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

            if (expected.Type == typeof(Guid))
            {
                return CompareGuid(expected, actual, out message);
            }

            if (expected.Type == typeof(decimal) || expected.Type == typeof(int))
            {
                return CompareNumber(expected, actual, out message);
            }

            return CompareText(expected, actual, out message);
        }
    }
}
