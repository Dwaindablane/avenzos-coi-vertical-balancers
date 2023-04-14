
using System.IO;
using UnityEditor;
using UnityEngine;

namespace AssetBundleExtractor
{
    [InitializeOnLoad]
    public class GraberProvider
    {
         static GraberProvider()
        {
            AssetBundle.UnloadAllAssetBundles(true);
        }

        public static string pathFolder
        { 
            get { return EditorPrefs.GetString("ABE_LIST_PATH_FOLDER"); }
            set { EditorPrefs.SetString("ABE_LIST_PATH_FOLDER", value); }
        }

        private static string pathBundle
        {
            get { return EditorPrefs.GetString("ABE_LIST_PATH_BUNDLE"); }
            set { EditorPrefs.SetString("ABE_LIST_PATH_BUNDLE", value); }
        }

        private static string pathObject
        {
            get { return EditorPrefs.GetString("ABE_LIST_PATH_OBJECT"); }
            set { EditorPrefs.SetString("ABE_LIST_PATH_OBJECT", value); }
        }



        public static void LoadFolder(string path)
        {
            pathFolder = path;
            OnLoadFolder?.Invoke();
        }


        private static AssetBundle loadedAssetBundle;
        private static Object loadedObject;

        public static AssetBundle GetAssetBundle()
        {
            if (loadedAssetBundle == null)
            {
                return loadedAssetBundle = AssetBundle.LoadFromFile(pathBundle);
            }

            return loadedAssetBundle;
        }

        public static Object GetObject()
        {
            var assetBundle = GetAssetBundle();
            if (assetBundle == null) return null;
            if (loadedObject == null)
            {
                return loadedObject = assetBundle.LoadAsset<Object>(pathObject);
            }
            return loadedObject;
        }

        public delegate void LoadObjectDelegate();
        public delegate void LoadFolderDelegate();
        public delegate void LoadAssetBundleDelegate();


        public static event LoadObjectDelegate OnLoadObject;
        public static event LoadFolderDelegate OnLoadFolder;
        public static event LoadAssetBundleDelegate OnLoadAssetBundle;

        public static void LoadBundle(string path)
        {
            AssetBundle.UnloadAllAssetBundles(true);
            if(path != pathBundle)
            {
                LoadObject("");
            }

            loadedAssetBundle = AssetBundle.LoadFromFile(path);
            OnLoadAssetBundle?.Invoke();
            pathBundle = path;
        }

        public static void LoadObject(string path)
        {
            if (string.IsNullOrEmpty(path)) return;
            if (loadedAssetBundle != null)
            {
                loadedObject = loadedAssetBundle.LoadAsset<Object>(path);
                OnLoadObject?.Invoke();
            }
            pathObject = path;
        }

    }
}