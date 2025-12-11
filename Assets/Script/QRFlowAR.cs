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

    [HideInInspector]
    public GameObject currentCollectible; // Track the spawned collectible

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

            Transform cam = Camera.main.transform;

            // Spawn box 30 cm in front of camera
            Vector3 spawnPos = cam.position + cam.forward * 0.3f;

            // Rotation: upright, facing the player
            Quaternion spawnRot = Quaternion.Euler(0, cam.eulerAngles.y, 0);

            // Spawn gift box
            currentGiftBox = Instantiate(giftBoxPrefab, spawnPos, spawnRot);
            currentGiftBox.transform.localScale = Vector3.one * 0.1f;

            Rigidbody rb = currentGiftBox.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;

            currentGiftBox.transform.SetParent(null);

            if (openButton != null)
                openButton.SetActive(true);
        }
    }

    public void OpenGift()
    {
        if (currentGiftBox == null) return;

        if (openButton != null)
            openButton.SetActive(false);

        // Pick a random collectible
        int index = Random.Range(0, collectiblePrefabs.Length);
        GameObject chosenCollectible = collectiblePrefabs[index];

        // Spawn collectible slightly above gift box
        currentCollectible = Instantiate(chosenCollectible);
        currentCollectible.transform.position = currentGiftBox.transform.position + new Vector3(0, 0.05f, 0);
        currentCollectible.transform.rotation = currentGiftBox.transform.rotation;
        currentCollectible.transform.localScale = Vector3.one * 0.05f;

        Rigidbody rb = currentCollectible.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        currentCollectible.transform.SetParent(null);

        // Add tap collection
        AddTapCollect(currentCollectible);

        // Destroy the gift box
        Destroy(currentGiftBox);

        Debug.Log("Gift opened! Spawned: " + chosenCollectible.name);
    }

    // Adds tap interaction to spawned collectible
    private void AddTapCollect(GameObject collectible)
    {
        TapCollect tap = collectible.AddComponent<TapCollect>();
        tap.qrFlow = this;
    }

    // Called by TapCollect script
    public void CollectItem()
    {
        if (currentCollectible == null) return;

        Debug.Log("Collected: " + currentCollectible.name);

        // TODO: Save collectible to JSON / Firebase here

        Destroy(currentCollectible);
        currentCollectible = null;
    }
}

// --------------------------
// Separate helper script attached dynamically
public class TapCollect : MonoBehaviour
{
    [HideInInspector]
    public QRFlow qrFlow;

    private void OnMouseDown()
    {
        if (qrFlow != null)
        {
            qrFlow.currentCollectible = this.gameObject;
            qrFlow.CollectItem();
        }
    }
}
