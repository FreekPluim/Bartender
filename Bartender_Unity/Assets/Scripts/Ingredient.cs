using TMPro;
using UnityEngine;

public class Ingredient : MonoBehaviour, IInteractable
{
    public IngredientSo ingredient;
    [SerializeField] TextMeshProUGUI label;
    [SerializeField] Collider col;

    private void Start()
    {
        label.text = ingredient.name;
        col = GetComponent<Collider>();
    }

    public void OnInteracted(PlayerInteraction playerInteraction)
    {
        //Pickup drink
        if (playerInteraction.IngredientInHand == null && playerInteraction.DrinkInHand == null)
        {
            //Add ingredient to hand
            playerInteraction.IngredientInHand = ingredient;

            //Set item on postion of player hand
            transform.parent = playerInteraction.Hand;
            transform.localPosition = Vector3.zero;

            col.enabled = false;
        }
    }
}
