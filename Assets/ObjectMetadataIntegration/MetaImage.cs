using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Linq;

/// <summary>
/// Simon Wilson | 28-01-2019 | si.j.wilson@gmail.com
/// </summary>
public class MetaImage : MonoBehaviour
{
    public Texture2D Texture { get; protected set; }

    public Renderer Renderer => renderer;
    [SerializeField]
    private Renderer renderer;

    public Text Text => text;
    [SerializeField]
    private Text text;

    public string TextureUri;

    public void SetMetadata(Dictionary<string, object> metadata, string separator = ": ")
    {
        List<string> formatted = (from KeyValuePair<string, object> kvp in metadata
                                  select string.Join(separator, kvp.Key, kvp.Value)).ToList();
        string joined = string.Join("\n", formatted);
        text.text = joined;
    }

    public IEnumerator SetTexture()
    {
        yield return StartCoroutine(LoadImage());
    }

    public void SetTexture(string textureUri)
    {
        TextureUri = textureUri;
        StartCoroutine(SetTexture());
    }

    public void MatchSize(float scale = 0.01f, float maxX = -1f, float maxY = -1f, float fixedZ = 0.2f)
    {
        Vector2 imageSize = new Vector2(Texture.width, Texture.height);
        Vector2 scaled = imageSize * scale;
        transform.localScale = new Vector3(scaled.x, scaled.y, fixedZ);
    }

    public IEnumerator LoadImage()
    {
        // Check that the file exists by getting its FileInfo
        FileInfo fileInfo = new FileInfo(TextureUri);

        // For debugging, log certain of the file's properties to the console
        Debug.Log($"{fileInfo.FullName} exists? {fileInfo.Exists} size: {fileInfo.Length}");

        // Use a UnityWebRequest to get the file as a texture (this is half-cludge, half-feature;_
        // UnityWebRequest has a method to load a texture from an an image with a URI_
        // this could also be used to load images from a URL: possibly useful in a GLAM context if a DLS is in use.
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(TextureUri);

        //request.SendWebRequest();
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError || request.error != null) // request.error != null is probably unneccesary, remove after testing
        {
            // The request returned an error, log it to the console
            Debug.LogError(request.error);
        }
        else
        {
            // The request has succeeded, get the texture from the DownloadHandlertexture returned by the request and set the texture of the render's material
            Texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            renderer.material.mainTexture = Texture;
        }
        MatchSize();

    }


}
