using UnityEngine;
using Firebase.Database;
using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

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
    private HashSet<string> collectedItems;

    void Start()
    {
        collectedItems = new HashSet<string>();

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
                    Debug.LogError($"FIREBASE ERROR (FAULTED): {task.Exception}"); 
                    return;
                }
                if (task.IsCanceled)
                {
                    Debug.LogWarning("FIREBASE WARNING (CANCELED).");
                    return;
                }
                
                if (task.Result == null || !task.Result.Exists)
                {
                    Debug.LogWarning("FIREBASE WARNING: Data not found at the specified path (Result is null or doesn't exist).");
                    return;
                }
                
                Debug.Log("FIREBASE SUCCESS: Starting data processing."); 
                
                DataSnapshot snapshot = task.Result;
                ProcessCollectionData(snapshot);
            });
    }

    private void ProcessCollectionData(DataSnapshot snapshot)
    {
        StartCoroutine(ProcessCollectionDataRoutine(snapshot));
    }

    private IEnumerator ProcessCollectionDataRoutine(DataSnapshot snapshot)
    {
        foreach (var item in snapshot.Children)
        {
            string key = item.Key.Trim().ToLower();
            
            bool isCollected = false;
            
            // --- ROBUST BOOLEAN CHECK ---
            if (item.Value is bool) 
            {
                isCollected = (bool)item.Value;
            }
            else if (item.Value != null) 
            {
                // Fallback for string "true" or "false"
                string valueString = item.Value.ToString().Trim().ToLower();
                isCollected = (valueString == "true");
            }
            // --- END ROBUST CHECK ---

            Debug.Log($"Found key: {key}, isCollected: {isCollected}");

            if (isCollected)
            {
                collectedItems.Add(key);

                if (buttonMap.ContainsKey(key))
                {
                    yield return null; 
                    EnableButton(key);
                }
            }
        }
    }

    void EnableButton(string key)
    {
        if (buttonMap.ContainsKey(key))
        {
            buttonMap[key].SetActive(true);
        }
    }
    
    public void SpawnCollectible(string key)
    {
        key = key.Trim().ToLower();

        if (!prefabMap.ContainsKey(key)) 
        {
            Debug.LogError($"SpawnCollectible: Unknown key '{key}'.");
            return;
        }
        
        if (!collectedItems.Contains(key))
        {
            Debug.LogWarning($"User has not collected '{key}'. Spawn attempt blocked.");
            return;
        }

        Transform cam = Camera.main.transform;
        Vector3 spawnPos = cam.position + cam.forward * 0.4f; 
        Quaternion spawnRot = Quaternion.Euler(0, cam.eulerAngles.y, 0); 

        GameObject obj = Instantiate(prefabMap[key], spawnPos, spawnRot);
        obj.transform.localScale = Vector3.one * 0.05f;

        Debug.Log($"Successfully spawned collected item for {key}");
    }
}