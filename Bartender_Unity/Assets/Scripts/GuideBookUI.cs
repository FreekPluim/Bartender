using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GuideBookUI : MonoBehaviour
{
    [SerializeField] List<DrinkSo> AllMixDrinks;
    int currentPage = 0;

    public TextMeshProUGUI DrinkName;
    public TextMeshProUGUI Ingredients;

    public Button Forward, Backward;


    private void Start()
    {
        SetPageVariables();
        Forward.onClick.AddListener(OnForwardPressed);
        Backward.onClick.AddListener(OnBackwardPressed);
    }

    private void Update()
    {
        if (currentPage == 0)
        {
            Backward.gameObject.SetActive(false);
        }
        else { Backward.gameObject.SetActive(true); }
        if (currentPage == AllMixDrinks.Count - 1)
        {
            Forward.gameObject.SetActive(false);
        }
        else { Forward.gameObject.SetActive(true); }
    }

    void SetPageVariables()
    {
        DrinkSo selectedDrink = AllMixDrinks[currentPage];

        DrinkName.text = selectedDrink.name;

        string ingredients = "";
        foreach (var item in selectedDrink.ingredients)
        {
            ingredients += "- " + item.name + "\n";
        }

        Ingredients.text = ingredients;
    }

    public void OnForwardPressed()
    {
        if (currentPage < AllMixDrinks.Count - 1) currentPage++;
        SetPageVariables();
    }

    public void OnBackwardPressed()
    {
        if (currentPage > 0) currentPage--;
        SetPageVariables();
    }



}
