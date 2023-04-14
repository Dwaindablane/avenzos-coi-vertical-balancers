using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Assets.AssetBundleExtractor.Editor
{
    public class GraberHelper
    {

        public static GameObject InstantiatePrefab(GameObject gameObject)
        {
            var instance = GameObject.Instantiate(gameObject);
            instance.name = gameObject.name;
            return instance;
        }

        public static void SaveAllTexturesAtPrefab(GameObject gameObject)
        {
            HashSet<Texture2D> textures = new HashSet<Texture2D>();

            var renderes = gameObject.GetComponentsInChildren<MeshRenderer>();
            foreach (var r in renderes)
            {
                foreach (var material in r.sharedMaterials)
                {
                    var albedo = material.GetTexture("_MainTex");
                    var normal = material.GetTexture("_BumpMap");
                    var metallic = material.GetTexture("_MetallicGlossMap");

                    textures.Add(albedo as Texture2D);
                    textures.Add(normal as Texture2D);
                    textures.Add(metallic as Texture2D);

                    Debug.Log(albedo);
                    Debug.Log(normal);
                    Debug.Log(metallic);
                }
            }

            foreach (var text in textures)
            {
                SaveTexture(text.name, text);
            }
        }



        public static Texture2D CloneTexture(Texture2D source)
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

        public static void SaveTexture(string name, Texture2D texture)
        {

            var newTexture = CloneTexture(texture);
            var bytes = newTexture.EncodeToPNG();
            File.WriteAllBytes($"Assets/{name}.png", bytes);
            AssetDatabase.Refresh();
        }



        //var item = items[selectIndex];

        //var prefab = assetBundle.LoadAsset<GameObject>(item.name);

        //var instance = GameObject.Instantiate(prefab);

        
    }
}