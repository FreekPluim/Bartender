using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ObjectDataSo))]
public class PrefabDataSoEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ObjectDataSo gen = (ObjectDataSo)target;

        if (GUILayout.Button("Set ID's"))
        {
            gen.SetIDs();
        }

        base.OnInspectorGUI();
    }
}
