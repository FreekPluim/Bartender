using UnityEngine;

public class Furniture : MonoBehaviour, ISaveLoad
{
    public int ID;

    public void Load()
    {
        //Do Nothing
    }

    public void Save()
    {
        SaveManager.instance.saveData.worldObjects.Add(new(ID, transform.position, transform.rotation));
    }
}
