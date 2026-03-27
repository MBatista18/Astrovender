using UnityEditor;
using UnityEngine;

public class ObjectID : MonoBehaviour
{
    [SerializeField, HideInInspector] private string uniqueID;

    #if UNITY_EDITOR
    private void OnValidate()
    {
        if (Application.isPlaying) return;

        if (string.IsNullOrEmpty(uniqueID) || !IsUniqueID(uniqueID))
        {
            RefreshUniqueID();
        }
    }

    private static bool IsUniqueID(string id)
    {
        var allIdentifiers = FindObjectsByType<ObjectID>(FindObjectsSortMode.None);
        foreach (var @object in allIdentifiers)
        {
            if (@object != null && @object.GetID() == id)
            {
                return false;
            }
        }
        return true;
    }

    public void RefreshUniqueID()
    {
        uniqueID = System.Guid.NewGuid().ToString();
        UnityEditor.EditorUtility.SetDirty(this);

        if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
        {
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
        }
    }
    #endif

    public string GetID() => uniqueID;
}

#if UNITY_EDITOR

[CustomEditor(typeof(ObjectID), true)]
public class IDEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var saveableID = (ObjectID)target;

        EditorGUILayout.Space(8);
        EditorGUILayout.LabelField("SaveSystem UniqueID", EditorStyles.boldLabel);

        using (new EditorGUI.DisabledScope(true))
        {
            EditorGUILayout.TextField("Unique ID", saveableID.GetID());
        }

        EditorGUILayout.Space(4);

        if (GUILayout.Button("Refresh Unique ID"))
        {
            Undo.RecordObject(saveableID, "Refresh Unique ID");
            saveableID.RefreshUniqueID();
            EditorUtility.SetDirty(saveableID);
        }

        EditorGUILayout.HelpBox("The Unique ID is used by the Save System to identify this object uniquely in the save data. Refreshing it will generate a new ID, which will break existing save files that reference this NPC. Use with caution. Delete existing save files after use.", MessageType.Warning);
    }
}
#endif
