using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking.Types;
using UnityEngine.Rendering.PostProcessing;

public class SnapshotTool : MonoBehaviour
{
#if  UNITY_EDITOR

    public List<GameObject> queue = new List<GameObject>();

    public int layer;
    public float outline;
    public int size = 256;

    public float distance = 4;
    public Vector3 center = Vector3.zero;
    public float angleX = 30;
    public float angleY = 45;



    private void OnDrawGizmosSelected()
    {
        Camera camera = Camera.main;

        Quaternion rottation = Quaternion.Euler(-angleX, angleY, 0);
        Vector3 position = center + rottation * new Vector3(0, 0, -distance);
        camera.transform.position = position;
        camera.transform.rotation = rottation;
    }

    [ContextMenu("CreateIcon")]
    public void Take()
    {
        StartCoroutine(DoQueue());
    }

    private IEnumerator DoQueue()
    {
        foreach (var item in queue)
        {
            yield return DoTake(item);
        }
    }

    public Texture2D colorTexture;
    public Texture2D maskTexture;

    public void ApplyLayer(GameObject gameObject, int layer)
    {
        var tranforms = gameObject.GetComponentsInChildren<Transform>();
        foreach (var tranform in tranforms)
        {
            tranform.gameObject.layer = layer;
        }
    }

    private IEnumerator DoTake(GameObject gameObject)
    {
        LayerMask layerMask = 1 << layer;
        int tempLayer = gameObject.layer;
        string name = gameObject.name;

        ApplyLayer(gameObject, layer);

        Camera.main.GetComponent<PostProcessLayer>().enabled = true;
        SnapshotUtility.Result result = new SnapshotUtility.Result();

        yield return SnapshotUtility.DoSnapshot(size, size, name, Camera.main, layerMask, true, result);
        colorTexture = result.texture;

        Camera.main.GetComponent<PostProcessLayer>().enabled = false;

        yield return SnapshotUtility.DoSnapshot(size, size, name, Camera.main, layerMask, true, result);
        maskTexture = result.texture;


        ApplyLayer(gameObject, tempLayer);

        Shader shader = Shader.Find("Hidden/Outline");
        Material material = new Material(shader);
        material.SetFloat("_Size", outline);

        RenderTexture renderTex = RenderTexture.GetTemporary(
             size,
             size, 0);

        Shader.SetGlobalTexture("_MaskTex", maskTexture);
        Graphics.Blit(colorTexture, renderTex, material);

        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;

        colorTexture.name = name;
        colorTexture.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        colorTexture.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);



        var bytes = colorTexture.EncodeToPNG();
        var path = AssetDatabase.GenerateUniqueAssetPath($"Assets/{name}.png");
        Debug.Log("Icon Create At Path: " + path);
        File.WriteAllBytes(path, bytes);
        AssetDatabase.Refresh();

        DestroyImmediate(colorTexture);
        DestroyImmediate(maskTexture);

        Camera.main.GetComponent<PostProcessLayer>().enabled = true;

        yield return 0;
    }

    public Texture2D AddOutline(Texture2D texture)
    {
        Shader shader = Shader.Find("Hidden/Blur");
        Material verticalBlur = new Material(shader);
        verticalBlur.SetVector("_Size", new Vector4(0, outline, 0, 0));
        Material horizontalBlur = new Material(shader);
        horizontalBlur.SetVector("_Size", new Vector4(outline, 0, 0, 0));

        RenderTexture temp0 = RenderTexture.GetTemporary(
            size,
            size, 0);

        RenderTexture temp1 = RenderTexture.GetTemporary(
    size,
    size, 0);

        //        RenderTexture temp3 = RenderTexture.GetTemporary(
        //size,
        //size, 0);

        //        RenderTexture temp3 = RenderTexture.GetTemporary(
        //size,
        //size, 0);


        Graphics.Blit(texture, temp0, verticalBlur);
        Graphics.Blit(temp0, temp1, horizontalBlur);

        //Graphics.Blit(temp1, temp3, verticalBlur);
        //Graphics.Blit(temp3, temp0, horizontalBlur);

        //Graphics.Blit(temp0, temp1, verticalBlur);
        //Graphics.Blit(temp1, temp3, horizontalBlur);

        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = temp1;
        Texture2D readableText = new Texture2D(size, size);
        readableText.name = name;
        readableText.ReadPixels(new Rect(0, 0, size, size), 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;

        //Graphics.Blit(temp1, temp1, horizontalBlur);


        RenderTexture.ReleaseTemporary(temp0);
        RenderTexture.ReleaseTemporary(temp1);
        //RenderTexture.ReleaseTemporary(temp3);

        return readableText;
    }

#endif
}
