using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Map_Generator : MonoBehaviour, ISaveLoad
{
    public static Map_Generator instance;

    [Header("Floor")]
    [SerializeField] GameObject floorTile;
    [SerializeField] int tileSize;
    public Vector2Int floorSizeX;
    public Vector2Int floorSizeZ;
    [SerializeField] Dictionary<Vector2Int, GameObject> floorTiles = new();

    [Header("Door")]
    public GameObject doorPrefab;
    public int doorX = 0;

    [Header("Walls")]
    [SerializeField] GameObject wallTile;
    [SerializeField] int wallHeight;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(this);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)) Expand(new(-1, 0));
        if (Input.GetKeyDown(KeyCode.X)) Expand(new(1, 0));
        if (Input.GetKeyDown(KeyCode.V)) Expand(new(0, 1));
    }

    public void CreateBuilding()
    {
        GameObject building = new GameObject("Building");
        floorTiles.Clear();

        for (int x = floorSizeX.x; x <= floorSizeX.y; x++)
        {
            for (int z = floorSizeZ.x; z <= floorSizeZ.y; z++)
            {
                GameObject floor = Instantiate(floorTile, new Vector3(x, 0, z), Quaternion.identity, building.transform);
                floor.name = $"{x} - {z}";
                floorTiles.Add(new(x, z), floor);
            }
        }

        GenerateWallPerTile();
        PlaceDoor(doorX, 2);
    }
    void GenerateWallPerTile()
    {
        foreach (var floorTile in floorTiles.Values)
        {
            //If already has walls, dont place more
            if (floorTile.transform.childCount == 0)
            {

                for (int y = 0; y < wallHeight; y++)
                {
                    Vector3 pos = floorTile.transform.position;

                    //Else, place walls
                    if (pos.x == floorSizeX.x)
                    {
                        GameObject wallx0 = Instantiate(wallTile, new Vector3(pos.x, y, pos.z), Quaternion.Euler(new(0, 180, 0)), floorTile.transform);
                    }
                    if (pos.x == floorSizeX.y)
                    {
                        GameObject wallxEnd = Instantiate(wallTile, new Vector3(pos.x, y, pos.z), Quaternion.Euler(new(0, 0, 0)), floorTile.transform);
                    }
                    if (pos.z == floorSizeZ.x)
                    {
                        GameObject wallz0 = Instantiate(wallTile, new Vector3(pos.x, y, pos.z), Quaternion.Euler(new(0, 90, 0)), floorTile.transform);
                    }
                    if (pos.z == floorSizeZ.y)
                    {
                        GameObject wallzEnd = Instantiate(wallTile, new Vector3(pos.x, y, pos.z), Quaternion.Euler(new(0, -90, 0)), floorTile.transform);
                    }
                }
            }
        }
    }
    void PlaceDoor(int doorLocationX, int doorHeight)
    {
        //Get door location('s)
        Vector2Int doorLocation = new(doorLocationX, floorSizeZ.x);

        //Remove walls
        for (int i = doorHeight; i > 0; i--)
        {
            Destroy(floorTiles[doorLocation].transform.GetChild(i - 1).gameObject);
        }

        //Place door prefab('s)
        if (doorPrefab != null) Instantiate(doorPrefab, floorTiles[doorLocation].transform);

    }

    //Expand building
    public void Expand(Vector2Int direction)
    {
        Debug.Log("Expanding");

        //Figure out direction
        if (direction.x != 0) expandXDir(direction.x);
        else expandZDir(direction.y);

        //Generate walls
        GenerateWallPerTile();
    }

    void expandXDir(int dir)
    {
        IEnumerable<KeyValuePair<Vector2Int, GameObject>> tileRow;

        if (dir == -1) tileRow = floorTiles.Where(x => x.Key.x == floorSizeX.x);
        else tileRow = floorTiles.Where(x => x.Key.x == floorSizeX.y);

        List<GameObject> newTiles = new List<GameObject>();
        //Remove Walls
        foreach (var tile in tileRow)
        {
            for (int j = tile.Value.transform.childCount; j > 0; j--)
            {
                Destroy(tile.Value.transform.GetChild(j - 1).gameObject);
            }

            //create new floor tile in correct dir
            if (dir == -1) newTiles.Add(Instantiate(floorTile, new Vector3(floorSizeX.x + dir, 0, tile.Key.y), Quaternion.identity));
            else newTiles.Add(Instantiate(floorTile, new Vector3(floorSizeX.y + dir, 0, tile.Key.y), Quaternion.identity));
        }

        foreach (var tile in newTiles)
        {
            floorTiles.Add(new((int)tile.transform.position.x, (int)tile.transform.position.z), tile);
        }

        if (dir == -1) floorSizeX = new(floorSizeX.x + dir, floorSizeX.y);
        else floorSizeX = new(floorSizeX.x, floorSizeX.y + dir);
    }
    void expandZDir(int dir)
    {
        IEnumerable<KeyValuePair<Vector2Int, GameObject>> tileRow;
        if (dir == -1) return;
        else tileRow = floorTiles.Where(x => x.Key.y == floorSizeZ.y);

        List<GameObject> newTiles = new List<GameObject>();

        //Remove Walls
        foreach (var tile in tileRow)
        {
            for (int j = tile.Value.transform.childCount; j > 0; j--)
            {
                Destroy(tile.Value.transform.GetChild(j - 1).gameObject);
            }

            //create new floor tile in correct dir
            newTiles.Add(Instantiate(floorTile, new Vector3(tile.Key.x, 0, floorSizeZ.y + dir), Quaternion.identity));
        }
        foreach (var tile in newTiles)
        {
            floorTiles.Add(new((int)tile.transform.position.x, (int)tile.transform.position.z), tile);
        }

        floorSizeZ = new(floorSizeZ.x, floorSizeZ.y + dir);
    }

    //Save Load
    public void Save()
    {
        SaveManager.instance.saveData.buildingSizeX = floorSizeX;
        SaveManager.instance.saveData.buildingSizeZ = floorSizeZ;
        SaveManager.instance.saveData.doorLocationX = doorX;
    }
    public void Load()
    {
        doorX = SaveManager.instance.saveData.doorLocationX;
        if (SaveManager.instance.saveData.buildingSizeX != Vector2Int.zero) floorSizeX = SaveManager.instance.saveData.buildingSizeX;
        if (SaveManager.instance.saveData.buildingSizeZ != Vector2Int.zero) floorSizeZ = SaveManager.instance.saveData.buildingSizeZ;

        Debug.Log("Generating building");
        CreateBuilding();

    }
}
