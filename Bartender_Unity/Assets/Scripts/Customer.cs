using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour, IInteractable
{
    [SerializeField] List<DrinkSo> allDrinks;

    string Name = "Dopinder";
    //Age
    int Age;

    //Order
    DrinkSo order;

    public NavMeshAgent navAgent;



    public bool gaveOrder = false;
    public bool hasID = true;

    bool reached = false;

    public void OnInteracted(PlayerInteraction playerInteraction)
    {
        //Open dialog box
        if (playerInteraction.DrinkInHand == null)
        {
            if (UIManager.instance != null)
            {
                UIManager.instance.onInteractedWithCustomer.Invoke();
                UIManager.instance.onAskedForID.AddListener(GiveID);
                UIManager.instance.onReturnID.AddListener(ReturnID);
                UIManager.instance.onAskedForOrder.AddListener(GiveOrder);
                UIManager.instance.onNevermind.AddListener(OnNevermind);
                UIManager.instance.InteractedWithCustomer(this);
            }
            else Debug.LogError("**NO UIMANAGER FOUND**");
        }
        else
        {
            //hand customer drink
            if (playerInteraction.DrinkInHand == order)
            {
                playerInteraction.RemoveItemFromHand(true);
                GameStateManager.Instance.GetCustomerManager().OnCustomerLeft(gameObject);

                Debug.Log("Correct Drink: Leaving");
            }
        }
    }
    void GiveID(UIManager uiManager)
    {
        uiManager.SetID(Name, Age);
        hasID = false;
    }
    void ReturnID(UIManager uiManager)
    {
        hasID = true;
    }
    void GiveOrder(UIManager uiManager)
    {
        uiManager.AddOrder(order);
    }
    void OnNevermind(UIManager uiManager)
    {
        uiManager.onAskedForID.RemoveListener(GiveID);
        uiManager.onAskedForOrder.RemoveListener(GiveOrder);
        uiManager.onNevermind.RemoveListener(OnNevermind);
    }

    private void Start()
    {
        order = allDrinks[Random.Range(0, allDrinks.Count)];
        Age = Random.Range(15, 80);
    }

    void Update()
    {
        if (!reached && CheckIfDestinationReached())
        {
            OnDestinationReached();
        }
    }

    void OnDestinationReached()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(Vector3.zero), 1);
    }

    bool CheckIfDestinationReached()
    {
        // Check if we've reached the destination
        if (!navAgent.pathPending)
        {
            if (navAgent.remainingDistance <= navAgent.stoppingDistance)
            {
                if (!navAgent.hasPath || navAgent.velocity.sqrMagnitude == 0f)
                {
                    // Done
                    return true;

                }
            }
        }

        reached = false;
        return false;
    }
}
