using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Drink", menuName = "Drinks/New Drink")]

public class DrinkSo : ScriptableObject
{
    public List<IngredientSo> ingredients;
    public GameObject drink;
}
