using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private float speed = 50f;
    [SerializeField] private Vector3 rotationAxis = new Vector3(0f, 1f, 0f);

    void Update()
    {
        transform.Rotate(rotationAxis * speed * Time.deltaTime);
    }
}
