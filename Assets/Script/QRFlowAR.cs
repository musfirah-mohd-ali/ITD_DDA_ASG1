using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class QRFlow : MonoBehaviour
{
    [Header("AR Components")]
    public ARTrackedImageManager trackedImageManager;

    [Header("Prefabs")]
    public GameObject giftBoxPrefab;
    public GameObject[] collectiblePrefabs;

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
        if (trackedImage.referenceImage.name != "ockQR") return;

        if (!qrDetected && trackedImage.trackingState == TrackingState.Tracking)
        {
            qrDetected = true;
            Debug.Log("QR Detected! Spawning gift box...");

            // Spawn box 30 cm in front of camera
            Transform cam = Camera.main.transform;
            Vector3 spawnPos = cam.position + cam.forward * 0.3f;

            // Rotation: upright, facing the player
            Quaternion spawnRot = Quaternion.Euler(0, cam.eulerAngles.y, 0);

            // Spawn gift box
            currentGiftBox = Instantiate(giftBoxPrefab, spawnPos, spawnRot);
            currentGiftBox.SetActive(true);
            currentGiftBox.transform.localScale = Vector3.one * 0.1f;

            if (openButton != null)
                openButton.SetActive(true);
        }
    }

    // Assign this to your Open Button OnClick
    public void OpenGift()
    {
        if (currentGiftBox == null) return;

        if (openButton != null)
            openButton.SetActive(false);

        // Pick a random collectible
        int index = Random.Range(0, collectiblePrefabs.Length);
        GameObject chosenCollectible = collectiblePrefabs[index];

        // Spawn collectible slightly above gift box
        GameObject collectible = Instantiate(chosenCollectible);
        collectible.transform.position = currentGiftBox.transform.position + new Vector3(0, 0.05f, 0);
        collectible.transform.rotation = currentGiftBox.transform.rotation;
        collectible.transform.localScale = Vector3.one * 0.05f;

        // Destroy the gift box
        Destroy(currentGiftBox);

        Debug.Log("Gift opened! Spawned: " + chosenCollectible.name);
    }
}
