using UnityEngine;

public class CheckOverlap : MonoBehaviour
{
    Collider col;

    public bool overlapping = false;

    private void Start()
    {
        col = GetComponent<Collider>();

        if (col == null)
        {
            col = GetComponentInChildren<Collider>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        overlapping = true;
        Debug.Log(other.name);
    }
    private void OnTriggerExit(Collider other)
    {
        overlapping = false;
    }
}
