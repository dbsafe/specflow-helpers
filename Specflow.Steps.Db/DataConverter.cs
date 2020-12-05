using DbSafe.FileDefinition;
using System.Linq;
using System.Xml.Linq;
using TechTalk.SpecFlow;

namespace Specflow.Steps.Db
{
    public static class DataConverter
    {
        public static DatasetElement BuildDatasetElementFromSpecFlowTable(string tableName, Table table, bool setIdentityInsert)
        {
            DatasetElement result = new DatasetElement
            {
                Data = new XElement("data"),
                Table = tableName,
                Name = tableName,
                SetIdentityInsert = setIdentityInsert
            };

            var headers = table.Header.ToArray();

            foreach (var row in table.Rows)
            {
                XElement xmlRow = new XElement("row");
                result.Data.Add(xmlRow);

                for (int i = 0; i < headers.Length; i++)
                {
                    if (row[i] == "[NULL]")
                    {
                        continue;
                    }

                    var columnName = headers[i];
                    var attribute = new XAttribute(columnName, row[i]);
                    xmlRow.Add(attribute);
                }
            }

            return result;
        }
    }
}