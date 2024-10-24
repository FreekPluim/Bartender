using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour, IInteractable
{
    [SerializeField] List<DrinkSo> allDrinks;

    string Name = "Dopinder";
    //Age
    int Age;

    //Order
    DrinkSo order;

    bool gaveOrder = false;
    bool hasID = true;


    public void OnInteracted(PlayerInteraction playerInteraction)
    {
        //Open dialog box
        if (playerInteraction.DrinkInHand == null)
        {
            if (UIManager.instance != null)
            {
                UIManager.instance.onInteractedWithCustomer.Invoke();
                UIManager.instance.onAskedForID.AddListener(GiveID);
                UIManager.instance.onAskedForOrder.AddListener(GiveOrder);
                UIManager.instance.onNevermind.AddListener(OnNevermind);
                UIManager.instance.InteractedWithCustomer(this);
            }
            else Debug.LogError("**NO UIMANAGER FOUND**");
        }
        else
        {
            //hand customer drink
        }
    }

    void GiveID(UIManager uiManager)
    {
        uiManager.SetID(Name, Age);
        hasID = false;
    }

    void GiveOrder(UIManager uiManager)
    {

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
}
