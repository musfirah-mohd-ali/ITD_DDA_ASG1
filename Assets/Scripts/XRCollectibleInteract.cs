using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRCollectibleInteract : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable interactable;
    private Camera arCamera;
    private bool isDragging;

    void Awake()
    {
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
        arCamera = Camera.main;
    }

    void OnEnable()
    {
        interactable.selectEntered.AddListener(OnSelect);
        interactable.selectExited.AddListener(OnRelease);
    }

    void OnDisable()
    {
        interactable.selectEntered.RemoveListener(OnSelect);
        interactable.selectExited.RemoveListener(OnRelease);
    }

    void Update()
    {
        if (!isDragging) return;
        Debug.Log("Dragging collectible...");

        Vector3 targetPos = arCamera.transform.position + arCamera.transform.forward * 0.4f;
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 12f);
    }

    void OnSelect(SelectEnterEventArgs args)
    {
        if (args.interactorObject is UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor)
        {
            Debug.Log("Collectible selected via ray interactor, destroying...");
            Destroy(gameObject);
        }
        else
        {
            isDragging = true;
        }
    }

    void OnRelease(SelectExitEventArgs args)
    {
        Debug.Log("Collectible released.");
        isDragging = false;
    }
}
