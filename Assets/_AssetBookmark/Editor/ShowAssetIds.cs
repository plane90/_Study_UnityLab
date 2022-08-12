using UnityEditor;
using UnityEngine;

class ShowAssetIds
{
    [MenuItem("Assets/Show Asset Ids")]
    static void MenuShowIds()
    {
        foreach (var asset in AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(Selection.activeObject)))
        {
            if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(asset, out string guid, out long fileId))
            {
                Debug.Log($"Asset: {asset.name}\nInstance ID:{asset.GetInstanceID()}\nGUID: {guid}\nFile ID: {fileId}");
            }
        }
    }
}