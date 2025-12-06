using UnityEngine;

public class GiftBoxOpener : MonoBehaviour
{
    public GameObject collectiblePrefab;
    public GameObject openButton;

    private GameObject giftBox;

    public void SetGiftBox(GameObject box)
    {
        giftBox = box;
    }

    public void OpenBox()
    {
        // hide button
        openButton.SetActive(false);

        // remove gift box
        if (giftBox != null)
        {
            Destroy(giftBox);
        }

        Vector3 spawnPos = Camera.main.transform.position + Camera.main.transform.forward * 0.20f;
        Quaternion spawnRot = Camera.main.transform.rotation;

        Instantiate(collectiblePrefab, spawnPos, spawnRot);
    }
}
