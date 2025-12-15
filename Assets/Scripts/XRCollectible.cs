using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable))]
public class XRCollectible : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable interactable;

    // Initializes the XR interactable component and sets up the listener for when the object is collected
    void Awake()
    {
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
        interactable.selectEntered.AddListener(OnCollected);
    }

    // Handles the collection event by logging the collection and destroying the collectible object
    private void OnCollected(SelectEnterEventArgs args)
    {
        Debug.Log(gameObject.name + " collected!");


        Destroy(gameObject); // Remove collectible
    }
}
