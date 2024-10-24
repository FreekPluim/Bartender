using UnityEngine;

public class Drink : MonoBehaviour, IInteractable
{
    public DrinkSo drink;
    public Collider col;

    private void Start()
    {
        col = GetComponent<Collider>();
    }

    public void OnInteracted(PlayerInteraction playerInteraction)
    {
        //Pickup drink
        if (playerInteraction.IngredientInHand == null && playerInteraction.IngredientInHand == null)
        {
            //Add ingredient to hand
            playerInteraction.DrinkInHand = drink;

            //Set item on postion of player hand
            transform.parent = playerInteraction.Hand;
            transform.localPosition = Vector3.zero;

            col.enabled = false;
        }
    }
}
