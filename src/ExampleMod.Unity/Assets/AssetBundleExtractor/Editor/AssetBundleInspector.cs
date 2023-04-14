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
using Assets.AssetBundleExtractor.Editor;

namespace AssetBundleExtractor
{


    public class AssetBundleInspector : EditorWindow
    {
        public static AssetBundleInspector Singleton { get; private set; }

        public static void OpenIfNeed()
        {
            if (Singleton == null)
            {
                Open();
            }
            else FocusWindowIfItsOpen<AssetBundleInspector>();
        }

        public static void Open()
        {
            var window = CreateWindow<AssetBundleInspector>();
            window.titleContent = new GUIContent("Asset Bundle View");
        }

        private Object target;

        private void OnEnable()
        {
            Singleton = this;
            OnLoadObject();
            GraberProvider.OnLoadObject += OnLoadObject;            
        }

        private Editor editor;

        private void OnLoadObject()
        {
            target = GraberProvider.GetObject();
            if (editor != null) DestroyImmediate(editor);
            editor = Editor.CreateEditor(target);
        }

        private void OnDisable()
        {
            GraberProvider.OnLoadObject -= OnLoadObject;
            Singleton = null;
        }


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
 


        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            if(target is Texture2D)
            {
                if(GUILayout.Button("Save Texture"))
                {
                   GraberHelper.SaveTexture(target.name, target as Texture2D);
                }
            }

            if (target is GameObject)
            {
                if (GUILayout.Button("Instance"))
                {
                    GraberHelper.InstantiatePrefab(target as GameObject);                    
                }

                if (GUILayout.Button("Save All Textures"))
                {
                    GraberHelper.SaveAllTexturesAtPrefab(target as GameObject);
                }
            }
            
            if (target is Material)
            {
                //if (GUILayout.Button("Instance"))
                //{
                //    var instance = Instantiate(target as GameObject);
                //}
            }

            GUILayout.EndHorizontal();
            Rect rect = GUILayoutUtility.GetRect(position.width, position.height,GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            if (editor != null) editor.DrawPreview(rect);
        }
    }
}
