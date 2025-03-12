using TMPro;
using UnityEngine;

public class IngredientUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ingredientName;
    [SerializeField] TextMeshProUGUI IngredientAmount;

    public void SetValues(string name, int amount)
    {
        ingredientName.text = name;
        IngredientAmount.text = amount.ToString();
    }
}
