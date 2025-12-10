using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class QRFlowAR : MonoBehaviour
{
    [Header("AR Components")]
    public ARTrackedImageManager trackedImageManager;

    [Header("Prefabs")]
    public GameObject giftBoxPrefab;
    public GameObject collectiblePrefab;

    [Header("UI")]
    public GameObject openButton;

    private bool qrDetected = false;
    private GameObject currentGiftBox;

    void Start()
    {
        if (openButton != null)
            openButton.SetActive(false);
    }

    void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnImageChanged;
    }

    void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnImageChanged;
    }

    private void OnImageChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (var trackedImage in args.added)
            HandleQR(trackedImage);

        foreach (var trackedImage in args.updated)
            HandleQR(trackedImage);
    }

    private void HandleQR(ARTrackedImage trackedImage)
    {
        // Change "ockQR" to the exact name of your reference image in ARTrackedImageLibrary
        if (trackedImage.referenceImage.name != "ockQR")
            return;

        if (!qrDetected && trackedImage.trackingState == TrackingState.Tracking)
        {
            qrDetected = true;
            Debug.Log("QR Detected! Spawning gift box...");

            // Instantiate gift box in front of camera
            if (giftBoxPrefab != null)
            {
                currentGiftBox = Instantiate(giftBoxPrefab);
                Transform cam = Camera.main.transform;
                currentGiftBox.transform.position = cam.position + cam.forward * 0.3f; // 30cm in front
                currentGiftBox.transform.rotation = cam.rotation;
                currentGiftBox.transform.localScale = Vector3.one * 0.1f;
                currentGiftBox.SetActive(true);
            }

            if (openButton != null)
                openButton.SetActive(true);
        }
    }

    // Assign this to your Open Button OnClick in Inspector
    public void OpenGift()
    {
        if (currentGiftBox != null)
            Destroy(currentGiftBox);

        if (openButton != null)
            openButton.SetActive(false);

        if (collectiblePrefab != null)
        {
            GameObject collectible = Instantiate(collectiblePrefab);
            Transform cam = Camera.main.transform;
            collectible.transform.position = cam.position + cam.forward * 0.3f;
            collectible.transform.rotation = cam.rotation;
            collectible.SetActive(true);
        }

        Debug.Log("Gift opened and collectible spawned!");
    }
}
