using UnityEngine;

public class CollectionMaterialChanger : MonoBehaviour
{
    public CollectibleType collectibleType; // Type of collectible; can assign respective bool in inspector via dropdown list

    public Material unlockedMaterial; // Material for unlocked state
    public Material lockedMaterial; // Material for locked state

    private MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        UpdateMaterial();
    }

    public void UpdateMaterial()
    {
        if (UserData.Profile == null)
        {
            Debug.LogWarning("User profile not loaded yet");
            return;
        }

        Debug.Log($"Checking unlock status for {collectibleType}");

        bool isUnlocked = collectibleType switch // Check if the collectible is unlocked
        {
            // Access the respective boolean from UserData.Profile
            CollectibleType.Curry => UserData.Profile.collections.basic.hasCurry,
            CollectibleType.Wing => UserData.Profile.collections.basic.hasWing,
            CollectibleType.Fishballs => UserData.Profile.collections.basic.hasFBalls,
            CollectibleType.Sotong => UserData.Profile.collections.basic.hasSotong,
            _ => false // Default case
        };

        Debug.Log($"Updating material for {collectibleType}: Unlocked = {isUnlocked}");

        meshRenderer.material = isUnlocked ? unlockedMaterial : lockedMaterial; // Set material based on unlock status, thanks mr elyas <3
    }
}
