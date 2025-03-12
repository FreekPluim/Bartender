using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Map_Generator))]
public class BuildingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Map_Generator gen = (Map_Generator)target;
        base.OnInspectorGUI();

        if (GUILayout.Button("Generate Building"))
        {
            gen.CreateBuilding();
        }
    }
}
