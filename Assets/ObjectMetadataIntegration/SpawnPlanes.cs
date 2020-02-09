using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEditor;

/// <summary>
/// Simon Wilson | 25-01-2019 | si.j.wilson@gmail.com
/// </summary>
public class SpawnPlanes : MonoBehaviour
{
    public MetaImage PlanePrefab;
    public int Count;
    public TextAsset SourceData;
    public List<string> FieldNamesToDisplay = new List<string>();
    public Dictionary<string, Texture> TextureNames = new Dictionary<string, Texture>();
    public string ImageFolderPath;

    public float SpawnRange = 4f;

    void Start()
    {
        Test();
    }

    public static void SpawnObjectWithData(MetaImage prefab, string textureDirPath, DataRow metadataRecord, int fileNameField, int[] showFields, float spawnRange = 8f, bool faceOrigin = true)
    {
        // Get a random position on a the surface of a sphere of radius spawnRange
        Vector3 spawnPosition = Random.onUnitSphere * spawnRange;
        // Set the rotation of the spawned object
        Vector3 spawnRotation = Vector3.zero;
        
        if (faceOrigin)
        {
            spawnRotation = Vector3.zero - spawnPosition;
        }
        else
        {
            // Use the default rotation
        }

        SpawnObjectWithData(prefab, textureDirPath, metadataRecord, fileNameField, showFields, spawnPosition, spawnRotation);

    }

    public static void SpawnObjectWithData(MetaImage prefab, string textureDirPath, DataRow metadataRecord, int fileNameField, int[] showFields, Vector3 position, Vector3 rotation)
    {
        // Instantiate an object with the MetaImage component
        MetaImage spawnObject = Instantiate(prefab,position, Quaternion.Euler(rotation));

        // Use the spawned object's MetaImage to set its image and metadata
        string filename = metadataRecord[fileNameField].ToString();
        string filePath = textureDirPath + "\\" + filename;
        spawnObject.SetTexture(filePath);
    }


    public void Test()
    {
        //
        DataTable table = MetadataLoader.LoadCSV(SourceData);

        int[] testFields = { 1, 2 };

        string sourcePath = AssetDatabase.GetAssetPath(SourceData);
        FileInfo fileInfo = new FileInfo(sourcePath);
        string directory = fileInfo.DirectoryName;

        for (int i = 0; i < 5; i++)
        {            
            DataRow dataRow = table.Rows[i];
            SpawnObjectWithData(PlanePrefab, directory, dataRow, 5, testFields, SpawnRange);
        }

    }


}
