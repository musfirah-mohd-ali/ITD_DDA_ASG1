    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.XR.ARFoundation;
    using UnityEngine.XR.ARSubsystems;


    public class ImageTracker : MonoBehaviour
    {
        [SerializeField]
        private ARTrackedImageManager trackedImageManager;

        [SerializeField]
        private GameObject[] placeablePrefabs;

        private Dictionary<string, GameObject> spawnedPrefabs = new Dictionary<string, GameObject>();

        private Dictionary<GameObject, GameObject> spawnedObjects = new Dictionary<GameObject, GameObject>();

        private string[] someArray = new string[]{"Image1", "Image2", "Image3"};

        private void Start()
        {
            if (trackedImageManager != null)
            {
                trackedImageManager.trackablesChanged.AddListener(OnImageChanged);
                SetupPrefabs();
            }
        }

        void SetupPrefabs()
        {
            foreach (GameObject prefab in placeablePrefabs)
            {
                GameObject newPrefab = Instantiate(prefab);
                newPrefab.name = prefab.name;
                newPrefab.SetActive(false);
                spawnedPrefabs.Add(prefab.name, newPrefab);
                spawnedObjects.Add(newPrefab, prefab);
            }
        }

        void OnImageChanged(ARTrackablesChangedEventArgs<ARTrackedImage> eventArgs)
        {
            foreach (ARTrackedImage trackedImage in eventArgs.added)
            {
                UpdateImage(trackedImage);
            }

            foreach (ARTrackedImage trackedImage in eventArgs.updated)
            {
                UpdateImage(trackedImage);
            }

            foreach (KeyValuePair<TrackableId, ARTrackedImage> lostObj in eventArgs.removed)
            {
                UpdateImage(lostObj.Value);
            }
        }

        void UpdateImage(ARTrackedImage trackedImage)
        {
            if (trackedImage == null) return;
            string imageName = trackedImage.referenceImage.name;
            if (!spawnedPrefabs.ContainsKey(imageName))
            {
                Debug.LogWarning("No prefab found for image: " + imageName);
                return;
            }
            GameObject obj = spawnedPrefabs[imageName];
            if (trackedImage.trackingState == TrackingState.Limited ||
                trackedImage.trackingState == TrackingState.None)
            {
                obj.transform.SetParent(null);
                obj.SetActive(false);
            }
            else if (trackedImage.trackingState == TrackingState.Tracking)
            {
                obj.transform.SetParent(trackedImage.transform);
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localRotation = Quaternion.identity;
                obj.SetActive(true);
            }
        }

    }
