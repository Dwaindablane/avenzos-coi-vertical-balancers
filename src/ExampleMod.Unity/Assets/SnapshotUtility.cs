using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class SnapshotUtility
{
#if UNITY_EDITOR

    public class Result
    {
        public Texture2D texture;
    }

    public static IEnumerator DoSnapshot(int width, int height, string name, Camera camera, LayerMask layerMask, bool clearBackground, Result result)
    {
        //int width = camera.pixelWidth;
        //int height = camera.pixelHeight;
        Texture2D texture = new Texture2D(width, height);
        RenderTexture tempRenderTexture = camera.targetTexture;
        RenderTexture renderTexture = new RenderTexture(camera.pixelWidth, camera.pixelHeight, 24);
        int tempCullingMask = camera.cullingMask;
        var tempClearFlag = camera.clearFlags;
        Color tempColor = camera.backgroundColor;

        camera.targetTexture = renderTexture;
        camera.cullingMask = layerMask;
        if (clearBackground)
        {
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = Color.clear;
        }
        
        camera.Render();
        yield return null;
        RenderTexture.active = renderTexture;
        texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        texture.Apply();

        
        RenderTexture.active = null;
        renderTexture.Release();

        camera.cullingMask = tempCullingMask;
        camera.targetTexture = tempRenderTexture;
        camera.clearFlags = tempClearFlag;
        camera.backgroundColor = tempColor;

        result.texture = texture;

        //byte[] bytes = texture.EncodeToPNG();

        //if (bytes != null && bytes.Length > 0)
        //{
        //    //string file = name;
        //    string filePath = $"Assets/{name}.png"; // EditorUtility.SaveFilePanel("Save snaphot", "", file, "png");
        //    if (!string.IsNullOrEmpty(filePath))
        //    {
        //        File.WriteAllBytes(filePath, bytes);
        //    }
        //}

        //bytes = null;

        yield return 0;
    }

#endif
}
