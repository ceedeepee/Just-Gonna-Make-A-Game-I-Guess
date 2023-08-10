using UnityEngine;

public class RotateAroundZ : MonoBehaviour
{
    public float rotationSpeed = 360f; // 360 degrees per second by default

    void Update()
    {
        // Rotate the transform around its local Z axis at the specified degrees per second
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
