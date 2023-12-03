using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    private void Destroy()
    {
        Destroy(gameObject);
    }
}
