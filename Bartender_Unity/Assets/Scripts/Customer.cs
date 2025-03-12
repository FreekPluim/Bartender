using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Customer : MonoBehaviour, IInteractable
{
    [SerializeField] List<DrinkSo> allDrinks;

    [SerializeField]
    List<string> possibleOpeningSentences = new List<string>();

    string Name = "Dopinder";
    //Age
    int Age;

    //Order
    DrinkSo order;

    public NavMeshAgent navAgent;

    public bool gaveOrder = false;
    public bool hasID = true;
    public bool shownID = false;

    bool reached = false;
    bool canInteract = false;

    /// <summary>
    /// Initial Interact
    /// </summary>
    public virtual void OnInteracted(PlayerInteraction playerInteraction)
    {
        if (!canInteract)
        {
            return;
        }
        if (playerInteraction.DrinkInHand == order)
        {
            playerInteraction.RemoveItemFromHand(true);
            DialogueManager.Instance.AddToDialogueQueue(new DialogueData(
            "Thank you", CustomerLeaves));
        }
        //Open dialog box
        if (hasID && shownID)
        {
            DialogueManager.Instance.AddToDialogueQueue(new DialogueData(
            "Yes?", null,
            new Dictionary<string, UnityAction>()
            {
                {"Ask for ID", GiveorReturnID },
                {"Ask for Order", GiveOrder },
                {"Something wrong with ID", SomethingWrongWithID },
                {"Nevermind", OnNevermind }
            }));
        }
        else if (hasID)
        {
            DialogueManager.Instance.AddToDialogueQueue(new DialogueData(
                possibleOpeningSentences[Random.Range(0, possibleOpeningSentences.Count)], null,
                new Dictionary<string, UnityAction>()
                {
                    {"Ask for ID", GiveorReturnID },
                    {"Ask for Order", GiveOrder },
                    {"Nevermind", OnNevermind }
                }));
        }
        else
        {
            DialogueManager.Instance.AddToDialogueQueue(new DialogueData(
            "Yes?", null,
            new Dictionary<string, UnityAction>()
            {
                {"Return ID", GiveorReturnID },
                {"Ask for Order", GiveOrder },
                {"Something wrong with ID", SomethingWrongWithID },
                {"Nevermind", OnNevermind }
            }));
        }
    }

    protected virtual void GiveorReturnID()
    {
        GameStateManager.Instance.GetUIManager().SetID(Name, Age);
        hasID = !hasID;
        shownID = true;
        DialogueManager.Instance.Dequeue();
    }
    protected virtual void GiveOrder()
    {
        DialogueManager.Instance.AddToDialogueQueue(new DialogueData("I would like a " + order.name));

        if (!gaveOrder)
        {
            GameStateManager.Instance.GetUIManager().AddOrder(order, gameObject);
            gaveOrder = true;
        }

        DialogueManager.Instance.Dequeue();
    }
    protected virtual void OnNevermind()
    {
        DialogueManager.Instance.Dequeue();
    }
    protected virtual void SomethingWrongWithID()
    {
        DialogueManager.Instance.AddToDialogueQueue(new DialogueData(
                "What is wrong with my ID?",
                null,
                new Dictionary<string, UnityAction>()
                {
                    {"Not Old Enough", NotOldEnough },
                    {"Image doesn't match", ImageDoesntMatch },
                    {"It's fake", FakeID },
                    {"Nevermind", OnNevermind }
                }));

        DialogueManager.Instance.Dequeue();
    }

    /// <summary>
    /// Something wrong with ID
    /// </summary>
    protected void NotOldEnough()
    {
        if (!gaveOrder)
        {
            DialogueManager.Instance.AddToDialogueQueue(new DialogueData(
                "But you don't know what i was going to order yet?"));
        }
        else
        {
            DialogueManager.Instance.AddToDialogueQueue(new DialogueData(
                "Fine, i'll leave..", CustomerLeaves));
        }

        DialogueManager.Instance.Dequeue();
    }
    protected void ImageDoesntMatch()
    {
        DialogueManager.Instance.AddToDialogueQueue(new DialogueData(
                "Fine, i'll leave..", CustomerLeaves));
        DialogueManager.Instance.Dequeue();
    }
    protected void FakeID()
    {
        DialogueManager.Instance.AddToDialogueQueue(new DialogueData(
                "Fine, i'll leave", CustomerLeaves));

        DialogueManager.Instance.Dequeue();
    }

    /// <summary>
    /// Leaving Customer
    /// </summary>
    protected void CustomerLeaves()
    {
        canInteract = false;
        UIManager.instance.RemoveOrder(gameObject);
        GameStateManager.Instance.GetCustomerManager().OnCustomerLeft(gameObject);
        navAgent.SetDestination(GameStateManager.Instance.GetCustomerManager().SpawnPoint.position);
        if (!hasID) GiveorReturnID();
    }

    protected virtual void Start()
    {
        order = allDrinks[Random.Range(0, allDrinks.Count)];
        Age = Random.Range(12, 80);
    }
    protected virtual void Update()
    {
        if (!reached && CheckIfDestinationReached())
        {
            OnDestinationReached();
        }
    }
    protected void OnDestinationReached()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(Vector3.zero), 1);
        canInteract = true;
    }
    protected bool CheckIfDestinationReached()
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
