using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEditor;
/// <summary>
/// Simon Wilson | 25-01-2019 | si.j.wilson@gmail.com
/// </summary>
public class SpawnPlanes : MonoBehaviour
{
    public GameObject PlanePrefab;
    public int Count;
    public TextAsset SourceData;
    public List<string> FieldNamesToDisplay = new List<string>();
    public Dictionary<string, Texture> TextureNames = new Dictionary<string, Texture>();
    public string ImageFolderPath;

    //Test
    public List<string> testKeys;

    // Start is called before the first frame update
    void Start()
    {
        Test();
        //DataTable dataTable = MetadataLoader.LoadCSV(SourceData);
        //for(int i = 0; i < Count; i++)
        //{
        //    SpawnObjectWithData();
        //}
        //TextureNames = LoadTextures(ImageFolderPath);
    }

    public void SpawnObjectWithData()
    {

    }

    public Dictionary<string, Texture> LoadTextures(string path)
    {
        Dictionary<string, Texture> retval = new Dictionary<string, Texture>();
        //Texture2D[] textures = Resources.LoadAll(path, typeof(Texture2D)) as Texture2D[];
        //Debug.Log($"{textures.Length} loaded");

        DirectoryInfo directoryInfo = new DirectoryInfo(path);
        object[] what = directoryInfo.GetFiles();
        
        Texture[] textures = what.OfType<Texture>().ToArray();
        Debug.Log($"{what.Length} {textures.Length}" );
        return retval;
    }

    public void Test()
    {
        DataTable table = MetadataLoader.LoadCSV(SourceData);
        for(int i = 0; i < testKeys.Count; i++)
        {
            DataRow dataRow;
            if (table.TryGetRow(testKeys[i], out dataRow))
            {
                Debug.Log(string.Join(",",dataRow.ItemArray));
            }
            else
            {
                Debug.Log($"Key {testKeys[i]} was not found in the table");
            }
            
        }

    }

}
