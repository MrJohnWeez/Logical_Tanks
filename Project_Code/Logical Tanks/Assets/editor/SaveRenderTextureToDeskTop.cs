using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class SaveRenderTextureToDeskTop : MonoBehaviour
{    
    [MenuItem("Logical_Tanks/Save Selected RenderTexture To DeskTop")]
    static void SaveTexture () {
        RenderTexture textureToSave = Selection.activeObject as RenderTexture;
        if(textureToSave)
        {
            byte[] bytes = toTexture2D(textureToSave).EncodeToPNG();
            System.IO.File.WriteAllBytes("C:\\Users\\John\\Desktop\\test.png", bytes);
        }
    }
    
    static Texture2D toTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGB9e5Float, false, true);
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }
}
