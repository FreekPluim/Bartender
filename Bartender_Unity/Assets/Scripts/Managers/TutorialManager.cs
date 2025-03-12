using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class TutorialManager : Customer
{
    NavMeshAgent tutorialMan;

    int step = 0;
    protected override void Start()
    {
        base.Start();
        navAgent.SetDestination(GameStateManager.Instance.GetCustomerManager().getFirstOpenSeat());
    }
    protected override void Update()
    {
        base.Update();
        switch (step)
        {
            case 0:
                if (CheckIfDestinationReached())
                {
                    GameStateManager.Instance.GetDialogueManager().AddToDialogueQueue(
                        new DialogueData(
                            "Hey, you must be the new bartender! The name is ..., i used to work her before you!"));
                    GameStateManager.Instance.GetDialogueManager().AddToDialogueQueue(
                        new DialogueData("Im here to help you out on your first day if you need it!", null,
                            new Dictionary<string, UnityAction>()
                            {
                                {"Yes please", yes },
                                {"No, thank you (skip tutorial)", no }
                            }));
                    step++;
                }
                break;
            case 15:
                if (CheckIfDestinationReached())
                {
                    Destroy(gameObject);
                }
                break;
            default:
                break;
        }
    }

    public override void OnInteracted(PlayerInteraction playerInteraction)
    {
        //Open dialog box
        if (hasID)
        {
            DialogueManager.Instance.AddToDialogueQueue(
            new DialogueData(
                "Now ask what i want to order", null,
                new Dictionary<string, UnityAction>()
                {
                    {"Ask for ID", GiveorReturnID },
                    {"Ask for Order", GiveOrder },
                    {"Nevermind", OnNevermind }
                }));
        }
        else if (!hasID)
        {
            DialogueManager.Instance.AddToDialogueQueue(new DialogueData(
                "Yes?", null,
                new Dictionary<string, UnityAction>()
                {
                    {"Ask for ID", GiveorReturnID },
                    {"Ask for Order", GiveOrder },
                    {"Nevermind", OnNevermind }
                }));
        }
    }

    void yes()
    {
        GameStateManager.Instance.GetDialogueManager().AddToDialogueQueue(
                        new DialogueData(
                            "Great! To start off, look at me and ask me for my order. \n(Press F to interact)"));

        GameStateManager.Instance.GetDialogueManager().Dequeue();
    }
    void no()
    {
        GameStateManager.Instance.GetDialogueManager().AddToDialogueQueue(
                        new DialogueData("Okey then! I wish you good luck on your bartending journey!", leave));

        GameStateManager.Instance.GetDialogueManager().Dequeue();
    }
    void leave()
    {
        navAgent.SetDestination(new Vector3(6.5f, 0, -1.7f));
        step = 15;
    }


    protected override void GiveOrder()
    {
        base.GiveOrder();

        DialogueManager.Instance.AddToDialogueQueue(new DialogueData(
                "Now, as you can see in right top of the screen is my order. \n" +
                "If you dont know how to make it, press Tab to get the drinks guide!"));

    }


}

