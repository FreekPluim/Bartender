using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public bool paused = false;

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
}
