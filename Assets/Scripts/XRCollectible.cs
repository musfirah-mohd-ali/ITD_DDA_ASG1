using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable))]
public class XRCollectible : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable interactable;

    void Awake()
    {
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
        // Subscribe to the new event
        interactable.selectEntered.AddListener(OnCollected);
    }

    private void OnCollected(SelectEnterEventArgs args)
    {
        Debug.Log(gameObject.name + " collected!");

        // TODO: Save collectible to JSON/Firebase if needed

        Destroy(gameObject); // Remove collectible
    }
}
