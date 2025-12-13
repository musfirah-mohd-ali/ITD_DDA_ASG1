using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using System.Collections.Generic;
using System.Collections;

public class CollectibleSpawner : MonoBehaviour
{
    [Header("Collectible Prefabs")]
    public GameObject curryPrefab;
    public GameObject fballsPrefab;
    public GameObject sotongPrefab;
    public GameObject wingPrefab;

    [Header("UI Buttons")]
    public GameObject curryButton;
    public GameObject fballsButton;
    public GameObject sotongButton;
    public GameObject wingButton;

    private Dictionary<string, GameObject> prefabMap;

    void Start()
    {
        prefabMap = new Dictionary<string, GameObject>()
        {
            { "hasCurry", curryPrefab },
            { "hasFBalls", fballsPrefab },
            { "hasSotong", sotongPrefab },
            { "hasWing", wingPrefab }
        };

        HideAllButtons();
        LoadCollection();
    }

    void HideAllButtons()
    {
        curryButton.SetActive(false);
        fballsButton.SetActive(false);
        sotongButton.SetActive(false);
        wingButton.SetActive(false);
    }

    void LoadCollection()
    {
        if (FirebaseAuth.DefaultInstance.CurrentUser == null)
        {
            Debug.LogWarning("No Firebase user logged in!");
            return;
        }

        string uid = FirebaseAuth.DefaultInstance.CurrentUser.UserId;

        FirebaseDatabase.DefaultInstance
            .GetReference(uid)
            .Child("collections")
            .Child("basic")
            .GetValueAsync()
            .ContinueWith(task =>
            {
                if (!task.IsCompleted || task.Result == null) return;

                DataSnapshot snapshot = task.Result;

                foreach (var item in snapshot.Children)
                {
                    if (item.Value.ToString() == "True")
                    {
                        StartCoroutine(EnableButtonNextFrame(item.Key));
                    }
                }
            });
    }

    IEnumerator EnableButtonNextFrame(string key)
    {
        yield return null; // wait one frame to ensure UI updates on main thread
        EnableButton(key);
    }

    void EnableButton(string key)
    {
        if (key == "hasCurry") curryButton.SetActive(true);
        if (key == "hasFBalls") fballsButton.SetActive(true);
        if (key == "hasSotong") sotongButton.SetActive(true);
        if (key == "hasWing") wingButton.SetActive(true);
        Debug.Log($"Enabled button for {key}");
    }

    public void SpawnCollectible(string key)
    {
        if (!prefabMap.ContainsKey(key)) return;

        Transform cam = Camera.main.transform;
        Vector3 spawnPos = cam.position + cam.forward * 0.4f;
        Quaternion spawnRot = Quaternion.Euler(0, cam.eulerAngles.y, 0);

        GameObject obj = Instantiate(prefabMap[key], spawnPos, spawnRot);
        obj.transform.localScale = Vector3.one * 0.05f;
    }
}
