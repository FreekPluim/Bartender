using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour, ISaveLoad
{
    public static PlayerData instance;

    [SerializeField] int money;
    [SerializeField] List<InventoryItem> inventory;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    public void Save()
    {
        SaveManager.instance.saveData.money = money;
        SaveManager.instance.saveData.inventory = inventory;
    }

    public void Load()
    {
        money = SaveManager.instance.saveData.money;
        inventory = SaveManager.instance.saveData.inventory;
    }
}
