using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    [SerializeField] Transform SpawnPoint;

    public GameObject CustomerPrefab;

    [SerializeField] List<GameObject> seats;
    Dictionary<GameObject, bool> takenSeats = new Dictionary<GameObject, bool>();

    private void Start()
    {
        for (int i = 0; i < seats.Count; i++)
        {
            takenSeats.Add(seats[i], false);
        }

        //TODO: Handle new day starting stuff.

        //GameStateManager.Instance.NewDay.AddListener();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            SpawnCustomer();
        }
    }

    void SpawnCustomer()
    {
        if (!checkOpenSeats())
        {
            Debug.LogError("No open seats");
            return;
        }

        GameObject customer = Instantiate(CustomerPrefab, SpawnPoint.transform.position, Quaternion.identity);
        Customer customerData = customer.GetComponent<Customer>();

        //Set Target Seat
        customerData.navAgent.SetDestination(getFirstOpenSeat());
    }

    public void OnCustomerLeft(GameObject customer)
    {
        takenSeats[customer] = false;
    }

    bool checkOpenSeats()
    {
        if (takenSeats.ContainsValue(false))
        {
            return true;
        }
        return false;
    }
    Vector3 getFirstOpenSeat()
    {
        foreach (GameObject seat in seats)
        {
            if (!takenSeats[seat])
            {
                takenSeats[seat] = true;
                return seat.transform.position;
            }
        }

        return Vector3.zero;
    }
}
