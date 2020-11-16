using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class SaveRenderTextureToDeskTop : MonoBehaviour
{
    private const string SAVE_PATH = "Images/LevelPreviews";

    [MenuItem("Logical_Tanks/Save Level Preview")]
    static void SaveTexture (MenuCommand command)
    {
        Camera camera = GameObject.FindGameObjectWithTag("MapCamera").GetComponent<Camera>();
        RenderTexture oldRT = camera.targetTexture;
        RenderTexture tempRT = new RenderTexture(512, 512, oldRT.depth, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);

        var tex = new Texture2D(tempRT.width, tempRT.height, TextureFormat.ARGB32, false);
        camera.targetTexture = tempRT;
        camera.Render();
        RenderTexture.active = tempRT;

        tex.ReadPixels(new Rect(0, 0, tempRT.width, tempRT.height), 0, 0);
        tex.Apply();

        GameManager gm = GameObject.FindObjectOfType<GameManager>();
        string fileName = string.Format("{0} {1}.png", "LevelPreview", gm.LevelNumber);
        System.IO.File.WriteAllBytes(System.IO.Path.Combine(Application.dataPath, SAVE_PATH, fileName), tex.EncodeToPNG());

        DestroyImmediate(tex);

        camera.targetTexture = oldRT;
        camera.Render();
        RenderTexture.active = oldRT;

        DestroyImmediate(tempRT);

        AssetDatabase.Refresh();
    }
}
