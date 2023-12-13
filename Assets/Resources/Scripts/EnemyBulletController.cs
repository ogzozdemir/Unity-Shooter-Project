using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    [HideInInspector] public int damage;

    private void Update()
    {
        if (PlayerController.instance && gameObject.activeSelf)
        {
            if (Vector3.Distance(PlayerController.instance.transform.position, transform.position) <= 1f)
            {
                PlayerController.instance.playerHealth -= damage;
                UIManager.instance.GetHit();
                if (PlayerController.instance.playerHealth > 0)
                    CameraShake.Invoke();
                gameObject.SetActive(false);
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground") && gameObject.activeSelf)
        {
            GameObject impact = ObjectPooling.instance.GetPooledBulletHitObject();
            impact.transform.position = other.collider.transform.position;
            impact.SetActive(true);
            
            gameObject.SetActive(false);
        }
    }
}
