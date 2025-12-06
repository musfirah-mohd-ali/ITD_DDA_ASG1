using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class QRSpawn : MonoBehaviour
{
    public ARTrackedImageManager imageManager; // the AR image tracker on Session Origin
    public GameObject giftBoxPrefab; // the box i wanna spawn when QR detected

    private GameObject spawnedGiftBox; // so it doesnt spawn 39392 duplicates lol

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnImageChanged;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnImageChanged;
    }

    void OnImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
            UpdateImage(trackedImage);

        foreach (var trackedImage in eventArgs.updated)
            UpdateImage(trackedImage);

        foreach (var trackedImage in eventArgs.removed)
            UpdateImage(trackedImage);
    }

}
