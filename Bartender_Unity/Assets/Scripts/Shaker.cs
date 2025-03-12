using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Shaker : MonoBehaviour, IInteractable
{
    Camera Cam;
    [SerializeField] List<DrinkSo> Drinks;

    Dictionary<IngredientSo, int> CurrentIngredients = new Dictionary<IngredientSo, int>();

    bool ingredientsChanged = false;

    [SerializeField] GameObject incorrectDrink;
    [SerializeField] Transform drinkPlacementSpot;

    bool pickedUp = false;
    [SerializeField] GameObject IngredientPrefab;
    [SerializeField] Transform IngredientList;
    [SerializeField] GameObject CurrentIngredientsUI;

    //When picked up
    [SerializeField] GameObject shakerUI;
    [SerializeField] Slider mixedTracker;

    PlayerInteraction playerInteractor;

    private void Awake()
    {
        Cam = Camera.main;
    }

    public void OnInteracted(PlayerInteraction playerInteraction)
    {
        playerInteractor = playerInteraction;
        //Check if anything in hand
        if (playerInteraction.IngredientInHand != null)
        {
            //Open Dialogue for how much to add
            if (playerInteraction.IngredientInHand.Type == IngredientType.Liquid)
            {
                DialogueManager.Instance.AddToDialogueQueue(new DialogueData(
                    "How many shots do you want to add?", null, new Dictionary<string, UnityAction>()
                    {
                        { "1 shot", AddOneShot },
                        { "2 shots", AddTwoShots },
                        { "Nevermind", OnNevermind }
                    }
                    ));
            }
            if (playerInteraction.IngredientInHand.Type == IngredientType.Physical)
            {
                DialogueManager.Instance.AddToDialogueQueue(new DialogueData(
                    "How much do you want to add?", null, new Dictionary<string, UnityAction>()
                    {
                        { "1", AddOneShot },
                        { "2", AddTwoShots },
                        { "Nevermind", OnNevermind }
                    }
                    ));
            }

            GameStateManager.Instance.Pause();
        }
        else
        {
            if (CurrentIngredients.Count > 0)
            {
                //Pickup shaker and open HUD for shaker
                pickedUp = true;
                CurrentIngredientsUI.SetActive(false);



                #region Old code
                /*DrinkSo foundDrink = CompareLists();

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
                }*/
                #endregion
            }
        }
    }

    private void Update()
    {
        if (pickedUp)
        {
            if (!shakerUI.activeSelf) shakerUI.SetActive(true);
            if (!CurrentIngredientsUI.activeSelf) CurrentIngredientsUI.SetActive(false);


        }
        else
        {
            RaycastHit hit;
            if (Physics.Raycast(Cam.transform.position, Cam.transform.forward, out hit, 5f))
            {
                if (hit.transform.gameObject == gameObject)
                {
                    if (!CurrentIngredientsUI.activeSelf) ShowIngredients();
                }
                else
                {
                    CurrentIngredientsUI.SetActive(false);
                }
            }
            else
            {
                CurrentIngredientsUI.SetActive(false);
            }
        }
    }
    void ShowIngredients()
    {
        if (ingredientsChanged)
        {
            //Clear Previous
            for (int i = IngredientList.childCount; i > 0; i--)
            {
                Destroy(IngredientList.GetChild(i - 1).gameObject);
            }

            //CreateNew
            if (CurrentIngredients.Count > 0)
            {
                foreach (var ingredient in CurrentIngredients)
                {
                    GameObject obj = Instantiate(IngredientPrefab, IngredientList);
                    obj.GetComponent<IngredientUI>().SetValues(ingredient.Key.Name, ingredient.Value);
                }
            }
        }

        ingredientsChanged = false;
        CurrentIngredientsUI.SetActive(true);
    }
    private DrinkSo CompareLists()
    {
        List<IngredientSo> InShaker = new List<IngredientSo>(CurrentIngredients.Keys);
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
    void AddOneShot()
    {
        AddIngredients(playerInteractor.IngredientInHand, 1);
        ingredientsChanged = true;
        DialogueManager.Instance.Dequeue();
        GameStateManager.Instance.UnPause();
    }
    void AddTwoShots()
    {
        AddIngredients(playerInteractor.IngredientInHand, 2);
        ingredientsChanged = true;
        DialogueManager.Instance.Dequeue();
        GameStateManager.Instance.UnPause();
    }
    void AddIngredients(IngredientSo ingredient, int amount)
    {
        if (CurrentIngredients.ContainsKey(ingredient))
        {
            CurrentIngredients[ingredient] += amount;
            print("adding to existing");
        }
        else
        {
            CurrentIngredients.Add(ingredient, amount);
            print("adding new");
        }
        ShowIngredients();
    }
    void OnNevermind()
    {
        //Open UI for how much to add
        DialogueManager.Instance.Dequeue();
        GameStateManager.Instance.UnPause();
    }
}
