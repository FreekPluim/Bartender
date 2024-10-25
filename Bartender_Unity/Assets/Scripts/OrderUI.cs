using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OrderUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI DrinkName;

    public void SetDrink(DrinkSo drink)
    {
        DrinkName.text = drink.name;
    }
}
