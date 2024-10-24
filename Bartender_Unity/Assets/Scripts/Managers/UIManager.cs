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
    [HideInInspector] public UnityEvent<UIManager> onAskedForOrder;
    [HideInInspector] public UnityEvent<UIManager> onNevermind;


    //Buttons
    [SerializeField] Button ButtonAskForOrder;
    [SerializeField] Button ButtonAskForID;
    [SerializeField] Button ButtonReturnID;
    [SerializeField] Button ButtonNevermind;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else { Destroy(this); }
    }

    public void AskForID()
    {
        onAskedForID.Invoke(this);
        Questions.SetActive(false);
        GameStateManager.Instance.Pause();
    }
    public void AskForOrder()
    {
        onAskedForID.Invoke(this);
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

    }
    public void OnReturnedPressed()
    {
        ID.SetActive(false);
    }
}
