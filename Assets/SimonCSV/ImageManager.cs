using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ImageManager : MonoBehaviour
{
    public static ImageManager Instance => instance;
    private static ImageManager instance;

    public List<Texture> Images;
    // Start is called before the first frame update
    void Start()
    {
        //Basic singleton
        instance = this;
        Test();
    }

    public static Texture RandomImage()
    {
        return Instance.Images.OrderBy(t => Random.value).First();
    }

    public static void Test()
    {
        for(int i = 0; i < 10; i++)
        {
            Debug.Log(RandomImage().name);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
