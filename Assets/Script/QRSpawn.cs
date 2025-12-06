using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class QRFlow : MonoBehaviour
{
    public ARTrackedImageManager imageManager;
    public GameObject giftBox;
    public GameObject collectible;
    public GameObject openButton;

    private bool qrDetected = false;

    void Start()
    {
        // hide everything at start
        giftBox.SetActive(false);
        collectible.SetActive(false);
        openButton.SetActive(false);
    }

    void OnEnable()
    {
        imageManager.trackedImagesChanged += OnImageChanged;
    }

    void OnDisable()
    {
        imageManager.trackedImagesChanged -= OnImageChanged;
    }

    void OnImageChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (var trackedImage in args.added)
        {
            HandleQR(trackedImage);
        }

        foreach (var trackedImage in args.updated)
        {
            HandleQR(trackedImage);
        }
    }

    void HandleQR(ARTrackedImage trackedImage)
    {
        if (trackedImage.referenceImage.name != "ockQR") return;

        // QR is detected for the first time
        if (!qrDetected && trackedImage.trackingState == TrackingState.Tracking)
        {
            qrDetected = true;

            // spawn box on QR position
            Transform cam = Camera.main.transform;
            giftBox.transform.position = cam.position + cam.forward * 0.2f;
            giftBox.transform.rotation = Quaternion.LookRotation(cam.forward);
            giftBox.SetActive(true);
            openButton.SetActive(true);
        }
    }

    public void OpenGift()
    {
        giftBox.SetActive(false);
        openButton.SetActive(false);
        collectible.SetActive(true);
    }

}
