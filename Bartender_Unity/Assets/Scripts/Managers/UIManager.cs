using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] GameObject Questions;

    /// ID Stuff
    [SerializeField] GameObject ID;
    [SerializeField] TextMeshProUGUI Name;
    [SerializeField] TextMeshProUGUI Age;

    /// Order Prefab
    [SerializeField] Transform OrderHolder;
    [SerializeField] GameObject OrderPrefab;

    /// Events
    [HideInInspector] public UnityEvent onInteractedWithCustomer;
    [HideInInspector] public UnityEvent<UIManager> onAskedForID;
    [HideInInspector] public UnityEvent<UIManager> onReturnID;
    [HideInInspector] public UnityEvent<UIManager> onAskedForOrder;
    [HideInInspector] public UnityEvent<UIManager> onNevermind;


    //Buttons
    [SerializeField] Button ButtonAskForOrder;
    [SerializeField] Button ButtonAskForID;
    [SerializeField] Button ButtonReturnID;
    [SerializeField] Button ButtonNotOldEnough;
    [SerializeField] Button ButtonNevermind;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else { Destroy(this); }

        ButtonAskForID.onClick.AddListener(AskForID);
        ButtonAskForOrder.onClick.AddListener(AskForOrder);
        ButtonReturnID.onClick.AddListener(ReturnID);
        ButtonNevermind.onClick.AddListener(OnNevermindPressed);

    }

    public void AskForID()
    {
        onAskedForID.Invoke(this);
        Questions.SetActive(false);
        GameStateManager.Instance.Pause();
    }

    public void ReturnID()
    {
        onReturnID.Invoke(this);
        Questions.SetActive(false);
        ID.SetActive(false);
        GameStateManager.Instance.Pause();
    }

    public void AskForOrder()
    {
        onAskedForOrder.Invoke(this);
        Questions.SetActive(false);
        GameStateManager.Instance.Pause();
    }
    public void OnNevermindPressed()
    {
        onNevermind.Invoke(this);
        Questions.SetActive(false);
        GameStateManager.Instance.Pause();
    }
    public void InteractedWithCustomer(Customer customer)
    {
        //Check if has id
        if (customer.hasID)
        {
            ButtonAskForID.gameObject.SetActive(true);
            ButtonReturnID.gameObject.SetActive(false);
        }
        else 
        {
            ButtonAskForID.gameObject.SetActive(false);
            ButtonReturnID.gameObject.SetActive(true);
            ButtonNotOldEnough.gameObject.SetActive(true);
        }

        //Check if gave order
        if (!customer.gaveOrder) ButtonAskForOrder.gameObject.SetActive(true);
        else ButtonAskForOrder.gameObject.SetActive(false);

        Questions.SetActive(true);
        GameStateManager.Instance.Pause();
    }

    public void SetID(string pName, int pAge)
    {
        Name.text = pName;
        Age.text = pAge.ToString();

        ID.SetActive(true);
    }
    public void AddOrder(DrinkSo orderedDrink)
    {
        GameObject order = Instantiate(OrderPrefab, OrderHolder);
        order.GetComponent<OrderUI>().SetDrink(orderedDrink);
    }
}
