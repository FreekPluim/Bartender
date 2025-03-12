using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PrefabData", menuName = "Prefab Data")]
public class ObjectDataSo : ScriptableObject
{
    public List<GameObject> prefabs;

    public void SetIDs()
    {
        if (prefabs != null && prefabs.Count > 0)
        {
            for (int i = 0; i < prefabs.Count; i++)
            {
                if (prefabs[i].TryGetComponent(out Furniture furniture))
                {
                    furniture.ID = i;
                }
            }
        }
    }
}
