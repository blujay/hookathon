using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEditor;
/// <summary>
/// Simon Wilson | 25-01-2019 | si.j.wilson@gmail.com
/// </summary>
public static class MetadataLoader
{
    //public TextAsset Source;
    //public const char Separator = '|';

    public const char cSep = '|';
    public const string sSep = "|";
    
    public static DataTable LoadCSV(TextAsset source, int primaryKeyColumn = 0, char separator = cSep, string tableName = default, bool preview = true)
    {
        // Create the returned table, name it after the source TextAsset if a tableName is not provided
        string useName = tableName == default ? source.name : tableName;
        DataTable Table = new DataTable(useName);

        // Load the source TextAsset using a new StreamReader. StreamReader takes an absolute path as an argument.
        StreamReader streamReader = new StreamReader(AssetDatabase.GetAssetPath(source));

        // Create columns
        string headerRow = streamReader.ReadLine();
        List<string> headers = new List<string>(headerRow.Split(separator));
        for(int i = 0; i < headers.Count; i++)
        {
            // Currently it we expect all fields to be of type string
            // A new method could be used to get the type of each column and type appropriately
            Table.Columns.Add(headers[i], typeof(string));
        }

        // Set the table's primary key
        DataColumn[] keys = new DataColumn[1] { Table.Columns[primaryKeyColumn] };
        Table.PrimaryKey = keys;

        // For each row in the text stream add a row to the table
        while (!streamReader.EndOfStream)
        {
            string rowText = streamReader.ReadLine(); //Get the text of the next line in the text stream
            List<string> fields = new List<string>(rowText.Split(separator)); //Break the rowText into fields

            // Check that the row's key is unique; if it is, then add a new row
            string newKey = fields[primaryKeyColumn];
            if(Table.Rows.Find(newKey) == null)
            {
                DataRow newRow = Table.NewRow(); //Create a new row
                
                //Add all the fields to the new record
                for (int i = 0; i < fields.Count; i++)
                {
                    newRow[i] = fields[i];
                }

                //Add the new row to the table
                Table.Rows.Add(newRow);
            }
            else
            {
                Debug.Log($"{Table.TableName} contains key {newKey} so a row was not loaded to the table.");
            }


        }
        
        // If preview, then call the .Preview extension method
        if (preview)
        {
            Table.Preview();
        }

        Debug.Log($"{Table.Rows.Count} rows loaded from {source.name}");
        return Table;
    }

    // An extension method to print info about the table to the console
    public static void Preview(this DataTable table, int rows = 5)
    {
        List<string> output = new List<string>();
        foreach (DataColumn dc in table.Columns)
        {
            output.Add(dc.ColumnName);
        }
        Debug.Log(string.Join(sSep.ToString(), output));
        
        for(int i = 0; i < rows; i++)
        {
            Debug.Log(table.Rows[i].Preview());
        }
    }

    // An extension method that takes a dataRow and returns a string describing that row, using the .ToString() method of each item in itemArray[]
    public static string Preview(this DataRow dataRow)
    {
        List<string> output = new List<string>();
        for (int i = 0; i < dataRow.ItemArray.Length; i++)
        {
            output.Add(dataRow[i].ToString());
        }
        return string.Join(sSep, output);
    }

    // An extension method that takes a dataColumn and returns a string describing the first N items in that column using the .ToString() method of each item
    public static string Preview(this DataColumn dataColumn, int rows = 10)
    {
        List<string> output = new List<string>();
        for(int i = 0; i < dataColumn.Table.Rows.Count && i < rows; i++)
        {
            output.Add(dataColumn.Table.Rows[i][dataColumn].ToString());
        }
        return string.Join(sSep, output);
    }

    // An extension method that takes a key and returns a bool if the table contains that key. If true is returned, 
    // the matching row is passed back as an out param
    public static bool TryGetRow(this DataTable table, string key, out DataRow dataRow)
    {
        DataRow outRow = table.Rows.Find(key);
        if(outRow == null)
        {
            dataRow = default;
            return false;
        }
        else
        {
            dataRow = outRow;
            return true;
        }

    }

    // An extension method that takes a row and returns a dictionary of selected values, using column names as keys
    public static Dictionary<string, object> ToDictionary(this DataRow row, DataColumnCollection returnColumns)
    {
        Dictionary<string, object> retval = new Dictionary<string, object>();
        for(int i = 0; i < returnColumns.Count; i++)
        {
            DataColumn column = returnColumns[i];
            object v = row[column];
            retval.Add(column.ColumnName, v);
        }
        return retval;     
    }

    

}
