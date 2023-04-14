using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Linq;
using Unity.Collections;
using Unity.Mathematics;
using Autodesk.Fbx;
using UnityEditor.VersionControl;

namespace AssetBundleExtractor
{


    public class AssetBundleView : EditorWindow
    {
        public static AssetBundleView Singleton { get; private set; }

        public static void OpenIfNeed()
        {
            if (Singleton == null)
            {
                Open();
            }
            else FocusWindowIfItsOpen<AssetBundleView>();
        }

        public static void Open()
        {
            var window = CreateWindow<AssetBundleView>();
            window.titleContent = new GUIContent("Asset Bundle View");
        }

        private AssetBundle assetBundle;

        private void OnEnable()
        {
            Singleton = this;
            OnLoadAssetBundle();
            GraberProvider.OnLoadAssetBundle += OnLoadAssetBundle;
        }

        private void OnLoadAssetBundle()
        {
            selectIndex = -1;
            assetBundle = GraberProvider.GetAssetBundle();
            if (assetBundle != null)
            {
                CreateTree(ref items, assetBundle);
            }
        }

        private void OnDisable()
        {
            GraberProvider.OnLoadAssetBundle -= OnLoadAssetBundle;
            Singleton = null;
        }

        private class Item
        {
            public string path;
            public string name;
        }

        private List<Item> items = new List<Item>();

        private Style style;

        public class Style
        {
            public GUIStyle select;
            public GUIStyle normal;

            public Style()
            {
                select = new GUIStyle("SelectionRect");
                select.stretchWidth = true;
                normal = new GUIStyle("RectangleToolHighlight");
                normal.stretchWidth = true;
            }
        }

        private void CreateTree(ref List<Item> items, AssetBundle assetBundle)
        {
            Debug.Log(assetBundle);
            if (assetBundle == null) return;

            if (items == null) items.Clear();

            var assets = assetBundle.GetAllAssetNames();

            items = assets.Select(x => new Item() { name = x, path = x }).OrderBy(x => x.name).ToList();
        }


        public void Initialize(AssetBundle assetBundle)
        {
            this.assetBundle = assetBundle;
            CreateTree(ref items, assetBundle);
        }


        private int selectIndex = -1;
        private Vector2 scrollPosition;

        struct Vertex
        {
            public float3 position;
            public half4 normal;
            public half4 tangent;
            public half2 uv0;


            public override string ToString()
            {

                return $"{normal} {tangent} {uv0}";
            }
        }


        Texture2D duplicateTexture(Texture2D source)
        {
            RenderTexture renderTex = RenderTexture.GetTemporary(
                        source.width,
                        source.height,
                        0,
                        RenderTextureFormat.Default,
                        RenderTextureReadWrite.Linear);

            Graphics.Blit(source, renderTex);
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = renderTex;
            Texture2D readableText = new Texture2D(source.width, source.height);
            readableText.name = source.name;
            readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
            readableText.Apply();
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(renderTex);
            return readableText;
        }

        public void SaveTextures()
        {

        }

        public void Export()
        {
           
            //AssetDatabase.Refresh();
            //Mesh mesh = instance.GetComponentInChildren<MeshFilter>().sharedMesh;
            //Object data = Object.Instantiate(mesh) as Object;

            //AssetDatabase.CreateAsset(data, "Assets/Test.asset");

            //foreach (var asset in assets)
            //{
            //    Debug.Log(asset);
            //}

            //var prefab = assetBundle.LoadAsset<GameObject>(item.name);
            //var instance = GameObject.Instantiate(prefab);
            //instance.name = prefab.name;
            //Mesh mesh = instance.GetComponentInChildren<MeshFilter>().sharedMesh;




            //GameObject gameObject = new GameObject("Test");
            //SkinnedMeshRenderer  skinnedMeshRenderer = gameObject.AddComponent<SkinnedMeshRenderer>();
            //skinnedMeshRenderer.sharedMesh = mesh;

            //using (var dataArray = MeshUtility.AcquireReadOnlyMeshData(mesh))
            //{
            //    //Mesh newMesh = new Mesh();
            //    var positionFormat = dataArray[0].GetVertexAttributeFormat(UnityEngine.Rendering.VertexAttribute.Position);
            //    Debug.Log(positionFormat);
            //    //var normalFormat = dataArray[0].GetVertexAttributeFormat(UnityEngine.Rendering.VertexAttribute.Normal);
            //    //Debug.Log(normalFormat);
            //    //var tangentFormat = dataArray[0].GetVertexAttributeFormat(UnityEngine.Rendering.VertexAttribute.Tangent);
            //    //Debug.Log(tangentFormat);
            //    //var uv0Format = dataArray[0].GetVertexAttributeFormat(UnityEngine.Rendering.VertexAttribute.TexCoord0);
            //    //Debug.Log(uv0Format);

            //    //int sPosition = dataArray[0].GetVertexAttributeStream(UnityEngine.Rendering.VertexAttribute.Position);
            //    //int sNormal = dataArray[0].GetVertexAttributeStream(UnityEngine.Rendering.VertexAttribute.Normal);
            //    //int sTangent = dataArray[0].GetVertexAttributeStream(UnityEngine.Rendering.VertexAttribute.Tangent);
            //    //int sTexCoord0 = dataArray[0].GetVertexAttributeStream(UnityEngine.Rendering.VertexAttribute.TexCoord0);

            //    //dataArray[0].vert
            //    var verts = dataArray[0].GetVertexData<Vertex>();
            //    //Debug.Log(verts.Length);

            //    //for (int i = 0; i < verts.Length; i++)
            //    //{
            //    //    Debug.Log(verts[i]);
            //    //}

            //    //Debug.Log(steam);
            //    //dataArray[0].GetVertexData<>

            //    //var gotVertices = new NativeArray<Vector3>(dataArray[0].vertexCount, Allocator.Temp);
            //    //dataArray[0].GetVertices(gotVertices);
            //    //gotVertices.Dispose();

            //    //Debug.Log(gotVertices.Length);
            //    //Mesh.ApplyAndDisposeWritableMeshData(dataArray, newMesh);
            //}


            //Mesh newMesh = new Mesh();
            //Mesh.ApplyAndDisposeWritableMeshData(array, newMesh, UnityEngine.Rendering.MeshUpdateFlags.Default);
            //Vector3[] v = newMesh.vertices;

            //instance.GetComponentInChildren<MeshFilter>().mesh = Instantiate(mesh);
            //Debug.Log(instance);
        }





        private void OnGUI()
        {
            if (style == null) style = new Style();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Export")) { Export(); }
            GUILayout.EndHorizontal();

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            Color tempBackgroundColor = GUI.backgroundColor;

            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                bool gray = i % 2 == 0;

                bool isSelect = selectIndex == i;
                GUI.backgroundColor = gray || isSelect ? Color.white : Color.black;
                if (GUILayout.Button(item.name, selectIndex == i ? style.select : style.normal))
                {
                    selectIndex = i;
                    GraberProvider.LoadObject(item.path);
                    AssetBundleInspector.OpenIfNeed();
                    //var data = assetBundle.LoadAsset<Object>(item.name);
                    //if (customEditor != null) Editor.DestroyImmediate(customEditor);
                    //if (data != null)
                    //{
                    //    Editor editor = Editor.CreateEditor(data);
                    //    customEditor = editor;
                    //}
                }

            }

            GUI.backgroundColor = tempBackgroundColor;

            EditorGUILayout.EndScrollView();
        }

        private Editor customEditor;
    }
}
