using System.Collections.Generic;
using UnityEngine;

public class Shaker : MonoBehaviour, IInteractable
{
    [SerializeField] int IngredientMax;
    [SerializeField] List<DrinkSo> Drinks;

    List<IngredientSo> CurrentIngredients = new List<IngredientSo>();

    [SerializeField] GameObject incorrectDrink;
    [SerializeField] Transform drinkPlacementSpot;

    public void OnInteracted(PlayerInteraction playerInteraction)
    {
        //Check if anything in hand
        if (playerInteraction.IngredientInHand != null)
        {
            //check if ingredient max is reached
            if (CurrentIngredients.Count >= IngredientMax)
            {
                //Play animation that cant add item
                Debug.Log("Shaker is already full");
                return;
            }

            //Add to shaker
            CurrentIngredients.Add(playerInteraction.IngredientInHand);
        }
        else
        {
            if (CurrentIngredients.Count > 0)
            {
                //shake and create drink
                DrinkSo foundDrink = CompareLists();

                if (foundDrink != null)
                {
                    //Drink created
                    Instantiate(foundDrink.drink, drinkPlacementSpot.position, Quaternion.identity);
                    CurrentIngredients.Clear();

                }
                else
                {
                    //Create incorrect drink 
                    Instantiate(incorrectDrink, drinkPlacementSpot.position, Quaternion.identity);
                    CurrentIngredients.Clear();
                }
            }
        }
    }

    private DrinkSo CompareLists()
    {
        List<IngredientSo> InShaker = new List<IngredientSo>(CurrentIngredients);
        DrinkSo returnDrink = null;

        //Compare in shaker with in drinks
        foreach (var drink in Drinks)
        {
            List<IngredientSo> InDrink = new List<IngredientSo>(drink.ingredients);

            if (InShaker.Count == InDrink.Count)
            {
                for (int i = InShaker.Count - 1; i >= 0; i--)
                {
                    for (int j = 0; j < InDrink.Count; j++)
                    {
                        if (InShaker[i].name == InDrink[j].name)
                        {
                            InShaker.Remove(InShaker[i]);
                            InDrink.Remove(InDrink[j]);
                            j = InDrink.Count;
                        }
                    }
                }

            }

            if (InShaker.Count == 0 && InDrink.Count == 0)
            {
                returnDrink = drink;
                break;
            }
        }

        return returnDrink;
    }
}
