using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Linq;
using static UnityEditor.Progress;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

namespace AssetBundleExtractor
{



    public class AssetBundleListView : EditorWindow
    {


        private static string search
        {
            get { return EditorPrefs.GetString("ABE_LIST_SEARCH"); }
            set { EditorPrefs.SetString("ABE_LIST_SEARCH", value); }
        }

        [MenuItem("Tools/Asset Bundle Extractor/List")]
        public static void Open()
        {
            var window = CreateWindow<AssetBundleListView>();
            window.titleContent = new GUIContent("Asset Bundle List");
            if (string.IsNullOrEmpty(GraberProvider.pathFolder))
            {
                GraberProvider.LoadFolder(EditorUtility.OpenFolderPanel("Select Folder", GraberProvider.pathFolder, ""));
            }
        }

        private List<Item> items = new List<Item>();

        private void CreateTree(ref List<Item> items, string path)
        {
            if (string.IsNullOrEmpty(path)) return;

            if (items == null) items.Clear();

            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            var files = directoryInfo.GetFiles();

            items = files.Select(x => new Item() { name = x.Name, path = x.FullName }).OrderBy(x => x.name).ToList();
        }

        private class Item
        {
            public string path;
            public string name;
        }

        private Vector2 scrollPosition;

        private int selectIndex;

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

        private bool ContainsFilter(string name, string filter)
        {
            return string.IsNullOrEmpty(filter) || name.ToLower().Contains(filter.ToLower());
        }

        private void LoadSelected()
        {
            var item = items[selectIndex];
            GraberProvider.LoadBundle(item.path);
            AssetBundleView.OpenIfNeed(); 
            //Debug.Log(item.path);

            //AssetBundle.UnloadAllAssetBundles(true);

            ////string path = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Captain of Industry\\AssetBundles\\zippers_86df";
            //var bundle = AssetBundle.LoadFromFile(item.path);
            //Debug.Log(bundle);
            //AssetBundleView.Inspect(bundle);
            ////AssetBundleView.Load("C://Program Files (x86)/Steam/steamapps/common/Captain of Industry/AssetBundles/zippers_86df");
        }

        private void OnGUI()
        {
            if (style == null) style = new Style();

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Folder")) { }
            string newSearch = EditorGUILayout.TextField(search);
            search = newSearch;
            if (GUILayout.Button("Load")) { LoadSelected(); }

            GUILayout.EndHorizontal();

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            Color tempBackgroundColor = GUI.backgroundColor;

            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                bool gray = i % 2 == 0;
                if (ContainsFilter(item.name, newSearch))
                {
                    bool isSelect = selectIndex == i;
                    GUI.backgroundColor = gray || isSelect ? Color.white : Color.black;
                    if (GUILayout.Button(item.name, selectIndex == i ? style.select : style.normal))
                    {
                        selectIndex = i;
                    }
                }
            }

            GUI.backgroundColor = tempBackgroundColor;

            EditorGUILayout.EndScrollView();
        }

        private void OnEnable()
        {
            CreateTree(ref items, GraberProvider.pathFolder); 
        }

        private void OnDisable()
        {

        }
    }
}
