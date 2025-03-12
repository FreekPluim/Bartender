using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    /// ID Stuff
    [SerializeField] GameObject ID;
    [SerializeField] TextMeshProUGUI Name;
    [SerializeField] TextMeshProUGUI Age;

    /// Order Prefab
    [SerializeField] Transform OrderHolder;
    [SerializeField] GameObject OrderPrefab;

    //Recipe book
    [SerializeField] GameObject RecipeBook;

    Dictionary<GameObject, OrderUI> orders = new Dictionary<GameObject, OrderUI>();

    [SerializeField] GameObject IngredientPrefab;
    [SerializeField] GameObject IngredientList;
    [SerializeField] GameObject IngredientsInShakerUI;
    [SerializeField] GameObject ShakerUI;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else { Destroy(this); }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            RecipeBook.SetActive(!RecipeBook.activeSelf);
            GameStateManager.Instance.PauseUnpause();
        }
    }

    public void SetID(string pName, int pAge)
    {
        Name.text = pName;
        Age.text = pAge.ToString();

        ID.SetActive(!ID.activeSelf);
    }
    public void AddOrder(DrinkSo orderedDrink, GameObject orderer)
    {
        GameObject order = Instantiate(OrderPrefab, OrderHolder);
        OrderUI orderUI = order.GetComponent<OrderUI>();
        orderUI.SetDrink(orderedDrink);
        orders.Add(orderer, orderUI);
    }
    public void RemoveOrder(GameObject orderer)
    {
        Destroy(orders[orderer].gameObject);
        orders.Remove(orderer);
    }


}
