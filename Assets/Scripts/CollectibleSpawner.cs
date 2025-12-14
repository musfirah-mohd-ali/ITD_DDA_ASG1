using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;

public class CollectibleSpawner : MonoBehaviour
{
    public GameObject curryPrefab;
    public GameObject fballsPrefab;
    public GameObject sotongPrefab;
    public GameObject wingPrefab;

    public GameObject curryButton;
    public GameObject fballsButton;
    public GameObject sotongButton;
    public GameObject wingButton;

    private Dictionary<string, GameObject> prefabMap;
    private Dictionary<string, GameObject> buttonMap;

    void Start()
    {
        prefabMap = new Dictionary<string, GameObject>()
        {
            { "hascurry", curryPrefab },
            { "hasfballs", fballsPrefab },
            { "hassotong", sotongPrefab },
            { "haswing", wingPrefab }
        };

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
        FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser;

        if (user == null)
        {
            Debug.LogWarning("No Firebase user logged in! Cannot load collection.");
            return;
        }

        string uid = user.UserId;

        FirebaseDatabase.DefaultInstance
            .GetReference(uid)
            .Child("collections")
            .Child("basic")
            .GetValueAsync()
            .ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError($"FIREBASE ERROR: {task.Exception}"); 
                    return;
                }
                if (task.IsCanceled)
                {
                    Debug.LogWarning("FIREBASE WARNING: Task canceled.");
                    return;
                }
                
                if (task.Result == null || !task.Result.Exists)
                {
                    Debug.LogWarning("FIREBASE WARNING: Data not found at the specified path.");
                    return;
                }

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
        yield return null;
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
