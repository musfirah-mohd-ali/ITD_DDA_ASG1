using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class QRFlow : MonoBehaviour
{
    public ARTrackedImageManager trackedImageManager;
    public PanelManager panelManager;
    public AudioManager audioManager;


    public AudioClip giftSpawnSound;

    public GameObject giftBoxPrefab;
    public GameObject[] collectiblePrefabs;
    public GameObject QRFramePanel;
    public GameObject HomePanel;

    public GameObject openButton;
    public GameObject collectButton;
    public GameObject confetti;
    private GameObject currentConfetti;

    public GameObject currentCollectible;

    private bool qrDetected = false;
    private GameObject currentGiftBox;

    // Initializes the scene by hiding the open and collect buttons
    void Start()
    {
        if (openButton != null)
            openButton.SetActive(false);

        if (collectButton != null)
            collectButton.SetActive(false);
    }

    // Subscribes to the tracked images changed event when the script is enabled
    void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnImageChanged;
    }

    // Unsubscribes from the tracked images changed event when the script is disabled
    void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnImageChanged;
    }

    // Handles changes in tracked images, processing added and updated images
    private void OnImageChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (var trackedImage in args.added)
            HandleQR(trackedImage);

        foreach (var trackedImage in args.updated)
            HandleQR(trackedImage);
    }

    // Processes a tracked QR image, spawning a gift box if the QR is detected and not already handled
    private void HandleQR(ARTrackedImage trackedImage)
    {
        if (trackedImage.referenceImage.name != "ockQR") return;

        if (!qrDetected && trackedImage.trackingState == TrackingState.Tracking)
        {
            qrDetected = true;

            Transform cam = Camera.main.transform;
            Vector3 spawnPos = cam.position + cam.forward * 0.3f;
            Quaternion spawnRot = Quaternion.Euler(0, cam.eulerAngles.y, 0);

            currentGiftBox = Instantiate(giftBoxPrefab, spawnPos, spawnRot);
            currentGiftBox.transform.localScale = Vector3.one * 0.1f;
            audioManager.PlaySound(giftSpawnSound);

            Rigidbody rb = currentGiftBox.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;

            if (openButton != null)
                openButton.SetActive(true);
            if (QRFramePanel != null)
                QRFramePanel.SetActive(false);
        }
    }

    // Opens the gift box, spawns a random collectible and confetti, then destroys the gift box
    public void OpenGift()
    {
        if (currentGiftBox == null) return;

        if (openButton != null)
            openButton.SetActive(false);

        int index = Random.Range(0, collectiblePrefabs.Length);
        GameObject chosenCollectible = collectiblePrefabs[index];


        // Spawn the collectible above the gift box
        currentCollectible = Instantiate(chosenCollectible);
        currentCollectible.transform.position = currentGiftBox.transform.position + new Vector3(0, 0.05f, 0);
        currentCollectible.transform.rotation = currentGiftBox.transform.rotation;
        currentCollectible.transform.localScale = Vector3.one * 0.05f;

        // Spawn confetti effect
        currentConfetti = Instantiate(confetti, currentGiftBox.transform.position, Quaternion.identity);

        Rigidbody rb = currentCollectible.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        Destroy(currentGiftBox);

        if (collectButton != null)
            collectButton.SetActive(true);
    }

    // Collects the spawned item, unlocks it in the panel manager, cleans up objects, and switches to home panel
    public void CollectItemButton()
    {
        if (currentCollectible == null) return;

        CollectibleIdentity identity = currentCollectible.GetComponent<CollectibleIdentity>();

        if (identity != null)
        {
            panelManager.UnlockCollectible(identity.type);
        }


        Destroy(currentCollectible);
        Destroy(currentConfetti);

        currentCollectible = null;
        currentConfetti = null;
        qrDetected = false;

        if (collectButton != null)
            collectButton.SetActive(false);

        panelManager.SwitchPanel(HomePanel);
    }
}
