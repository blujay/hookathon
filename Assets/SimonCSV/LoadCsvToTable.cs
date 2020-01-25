using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class LoadCsvToTable : MonoBehaviour
{
    public TextAsset Source;
    public const char Separator = '|';
    public static DataTable LoadDataset(TextAsset Source)
    {
        DataTable Table = new DataTable(Source.name);
        StreamReader streamReader = new StreamReader( AssetDatabase.GetAssetPath(Source));
        
        // Set columns
        string headerRow = streamReader.ReadLine();
        List<string> headers = new List<string>(headerRow.Split(Separator));
        for(int i = 0; i < headers.Count; i++)
        {
            Table.Columns.Add(headers[i], typeof(string));
        }
        Debug.Log(Table.Columns.Count);

        while (!streamReader.EndOfStream)
        {
            string rowText = streamReader.ReadLine();
            DataRow newRow = Table.NewRow();
            List<string> fields = new List<string>(rowText.Split(Separator));

            for(int i = 0; i < fields.Count; i++)
            {
                newRow[i] = fields[i];
            }

            Table.Rows.Add(newRow);
        }

        Debug.Log(Table.Rows.Count);
        return Table;
        // Load rows
    }

    // Start is called before the first frame update
    void Start()
    {
        DataTable what = LoadDataset(Source);
        Debug.Log($"{what.Columns.Count} {what.Rows.Count}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
