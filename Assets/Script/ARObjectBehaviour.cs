using UnityEngine;

public class ARObjectBehaviour : MonoBehaviour
{
    [SerializeField]
    public MeshRenderer meshRendererToToggle;
    public void ToggleMeshRenderer()
    {
        meshRendererToToggle.enabled = !meshRendererToToggle.enabled;
    }
}
