using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class AssetBookmarkWindow : EditorWindow
{
    public class AssetInfo
    {
        public UnityEngine.Object asset;
        public bool isExpanded;
    }
    
    /* View */
    private ReorderableList _assetListView;
    
    /* Model */
    private List<AssetInfo> _assetInfos = new List<AssetInfo>();
    private string _regKey;
    
    private Vector2 _scrollPos;
    private Rect _rectDragAndDropArea;
    private const float kFoldoutWidth = 20f;
    private const float kRemoveBtnWidth = 30f;
    private const float kSpacing = 2f;

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
            var ixExpanded = data.Substring(0, data.IndexOf('/'));
            var guid = data.Substring(data.IndexOf('/') + 1);
            _assetInfos.Add(new AssetInfo
            {
                asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(AssetDatabase.GUIDToAssetPath(guid)),
                isExpanded = (ixExpanded == "1")
            });
        }
    }

    private void OnEnable()
    {
        _assetListView = new ReorderableList(_assetInfos, typeof(UnityEngine.Object), displayHeader: false, draggable: true, displayAddButton: true, displayRemoveButton: true)
        {
            drawElementCallback = (rect, index, active, focused) =>
            {
                var rectFoldout = new Rect(rect.x, rect.y, kFoldoutWidth, EditorGUIUtility.singleLineHeight);
                
                var rectObjField = rectFoldout;
                rectObjField.x = rectFoldout.xMax;
                rectObjField.xMax = rect.xMax - kRemoveBtnWidth - kSpacing;

                var rectRemoveBtn = rectObjField;
                rectRemoveBtn.x = rectObjField.xMax + kSpacing;
                rectRemoveBtn.xMax = rect.xMax;
                
                var rectPath = rectFoldout;
                rectPath.y += EditorGUIUtility.standardVerticalSpacing + EditorGUIUtility.singleLineHeight;
                rectPath.width = rect.width;
                
                // Foldout: 경로 보이기/숨기기
                _assetInfos[index].isExpanded = EditorGUI.Foldout(rectFoldout, _assetInfos[index].isExpanded, new GUIContent());
                if (_assetInfos[index].isExpanded)
                {
                    EditorGUI.LabelField(rectPath, AssetDatabase.GetAssetPath(_assetInfos[index].asset));
                }
                
                // ObjectField: Asset 프로퍼티
                _assetInfos[index].asset = EditorGUI.ObjectField(rectObjField, _assetInfos[index].asset, typeof(UnityEngine.Object), false);
                
                // Button: 원소 제거
                if (GUI.Button(rectRemoveBtn, new GUIContent("-", "Remove asset from list"), new GUIStyle(GUI.skin.button)))
                {
                    _assetInfos.RemoveAt(index);
                }
                
                UpdatePref();
            },
            drawNoneElementCallback = rect => EditorGUI.LabelField(rect, "즐겨 찾는 에셋을 등록하세요."),
            elementHeightCallback = index => _assetInfos[index].isExpanded ?
                EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing :
                EditorGUIUtility.singleLineHeight,
        };
    }

    private void UpdatePref()
    {
        var prefVal = "";
        foreach (var assetInfo in _assetInfos)
        {
            prefVal += $"{(assetInfo.isExpanded ? "1" : "0")}/{AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(assetInfo.asset)).ToString()}|";
        }
        EditorPrefs.SetString(_regKey, prefVal);
    }

    private void OnGUI()
    {
        // 스크롤바
        _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
        // 헤더 레이아웃
        EditorGUILayout.BeginHorizontal();

        // Box: Drag&Drop Area
        _rectDragAndDropArea = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none,
            GUILayout.Height(EditorGUIUtility.singleLineHeight));
        GUI.Box(_rectDragAndDropArea, "이 곳에 드랍하여 Asset을 추가하세요.");
        DraggingAndDropping(_rectDragAndDropArea);
        
        // Button: Clear
        if (GUILayout.Button("Clear", GUILayout.Width(45f)))
        {
            _assetInfos.Clear();
            UpdatePref();
        }
        EditorGUILayout.EndHorizontal();
        
        // 리오더러블리스트
        _assetListView.DoLayoutList();
        
        EditorGUILayout.EndScrollView();
    }
    
    private void DraggingAndDropping (Rect dropArea)
    {
        var currentEvent = Event.current;
        
        if (!dropArea.Contains (currentEvent.mousePosition))
            return;
        switch (currentEvent.type)
        {
            case EventType.DragUpdated:
                DragAndDrop.visualMode = IsDragValid() ? DragAndDropVisualMode.Link : DragAndDropVisualMode.Rejected;
                currentEvent.Use ();
                break;
            case EventType.DragPerform:
                DragAndDrop.AcceptDrag();
                foreach (var objectReference in DragAndDrop.objectReferences)
                {
                    _assetInfos.Add(new AssetInfo { asset = objectReference});
                }
                currentEvent.Use();
                break;
        }
    }
    
    private static bool IsDragValid () => !DragAndDrop.objectReferences.OfType<GameObject>().Any();
}