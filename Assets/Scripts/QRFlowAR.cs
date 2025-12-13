using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class QRFlow : MonoBehaviour
{
    public ARTrackedImageManager trackedImageManager;

    public GameObject giftBoxPrefab;
    public GameObject[] collectiblePrefabs;

    public GameObject openButton;
    public GameObject collectButton;

    public GameObject currentCollectible;

    private bool qrDetected = false;
    private GameObject currentGiftBox;

    void Start()
    {
        if (openButton != null)
            openButton.SetActive(false);

        if (collectButton != null)
            collectButton.SetActive(false);
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

            Transform cam = Camera.main.transform;
            Vector3 spawnPos = cam.position + cam.forward * 0.3f;
            Quaternion spawnRot = Quaternion.Euler(0, cam.eulerAngles.y, 0);

            currentGiftBox = Instantiate(giftBoxPrefab, spawnPos, spawnRot);
            currentGiftBox.transform.localScale = Vector3.one * 0.1f;

            Rigidbody rb = currentGiftBox.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;

            if (openButton != null)
                openButton.SetActive(true);
        }
    }

    public void OpenGift()
    {
        if (currentGiftBox == null) return;

        if (openButton != null)
            openButton.SetActive(false);

        int index = Random.Range(0, collectiblePrefabs.Length);
        GameObject chosenCollectible = collectiblePrefabs[index];

        currentCollectible = Instantiate(chosenCollectible);
        currentCollectible.transform.position = currentGiftBox.transform.position + new Vector3(0, 0.05f, 0);
        currentCollectible.transform.rotation = currentGiftBox.transform.rotation;
        currentCollectible.transform.localScale = Vector3.one * 0.05f;

        Rigidbody rb = currentCollectible.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        Destroy(currentGiftBox);

        if (collectButton != null)
            collectButton.SetActive(true);
    }

    public void CollectItemButton()
    {
        if (currentCollectible == null) return;

        Destroy(currentCollectible);
        currentCollectible = null;

        if (collectButton != null)
            collectButton.SetActive(false);
    }
}
