using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    // Called when the object goes out of the screen
    void OnBecameInvisible()
    {
        Destroy(transform.parent.gameObject);
    }
}