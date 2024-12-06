using UnityEngine;

public class DrinksGuide : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject GuideUI;

    public void OnInteracted(PlayerInteraction playerInteraction)
    {
        GuideUI.SetActive(true);
        GameStateManager.Instance.Pause();
    }
}
