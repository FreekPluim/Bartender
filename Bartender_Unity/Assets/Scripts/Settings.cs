using UnityEngine;

public class Settings : MonoBehaviour
{
    public static Settings instance;

    public KeyCode Interact = KeyCode.F;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(this);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
