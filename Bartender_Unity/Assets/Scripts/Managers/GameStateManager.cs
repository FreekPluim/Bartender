using UnityEngine;
using UnityEngine.Events;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public bool paused = false;

    public int Day = 0;
    public int Time = 8 * 60;

    public UnityEvent NewDay;

    //Managers
    [SerializeField] CustomerManager customerManager;
    [SerializeField] UIManager uiManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(this);
    }

    public void Pause()
    {
        paused = !paused;

        if (paused) Cursor.lockState = CursorLockMode.None;
        else Cursor.lockState = CursorLockMode.Locked;

        Cursor.visible = !Cursor.visible;

    }

    public void NextDay()
    {
        NewDay?.Invoke();
    }

    //Getters
    public CustomerManager GetCustomerManager()
    {
        if (customerManager == null)
        {
            Debug.LogError("No Customer Manger Refferenced in - GameStateManager -");
            return null;
        }

        return customerManager;
    }
    public UIManager GetUIManager()
    {
        if (uiManager == null)
        {
            Debug.LogError("No Customer Manger Refferenced in - GameStateManager -");
            return null;
        }

        return uiManager;
    }
}
