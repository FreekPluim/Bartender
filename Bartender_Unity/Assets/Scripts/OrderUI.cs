using TMPro;
using UnityEngine;

public class OrderUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI DrinkName;

    public void SetDrink(DrinkSo drink)
    {
        DrinkName.text = drink.name;
    }
}
