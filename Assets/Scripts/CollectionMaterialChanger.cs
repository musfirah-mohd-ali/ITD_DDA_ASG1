using UnityEngine;

public class CollectionMaterialChanger : MonoBehaviour
{
    public BasicCollectible collectibleType; // Type of collectible; can assign respective bool in inspector via dropdown list

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

        bool isUnlocked = collectibleType switch // Check if the collectible is unlocked
        {
            // Access the respective boolean from UserData.Profile
            BasicCollectible.Curry => UserData.Profile.collections.basic.hasCurry,
            BasicCollectible.Wing => UserData.Profile.collections.basic.hasWing,
            BasicCollectible.Fishballs => UserData.Profile.collections.basic.hasFBalls,
            BasicCollectible.Sotong => UserData.Profile.collections.basic.hasSotong,
            _ => false // Default case
        };

        meshRenderer.material = isUnlocked ? unlockedMaterial : lockedMaterial; // Set material based on unlock status, thanks mr elyas <3
    }
}
