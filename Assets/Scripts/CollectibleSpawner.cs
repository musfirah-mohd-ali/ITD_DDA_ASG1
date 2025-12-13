using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;

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
    private Dictionary<string, GameObject> buttonMap;

    void Start()
    {
        // Map lowercase keys to prefabs
        prefabMap = new Dictionary<string, GameObject>()
        {
            { "hascurry", curryPrefab },
            { "hasfballs", fballsPrefab },
            { "hassotong", sotongPrefab },
            { "haswing", wingPrefab }
        };

        // Map lowercase keys to buttons
        buttonMap = new Dictionary<string, GameObject>()
        {
            { "hascurry", curryButton },
            { "hasfballs", fballsButton },
            { "hassotong", sotongButton },
            { "haswing", wingButton }
        };

        HideAllButtons();
        LoadCollection();
    }

    void HideAllButtons()
    {
        foreach (var btn in buttonMap.Values)
            btn.SetActive(false);
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
                    string key = item.Key.Trim().ToLower();
                    string value = item.Value.ToString().Trim().ToLower();

                    Debug.Log($"Found key: {key}, value: {value}");

                    if (value == "true" && buttonMap.ContainsKey(key))
                        StartCoroutine(EnableButtonNextFrame(key));
                }
            });
    }

    IEnumerator EnableButtonNextFrame(string key)
    {
        yield return null; // wait one frame for UI
        EnableButton(key);
    }

    void EnableButton(string key)
    {
        if (buttonMap.ContainsKey(key))
        {
            buttonMap[key].SetActive(true);
            Debug.Log($"Enabled button for {key}");
        }
    }

    public void SpawnCollectible(string key)
    {
        key = key.Trim().ToLower();

        if (!prefabMap.ContainsKey(key)) return;

        Transform cam = Camera.main.transform;
        Vector3 spawnPos = cam.position + cam.forward * 0.4f;
        Quaternion spawnRot = Quaternion.Euler(0, cam.eulerAngles.y, 0);

        GameObject obj = Instantiate(prefabMap[key], spawnPos, spawnRot);
        obj.transform.localScale = Vector3.one * 0.05f;

        Debug.Log($"Spawned collectible for {key}");
    }
}
