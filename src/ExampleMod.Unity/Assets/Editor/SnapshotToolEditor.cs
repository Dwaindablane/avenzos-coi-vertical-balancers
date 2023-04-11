using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Mafi.Unity.InputControl.Inspectors.Buildings;
using Mafi.Serialization.Generators;

[CustomEditor(typeof(SnapshotTool), true), DisallowMultipleComponent]
public class SnapshotToolEditor : Editor
{
    private RenderTexture renderTexture;
    private SnapshotTool snapshotTool => target as SnapshotTool;
    private void OnEnable()
    {
        //renderTexture = new RenderTexture(snapshotTool.size, snapshotTool.size, 24);
        //Camera.main.targetTexture = renderTexture;
    }

    private void OnDisable()
    {
        //Camera.main.targetTexture = null;
        //if (renderTexture != null)
        //{
        //    renderTexture.Release();
        //    DestroyImmediate(renderTexture);
        //}
    }

    public override bool HasPreviewGUI()
    {
        return true;
    }

    public override void DrawPreview(Rect previewArea)
    {
        EditorGUI.DrawTextureTransparent(previewArea, Camera.main.targetTexture, ScaleMode.ScaleToFit, 1f);
    }
}
