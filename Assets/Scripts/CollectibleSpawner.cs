using UnityEngine;
using System.Collections;
using TMPro;

public class SpawnerManager : MonoBehaviour
{
    public GameObject lockedText;
    public AudioManager audioManager;
    public AudioClip spawnSound;

    // Attempt to spawn a collectible prefab
    public void TrySpawn(GameObject prefab)
    {
        if (prefab == null) return;

        CollectibleIdentity identity = prefab.GetComponent<CollectibleIdentity>(); // Get the identity component from the connected prefab

        if (identity == null)
        {
            Debug.LogError("Prefab missing CollectibleIdentity!");
            return;
        }

        bool isUnlocked = CheckOwnership(identity.type); // Check if user owns this collectible, based on its type
        if (!isUnlocked)
        {
            StartCoroutine(ShowLockedMessage()); // Show locked message if not owned
            return;
        }

        Spawn(prefab); // Spawn the collectible
    }

    bool CheckOwnership(CollectibleType type) // Check if user owns the collectible
    {
        if (UserData.Profile == null)
        {
            Debug.LogWarning("User profile not loaded yet.");
            return false;
        }

        var basic = UserData.Profile.collections.basic;

        switch (type) // Check based on collectible type
        {
            case CollectibleType.Curry:
                return basic.hasCurry;

            case CollectibleType.Wing:
                return basic.hasWing;

            case CollectibleType.Fishballs:
                return basic.hasFBalls;

            case CollectibleType.Sotong:
                return basic.hasSotong;
        }

        return false; // Default to false if type is unrecognized
    }

    void Spawn(GameObject prefab) // Spawn the collectible in front of the camera
    {
        Transform cam = Camera.main.transform;

        Vector3 spawnPos = cam.position + cam.forward * 0.4f;
        Quaternion spawnRot = Quaternion.Euler(0, cam.eulerAngles.y, 0);

        GameObject obj = Instantiate(prefab, spawnPos, spawnRot);
        obj.transform.localScale = Vector3.one * 0.05f;

        Debug.Log("Spawned: " + prefab.name);
    } 

    IEnumerator ShowLockedMessage()
    {
        lockedText.SetActive(true);
        yield return new WaitForSeconds(3.5f);
        lockedText.SetActive(false);
    }
}   
