using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    [HideInInspector] public int damage;
    [SerializeField] private GameObject bulletImpactPrefab;

    private void Update()
    {
        if (PlayerController.instance)
        {
            if (Vector3.Distance(PlayerController.instance.transform.position, transform.position) <= 1f)
            {
                PlayerController.instance.playerHealth -= damage;
                UIManager.instance.UpdateHealth();
                if (PlayerController.instance.playerHealth > 0)
                    CameraShake.Invoke();
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            GameObject impact = Instantiate(bulletImpactPrefab, other.collider.transform.position, Quaternion.identity);
            Destroy(impact, 1f);
            Destroy(gameObject);
        }
    }
}
