using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] Camera Cam;

    public Transform Hand;
    public IngredientSo IngredientInHand;
    public DrinkSo DrinkInHand;
    public LayerMask IgnoreLayer;

    private void Awake()
    {
        if (Cam == null)
        {
            Cam = Camera.main;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(Settings.instance.Interact) && !GameStateManager.Instance.paused)
        {
            RaycastHit hit;
            if (Physics.Raycast(Cam.transform.position, Cam.transform.forward, out hit, 5f, IgnoreLayer))
            {
                if (hit.transform.TryGetComponent(out IInteractable interactable))
                {
                    interactable.OnInteracted(this);
                }
                else
                {
                    //Clear hand if something is in it;
                    if (IngredientInHand != null || DrinkInHand != null)
                    {
                        Transform itemInHand = RemoveItemFromHand();
                        itemInHand.transform.position = hit.point;
                        itemInHand.transform.rotation = Quaternion.Euler(0, 0, 0);
                        itemInHand.GetComponent<Collider>().enabled = true;
                    }
                }
            }
        }
    }

    public Transform RemoveItemFromHand(bool destroyItem = false)
    {
        Transform itemInHand = Hand.GetChild(0);
        itemInHand.parent = null;

        if (destroyItem)
        {
            Destroy(itemInHand.gameObject);
        }

        IngredientInHand = null;
        DrinkInHand = null;
        return itemInHand;
    }
}
