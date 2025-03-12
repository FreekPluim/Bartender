using UnityEngine;
public enum IngredientType { Liquid, Physical }

[CreateAssetMenu(fileName = "New Ingredient", menuName = "Ingredients/New Ingredient")]
public class IngredientSo : ScriptableObject
{
    public string Name;
    public IngredientType Type;
}
