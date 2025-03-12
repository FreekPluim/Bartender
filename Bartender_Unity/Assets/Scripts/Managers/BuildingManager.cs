using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    Camera cam;
    [SerializeField] LayerMask layerMask;

    [SerializeField] GameObject BuildingUI;
    bool Building = false;

    int selectedItem = 0;
    [SerializeField] List<InventoryItem> inventory;

    GameObject PreviewPrefabParent;
    GameObject PreviewPrefab;

    private void Start()
    {
        cam = Camera.main;
        PreviewPrefabParent = new GameObject();
        PreviewPrefabParent.SetActive(false);
        PreviewPrefabParent.layer = 2;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            EnterBuildingMode();
        }

        if (Building && inventory.Count > 0)
        {
            //Place Tile
            if (Input.GetMouseButtonDown(0))
            {
                //Place tile
                PlaceTile();
            }

            //Scroll between items in inventory
            if (Input.mouseScrollDelta.y > 0)
            {
                if (selectedItem == inventory.Count - 1) selectedItem = 0;
                else selectedItem++;
                Preview();

            }
            if (Input.mouseScrollDelta.y < 0)
            {
                if (selectedItem == 0) selectedItem = inventory.Count - 1;
                else selectedItem--;
                Preview();
            }

            //Place preview in specific place
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 20, layerMask))
            {
                PreviewPrefabParent.transform.position =
                    new Vector3(
                    (float)Math.Round(hit.point.x * 2) / 2,
                    inventory[selectedItem].floorLevel + 0.01f,
                    (float)Math.Round(hit.point.z * 2) / 2
                );
            }

            //Rotate preview
            if (Input.GetKeyDown(KeyCode.E))
            {
                PreviewPrefabParent.transform.Rotate(new(0, 90, 0));
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                PreviewPrefabParent.transform.Rotate(new(0, -90, 0));
            }

        }
    }

    public void EnterBuildingMode()
    {
        Building = !Building;
        BuildingUI.SetActive(!BuildingUI.activeSelf);
        PreviewPrefabParent.SetActive(!PreviewPrefabParent.activeSelf);

        Preview();
    }

    void PlaceTile()
    {
        //Check for overlap
        if (PreviewPrefab.TryGetComponent<CheckOverlap>(out CheckOverlap checkOverlap))
        {
            if (checkOverlap.overlapping)
            {
                Debug.LogError("Object is overlapping and cant be placed");
                return;
            }
        }

        GameObject newObject = Instantiate(
            SaveManager.instance.ObjectDataSo.prefabs[inventory[selectedItem].ID],
            PreviewPrefabParent.transform.position,
            PreviewPrefabParent.transform.rotation);

        if (inventory[selectedItem].amount > 1)
        {
            inventory[selectedItem].amount--;
        }
        else if (inventory[selectedItem].amount == 1)
        {
            inventory.Remove(inventory[selectedItem]);

            if (inventory.Count > 0)
            {
                selectedItem = 0;
                Preview();
            }
        }
    }

    Collider col;
    void Preview()
    {
        if (PreviewPrefabParent.transform.childCount > 0) Destroy(PreviewPrefabParent.transform.GetChild(0).gameObject);
        PreviewPrefab = Instantiate(
            SaveManager.instance.ObjectDataSo.prefabs[inventory[selectedItem].ID],
            PreviewPrefabParent.transform);

        if (PreviewPrefab.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
        else
        {
            Rigidbody newRb = PreviewPrefab.AddComponent<Rigidbody>();
            newRb.isKinematic = true;
            newRb.useGravity = false;
        }

        col = PreviewPrefab.GetComponent<Collider>();
        if (col == null) col = PreviewPrefab.GetComponentInChildren<Collider>();

        if (col is MeshCollider) SetConvex(col as MeshCollider);
        col.isTrigger = true;
        col.gameObject.layer = 2;
    }
    void SetConvex(MeshCollider col)
    {
        col.convex = true;
    }
}

[Serializable]
public class InventoryItem
{
    public int ID;
    public int amount;
    public int floorLevel;
}
