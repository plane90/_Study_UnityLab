using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class AssetBookmarkWindow : EditorWindow
{
    /* View */
    private ReorderableList _assetListView;
    
    /* Model */
    private List<UnityEngine.Object> _assets = new List<UnityEngine.Object>();

    private string _regKey;

    [MenuItem("Window/Asset Bookmark")]
    public static void ShowWindow()
    {
        var window = EditorWindow.GetWindow(typeof(AssetBookmarkWindow));
        window.minSize = new Vector2(100f, 100f);
        window.titleContent = EditorGUIUtility.TrTextContentWithIcon("Asset Bookmark", "UnityEditor.ConsoleWindow");
    }

    private void Awake()
    {
        _regKey = $"AssetBookmark_At_{Application.dataPath.Split('/')[1]}";
        foreach (var data in EditorPrefs.GetString(_regKey).Split('|'))
        {
            if (string.IsNullOrEmpty(data)) continue;
            var idx = data.Substring(0, data.IndexOf('/'));
            var guid = data.Substring(data.IndexOf('/') + 1);
            _assets.Add(AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(AssetDatabase.GUIDToAssetPath(guid)));
        }
    }

    private void OnEnable()
    {
        _assetListView = new ReorderableList(_assets, typeof(UnityEngine.Object), displayHeader: false, draggable: true, displayAddButton: true, displayRemoveButton: true)
        {
            drawElementCallback = (rect, index, active, focused) =>
            {
                _assets[index] = EditorGUI.ObjectField(rect, _assets[index], typeof(UnityEngine.Object), false);
                UpdatePref();
            }
        };
    }

    private void UpdatePref()
    {
        var prefVal = "";
        for(var i = 0; i < _assets.Count; i++)
        {
            prefVal += $"{i}/{AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(_assets[i])).ToString()}|";
        }
        EditorPrefs.SetString(_regKey, prefVal);
    }

    private void OnGUI()
    {
        _assetListView.DoLayoutList();
    }
}