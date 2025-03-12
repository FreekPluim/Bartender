using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    public SaveData saveData;
    public SaveData loadData;

    public ObjectDataSo ObjectDataSo;

    private FileDataHandler fileDataHandler;
    [SerializeField] string fileName = "NewSaveData.salt";

    public GameObject test;

    private void Start()
    {
        fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(this);
        Load();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Save();
        }
    }

    public void Save()
    {
        //Save "SaveData" To JSON
        //clear save data
        saveData = new SaveData();
        saveData.worldObjects = new();
        //get all data added to save

        var allObj = FindObjectsOfType<MonoBehaviour>().OfType<ISaveLoad>();
        foreach (var item in allObj)
        {
            item.Save();
        }

        //Save data;
        fileDataHandler.Save(saveData);
    }

    public void Load()
    {
        this.loadData = fileDataHandler.Load();

        //Load SaveData From JSON and apply to "LoadData"
        var allObj = FindObjectsOfType<MonoBehaviour>().OfType<ISaveLoad>();
        foreach (var item in allObj)
        {
            item.Load();
        }

        ObjectDataSo.SetIDs();
        foreach (WorldObjects obj in loadData.worldObjects)
        {
            if (obj.ID < ObjectDataSo.prefabs.Count)
            {
                Instantiate(ObjectDataSo.prefabs[obj.ID], obj.location, obj.rotation);
            }
        }

        Debug.Log("Finished loading");
    }
}


//Data to be turned into JSON
[Serializable]
public class SaveData
{
    //Player Data
    public int money;
    public List<InventoryItem> inventory;

    //World save settings
    public List<WorldObjects> worldObjects;
    public Vector2Int buildingSizeX;
    public Vector2Int buildingSizeZ;
    public int doorLocationX;

    //Customer Data

}

//Classes to save data propperly.
[Serializable]
public class WorldObjects
{
    public int ID;
    public Vector3 location;
    public Quaternion rotation;

    public WorldObjects(int pID, Vector3 pLocation, Quaternion pRotation)
    {
        ID = pID;
        location = pLocation;
        rotation = pRotation;
    }
}
public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }
    public SaveData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        SaveData loadedData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                //Serialize
                string dataToLoad = "";

                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadedData = JsonUtility.FromJson<SaveData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error while loading data from: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }
    public void Save(SaveData data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            //Serialize
            string dataToStore = JsonUtility.ToJson(data, true);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                    Debug.Log("Saved data to: " + fullPath);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error while saving data to: " + fullPath + "\n" + e);
        }
    }
}