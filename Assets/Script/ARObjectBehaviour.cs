using UnityEngine;

public class ARObjectBehaviour : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] meshRenderersToToggle;

    public void ToggleMeshes()
    {
        foreach (MeshRenderer r in meshRenderersToToggle)
            r.enabled = !r.enabled;
    }

}