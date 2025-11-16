using UnityEngine;

public class ARObjectBehaviour : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer meshRendererToToggle;

    [SerializeField]
    private AudioSource meshDisableSound;
    [SerializeField]
    private AudioClip disableSoundClip;

    public void ToggleMeshRenderer()
    {
        bool newState = !meshRendererToToggle.enabled;
        meshRendererToToggle.enabled = newState;

        // Play sound ONLY when disabling
        if (newState == false)
        {
            if (meshDisableSound != null)
            {
                meshDisableSound.PlayOneShot(disableSoundClip);
            }
            else
            {
                Debug.LogWarning("No AudioSource assigned for disable sound.");
            }
        }

        Debug.Log("Mesh Renderer is now: " + (meshRendererToToggle.enabled ? "Enabled" : "Disabled"));
    }
}
