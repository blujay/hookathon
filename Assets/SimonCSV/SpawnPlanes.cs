using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlanes : MonoBehaviour
{
    public GameObject PlanePrefab;
    public int Count;
    public TextAsset SourceData;
    public List<string> FieldNamesToDisplay = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < Count; i++)
        {
            SpawnObjectWithData();
        }
    }

    public void SpawnObjectWithData()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
