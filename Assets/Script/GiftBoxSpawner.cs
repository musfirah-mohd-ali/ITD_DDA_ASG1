using UnityEngine;

public class GiftBoxSpawner : MonoBehaviour
{
    public GameObject giftBoxPrefab;
    public GameObject openButton; 
    public GiftBoxOpener opener; // reference to second script

    private GameObject currentGiftBox;

    public void SpawnGiftBox()
    {
        // Calculate spawn 20cm in front of camera
        Vector3 spawnPos = Camera.main.transform.position + Camera.main.transform.forward * 0.20f;
        Quaternion spawnRot = Camera.main.transform.rotation;

        // Spawn gift box and store the reference
        currentGiftBox = Instantiate(giftBoxPrefab, spawnPos, spawnRot);

        // tell opener which box to destroy later
        opener.SetGiftBox(currentGiftBox);

        // show open button
        openButton.SetActive(true);
    }
}
